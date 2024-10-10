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
    internal class EventsWebService
    {
        private readonly string _eventsURL = "https://eventsincapetown.com/all-events/";
        private HtmlDocument HtmlDocument;

        public EventsWebService()
        {}

        public async Task InitializeEvents()
        {
            // Downloading the page from the internet and populating local fields
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(this._eventsURL);
            this.HtmlDocument = new HtmlDocument();
            this.HtmlDocument.LoadHtml(html);
        }

        public async Task LoadEventsAsync(ObservableQueue<MunicipalEventViewModel> eventsQueue)
        {
            // First, initializing page
            await this.InitializeEvents();

            // Iterating through all the events
            var eventNodes = this.HtmlDocument.DocumentNode.SelectNodes("//article[contains(@class, 'mec-event-article') and contains(@class, 'mec-clear')]");
            if (eventNodes != null)
            {
                foreach (var eventElement in eventNodes)
                {
                    var municipalEvent = new MunicipalEventViewModel();

                    // Extracting Event Title
                    var eventTitleNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                    municipalEvent.Title = eventTitleNode != null ? eventTitleNode.InnerText.Trim() : "No Title";

                    // Extracting Event Category
                    var eventCategoryNode = eventElement.SelectSingleNode(".//span[@class='mec-category']");
                    municipalEvent.Category = eventCategoryNode != null ? eventCategoryNode.InnerText.Trim() : "None";

                    // Extracting Event Image 
                    var imageNode = eventElement.SelectSingleNode(".//div[@class='mec-event-image']//a//img");
                    municipalEvent.Image = imageNode != null ? await ConvertImageToBitmap(imageNode) : new BitmapImage(); 

                    // Adding the created event to the queue prior to all details being loaded
                    eventsQueue.Enqueue(municipalEvent);

                    // Extracting Event Details Link
                    var eventDetailsLinkNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                    var eventDetailsLink = eventDetailsLinkNode != null ? eventDetailsLinkNode.GetAttributeValue("href", "No Link") : "No Link";

                    // Loading details for the event's specific page in the background
                    LoadEventDetails(municipalEvent, eventDetailsLink);
                }
            }
        }

        private async Task LoadEventDetails(MunicipalEventViewModel municipalEvent, string eventDetailsLink)
        {
            if (eventDetailsLink != string.Empty)
            {
                // Getting the document for the page
                HttpClient httpClient = new HttpClient();
                string html = await httpClient.GetStringAsync(eventDetailsLink);
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                // Extracting Event Date
                var dateNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Event Date:')]]");
                municipalEvent.Date = dateNode != null ? dateNode.InnerText.Replace("Event Date:", "").Trim() : "Unspecified";

                // Extracting Event Time
                var timeNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Event Time:')]]");
                municipalEvent.Time = timeNode != null ? timeNode.InnerText.Replace("Event Time:", "").Trim() : "--:--";

                // Extracting Event Venue
                var venueNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Venue:')]]");
                municipalEvent.Venue = venueNode != null ? venueNode.InnerText.Replace("Venue:", "").Trim() : "Unspecified";

                // Extracting Event Website Link 
                var eventLinkNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Website:')]]");
                municipalEvent.Link = eventLinkNode != null ? eventLinkNode.InnerText.Replace("Website:", "").Trim() : "Unspecified";
            }
            else
            {
                municipalEvent.Date = "Unspecified";
                municipalEvent.Time = "--:--";
                municipalEvent.Venue = "Unspecified";
                municipalEvent.Link = "Unspecified";
            }
        }

        private async Task<BitmapImage> ConvertImageToBitmap(HtmlNode imageNode)
        {
            if (imageNode != null)
            {
                // Get the image URL from the src attribute
                string imageUrl = imageNode.GetAttributeValue("data-lazy-src", string.Empty);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    // Download the image data
                    HttpClient httpClient = new HttpClient();
                    byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);

                    // Convert the byte array to a BitmapImage
                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze(); // Optional: Freeze the BitmapImage for better performance
                    }

                    // Return the BitmapImage to be used in the view
                    return bitmap;
                }
            }
            else
            {
                Console.WriteLine("Failed to get image"); // Properly set this to Image not found image
            }
            return new BitmapImage();
        }

        public async Task<BitmapImage> GetImageTest()
        {
            // Selecting an image 
            var imageNode = this.HtmlDocument.DocumentNode.SelectSingleNode("//div[@class='mec-event-image']//img");

            if (imageNode != null)
            {
                // Get the image URL from the src attribute
                string imageUrl = imageNode.GetAttributeValue("data-lazy-src", string.Empty);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    // Download the image data
                    HttpClient httpClient = new HttpClient();
                    byte[] imageData = await httpClient.GetByteArrayAsync(imageUrl);

                    // Convert the byte array to a BitmapImage
                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream stream = new MemoryStream(imageData))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                        bitmap.Freeze(); // Optional: Freeze the BitmapImage for better performance
                    }

                    // Return the BitmapImage to be used in the view
                    return bitmap;
                }
            }
            else
            {
                Console.WriteLine("Failed to get image");
            }
            return null;
        }

        public async Task<string> GetEventsTestString()
        {
            var events = new StringBuilder();

            // Selecting all events
            var eventNodes = this.HtmlDocument.DocumentNode.SelectNodes("//article[contains(@class, 'mec-event-article') and contains(@class, 'mec-clear')]");

            // Iterating through each event element
            if (eventNodes != null)
            {
                foreach (var eventElement in eventNodes)
                {
                    // Extracting Event Title
                    var eventTitleNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                    string eventTitle = eventTitleNode != null ? eventTitleNode.InnerText.Trim() : "No Title";

                    // Extracting Event Date
                    var eventDateNode = eventElement.SelectSingleNode(".//div[@class='mec-event-date mec-bg-color']");
                    string eventDate = eventDateNode != null ? eventDateNode.InnerText.Trim() : "No Date";

                    // Extracting Event Link
                    var eventLinkNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                    string eventLink = eventLinkNode != null ? eventLinkNode.GetAttributeValue("href", "No Link") : "No Link";

                    var eventDetails = "No details found";
                    if (eventLink != string.Empty)
                    {
                        eventDetails = await InitializeEventDetailsPage(eventLink);
                    }

                    // Building the event details string
                    events.AppendLine("Event Title: " + eventTitle);
                    events.AppendLine("Event Date: " + eventDate);
                    events.AppendLine(eventDetails);
                    events.AppendLine("Event Link :" + eventLink);
                    events.AppendLine("------------------------------------");
                }
            }
            return events.ToString();
        }

        private async Task<string> InitializeEventDetailsPage(string eventLink)
        {
            var detailsBuilder = new StringBuilder();

            // Getting the document for the page
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(eventLink);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            // Getting the event Time
            var timeNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Event Time:')]]");
            if (timeNode != null)
            {
                var timeText = timeNode.InnerText.Trim();
                detailsBuilder.AppendLine(timeText);
            }
            else
            {
                detailsBuilder.AppendLine("Time: Not Found");
            }

            // Getting the venue
            var venueNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Venue:')]]");

            // Extracting the venue text (if the venue node is found)
            if (venueNode != null)
            {
                string venueText = venueNode.InnerText.Trim();
                detailsBuilder.AppendLine(venueText);
            }
            else
            {
                detailsBuilder.AppendLine("Venue: Not Found");
            }

            return detailsBuilder.ToString();
        }
    }
}
