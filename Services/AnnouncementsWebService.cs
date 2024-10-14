using HtmlAgilityPack;
using Municipal_App.Models;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Municipal_App.Services
{
    internal class AnnouncementsWebService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Url to the website being scraped for announcements
        /// </summary>
        private readonly string _announcementsURL = "https://eventsincapetown.com/blog/";

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Html document created from the _announcementsURL
        /// </summary>
        private HtmlDocument HtmlDocument;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes the Announcement Document by loading it from the _announcementsURL
        /// </summary>
        /// <returns> Awaitable Task </returns>
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
        /// Loads the Announcements from the website asynchronously, populating the fields 
        /// of AnnouncementViewModels which are then enqueued into the announcementQueue 
        /// param.
        /// 
        /// Since the AnnouncementViewModels implements INotifyPropertyChanged, 
        /// the UI is updated the moment a field is set.
        /// </summary>
        /// 
        /// <param name="announcementQueue"> 
        /// An ObservableQueue for which scraped announcements will be enqueued, which must be 
        /// bound to the UI 
        /// </param>
        /// <returns> Awaitable Task </returns>
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

                    // Load additional pages
                    //ScrapeMultiplePages();
                }
            }
        }

        //public async Task ScrapeMultiplePages()
        //{
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        string baseUrl = "https://eventsincapetown.com/blog/?nocache=";
        //        string ajaxQuery = "&jet_blog_ajax=1";

        //        int pageNumber = 1728783634; // Starting number, you can adjust this
        //        bool hasMorePages = true;

        //        while (hasMorePages)
        //        {
        //            // Construct the URL for the POST request
        //            string url = $"{baseUrl}{pageNumber}{ajaxQuery}";

        //            // Set up the POST request (if there is any POST data, you need to include it here)
        //            var postData = new StringContent(""); // If POST data is required, add it here

        //            // Send the POST request
        //            HttpResponseMessage response = await httpClient.PostAsync(url, postData);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string pageContent = await response.Content.ReadAsStringAsync();

        //                // Process the page content (e.g., parse it with HtmlAgilityPack)
        //                Console.WriteLine($"Processing Page {pageNumber}");

        //                // Check if there's more content. This logic will depend on how the pagination works.
        //                hasMorePages = !pageContent.Contains("No more content");;

        //                // Increment the page number for the next request
        //                pageNumber++;
        //            }
        //            else
        //            {
        //                Console.WriteLine($"Failed to load page {pageNumber}. Status code: {response.StatusCode}");
        //                hasMorePages = false;
        //            }
        //        }
        //    }
        //}

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Converts the source of an HtmlNode's image to a bitmap.
        /// </summary>
        /// <param name="imageNode"> HtmlNode where the image is located </param>
        /// <returns> A task containing the converted image </returns>
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

        private BitmapImage ConvertDataUriToBitmap(string dataUri)
        {
            // Extract the Base64-encoded string (remove the "data:image/svg+xml;base64," part)
            var base64Data = dataUri.Substring(dataUri.IndexOf(",") + 1);
            byte[] imageData = Convert.FromBase64String(base64Data);

            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze(); // Optional: Freeze the BitmapImage for better performance
            }

            return bitmap;
        }


        private string SanitizeWebField(string fieldValue)
        {
            return System.Net.WebUtility.HtmlDecode(fieldValue.Trim());
        }
    }
}
