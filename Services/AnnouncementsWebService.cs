using HtmlAgilityPack;
using Municipal_App.Models;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Municipal_App.Services
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// This class is responsible for scraping and loading announcement data from a specified website.
    /// It handles downloading the webpage, parsing the HTML content, and converting the relevant information 
    /// into a list of AnnouncementViewModels, which are then enqueued into an ObservableQueue for display.
    /// </summary>
    internal class AnnouncementsWebService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Url to the website being scraped for announcements
        /// </summary>
        private readonly string _announcementsURL = "https://eventsincapetown.com/blog/";

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The HTML document created from the _announcementsURL after being loaded from the web.
        /// </summary>
        private HtmlDocument HtmlDocument;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes the HTML document for announcements by downloading the content from the announcements URL.
        /// This method uses an HttpClient to asynchronously retrieve the HTML content.
        /// </summary>
        /// <returns>Returns an awaitable Task.</returns>
        private async Task InitializeAnnouncements()
        {
            // Downloading the page from the internet and populating local fields
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(this._announcementsURL);
            this.HtmlDocument = new HtmlDocument();
            this.HtmlDocument.LoadHtml(html);
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Loads the announcements from the website asynchronously and populates the provided ObservableQueue.
        /// Each scraped announcement is converted into an AnnouncementViewModel and enqueued.
        /// </summary>
        /// <param name="announcementQueue">
        /// The ObservableQueue where the scraped announcements are enqueued.
        /// It must be bound to the UI to reflect real-time updates.
        /// </param>
        /// <returns>Returns an awaitable Task.</returns>
        public async Task LoadAnnouncementsAsync(ObservableQueue<AnnouncementViewModel> announcementQueue)
        {
            // First, initializing page
            await this.InitializeAnnouncements();

            // Iterating through all the events
            var announcementNodes = this.HtmlDocument.DocumentNode.SelectNodes("//div[contains(@class, 'jet-smart-listing__post-wrapper')]");
            if (announcementNodes != null)
            {
                foreach (var announcementNode in announcementNodes)
                {
                    var announcement = new AnnouncementViewModel();
                    

                    // Extracting Announcement Title
                    var announcementTitleNode = announcementNode.SelectSingleNode(".//div[@class='jet-smart-listing__post-title post-title-simple']/a");
                    announcement.Title = announcementTitleNode != null ? this.SanitizeWebField(announcementTitleNode.InnerText) : "No Title";

                    // Extracting Announcement Date
                    var announcementDateNode = announcementNode.SelectSingleNode(".//a[contains(@class, 'post__date-link')]/time");
                    announcement.Date = announcementDateNode != null ? this.SanitizeWebField(announcementDateNode.InnerText) : "None";

                    // Extracting Announcement Image 
                    var imageNode = announcementNode.SelectSingleNode(".//img[contains(@class, 'wp-post-image')]");
                    announcement.Image = imageNode != null ? await ConvertImageToBitmap(imageNode) : new BitmapImage();

                    // Extracting Announcement Description
                    var announcementDescriptionNode = announcementNode.SelectSingleNode(".//div[@class='jet-smart-listing__post-excerpt post-excerpt-simple']");
                    announcement.Description = announcementDescriptionNode != null ? this.SanitizeWebField(announcementDescriptionNode.InnerText) : "No Description";

                    announcementQueue.Enqueue(announcement);
                    AppStore.Instance.AnnouncementsStore.UpdateSortedDictionary(announcement);
                }
            }
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Converts an image from an HtmlNode to a BitmapImage. The image URL can be retrieved from
        /// either the "data-lazy-src" or "src" attribute of the node.
        /// </summary>
        /// <param name="imageNode">The HtmlNode representing the image element.</param>
        /// <returns>A task that contains the converted BitmapImage.</returns>
        private async Task<BitmapImage> ConvertImageToBitmap(HtmlNode imageNode)
        {
            if (imageNode != null)
            {
                // First, try to get the image URL from 'src' or 'data-lazy-src'
                string imageUrl = imageNode.GetAttributeValue("data-lazy-src", string.Empty);

                if (string.IsNullOrEmpty(imageUrl))
                {
                    imageUrl = imageNode.GetAttributeValue("src", string.Empty);
                }

                // Check if it's a data URL (inline image)
                if (imageUrl.StartsWith("data:image"))
                {
                    return ConvertDataUriToBitmap(imageUrl);  // Handle inline data URIs
                }
                else if (!string.IsNullOrEmpty(imageUrl))
                {
                    // Download the image data from a regular URL
                    HttpClient httpClient = new HttpClient();
                    byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);

                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze();
                    }

                    return bitmap;
                }
            }

            // Handle failure to get an image
            Console.WriteLine("Failed to get image");
            return new BitmapImage();
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Converts a Base64-encoded data URI into a BitmapImage.
        /// This method handles inline images that are encoded directly in the HTML.
        /// </summary>
        /// <param name="dataUri">The Base64-encoded image data URI.</param>
        /// <returns>A BitmapImage constructed from the data URI.</returns>
        private BitmapImage ConvertDataUriToBitmap(string dataUri)
        {
            var base64Data = dataUri.Substring(dataUri.IndexOf(",") + 1);
            byte[] imageData = Convert.FromBase64String(base64Data);

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze(); // Freeze the BitmapImage for better performance
            }
            return bitmap;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sanitizes an HTML field by decoding and trimming its content to ensure clean text.
        /// </summary>
        /// <param name="fieldValue">The HTML content to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        private string SanitizeWebField(string fieldValue)
        {
            return System.Net.WebUtility.HtmlDecode(fieldValue.Trim());
        }
    }
}
//---------------------------------------EOF-------------------------------------------