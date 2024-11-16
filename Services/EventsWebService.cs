﻿using HtmlAgilityPack;
using Municipal_App.Models;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Municipal_App.Services
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Service used to scrape Events from the CapeTown Municipal Webiste. Contains
    /// Asynchronous operations to ensure information is shown to the user as soon
    /// as possible.
    /// </summary>
    internal class EventsWebService
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Url to the website being scraped for events
        /// </summary>
        private readonly string _eventsURL = "https://eventsincapetown.com/all-events/";

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Html document created from the _eventsURL
        /// </summary>
        private HtmlDocument HtmlDocument;

        private bool isOfflineMode = false;

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the Event Document by loading it from the _eventsURL
        /// </summary>
        /// <returns> Awaitable Task </returns>
        private async Task InitializeEvents()
        {
            // Downloading the page from the internet and populating local fields
            try
            {
                HttpClient httpClient = new HttpClient();
                string html = await httpClient.GetStringAsync(this._eventsURL);
                this.HtmlDocument = new HtmlDocument();
                this.HtmlDocument.LoadHtml(html);

                //SaveHtmlDocument(this.HtmlDocument, "OfflineData", "all-events.html");
            }
            catch (HttpRequestException)
            {
                MessageBox.Show($"Failed to download webpage ${_eventsURL}.\n Events will now be loaded from offline backup");
                isOfflineMode = true;
                this.HtmlDocument = LoadHtmlDocument("OfflineData", "all-events.html");
            }
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// Loads the Events from the website asynchronously, populating the fields of 
        /// MunicipalEventViewModels which are then enqueued into the eventsQueue param.
        /// 
        /// Since the MunicipalEventViewModel implements INotifyPropertyChanged, 
        /// the UI is updated the moment a field is set.
        /// </summary>
        /// 
        /// <param name="eventsQueue"> 
        /// An ObservableQueue for which scraped events will be enqueued, which must be 
        /// bound to the UI 
        /// </param>
        /// <returns> Awaitable Task </returns>
        public async Task LoadEventsAsync(ObservableQueue<EventViewModel> eventsQueue)
        {
            try
            {
                // First, initializing page
                await this.InitializeEvents();

                if (this.HtmlDocument == null)
                {
                    return;
                }

                // Iterating through all the events
                var eventNodes = this.HtmlDocument.DocumentNode.SelectNodes("//article[contains(@class, 'mec-event-article') and contains(@class, 'mec-clear')]");
                if (eventNodes != null)
                {
                    foreach (var eventElement in eventNodes)
                    {
                        var municipalEvent = new EventViewModel();

                        // Extracting Event Title
                        var eventTitleNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                        municipalEvent.Title = eventTitleNode != null ? this.SanitizeWebField(eventTitleNode.InnerText) : "No Title";

                        // Extracting Event Category
                        var eventCategoryNode = eventElement.SelectSingleNode(".//span[@class='mec-category']");
                        municipalEvent.Category = eventCategoryNode != null ? this.SanitizeWebField(eventCategoryNode.InnerText) : "None";

                        // Extracting Event Image 
                        var imageNode = eventElement.SelectSingleNode(".//div[@class='mec-event-image']//a//img");
                        municipalEvent.Image = imageNode != null ? await ConvertImageToBitmap(imageNode) : new BitmapImage();

                        // Adding the created event to the queue prior to all details being loaded
                        eventsQueue.Enqueue(municipalEvent);

                        // Extracting Event Details Link
                        var eventDetailsLinkNode = eventElement.SelectSingleNode(".//h4[@class='mec-event-title']/a");
                        var eventDetailsLink = eventDetailsLinkNode != null ? eventDetailsLinkNode.GetAttributeValue("href", "No Link") : "No Link";

                        LoadEventDetails(municipalEvent, eventDetailsLink);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //---------------------------------------------------------------------------------
        /// <summary>
        /// A helper method for LoadEventsAsync. Loads the event details nested in the 
        /// document linked to by the main site. This method is Asynchronous because
        /// the added processing time for loading the html document for each event
        /// is great.
        /// </summary>
        /// 
        /// <param name="municipalEvent">
        /// The MunicipalEventViewModel for which details are being set
        /// </param>
        /// <param name="eventDetailsLink">
        /// A string containing the URL to the page holding the event details for the 
        /// municipal event
        /// </param>
        /// <returns> Awaitable Task </returns>
        private async Task LoadEventDetails(EventViewModel municipalEvent, string eventDetailsLink)
        {
            try
            {
                if (eventDetailsLink != string.Empty)
                {
                    // Getting the document for the page
                    var htmlDocument = new HtmlDocument();
                    if (!isOfflineMode)
                    {
                        // When online
                        HttpClient httpClient = new HttpClient();
                        string html = await httpClient.GetStringAsync(eventDetailsLink);
                        htmlDocument.LoadHtml(html);
                    } 
                    else
                    {
                        // When offline
                        htmlDocument = LoadHtmlDocument("OfflineData", $"{municipalEvent.Title}.html");
                    }

                    //SaveHtmlDocument(this.HtmlDocument, "OfflineData", $"{municipalEvent.Title}.html");

                    // Extracting Event Date
                    var dateNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Event Date:')] or " +
                        "strong[contains(text(), 'Date:')] or " +
                        "strong[contains(text(), 'Dates:')] or " +
                        "strong[contains(text(), 'Event Dates:')]]");
                    municipalEvent.Date = dateNode != null ? this.SanitizeWebField(dateNode.InnerText) : "Unspecified";

                    // Extracting Event Time
                    var timeNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[" +
                        "contains(text(), 'Event Time:')] or " +
                        "strong[contains(text(), 'Event Times:')] or " +
                        "strong[contains(text(), 'Time:')] or " +
                        "strong[contains(text(), 'Times:')]]");
                    municipalEvent.Time = timeNode != null ? this.SanitizeWebField(timeNode.InnerText) : "--:--";

                    // Extracting Event Venue
                    var venueNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Venue:')]]");
                    municipalEvent.Venue = venueNode != null ? this.SanitizeWebField(venueNode.InnerText) : "Unspecified";

                    // Extracting Event Website Link 
                    var eventLinkNode = htmlDocument.DocumentNode.SelectSingleNode("//li[strong[contains(text(), 'Website:')]]");
                    municipalEvent.Link = eventLinkNode != null ? this.SanitizeWebField(eventLinkNode.InnerText) : "Unspecified";

                    // Update EventsStore Fields
                    AppStore.Instance.EventsStore.UpdateFields(municipalEvent);
                }
                else
                {
                    municipalEvent.Date = "Unspecified";
                    municipalEvent.Time = "--:--";
                    municipalEvent.Venue = "Unspecified";
                    municipalEvent.Link = "Unspecified";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

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

        /// <summary>
        /// Saves an HtmlDocument to a specified folder relative to the project root.
        /// </summary>
        /// <param name="htmlDocument">The HtmlDocument to save.</param>
        /// <param name="relativeFolderPath">The relative folder path from the project root.</param>
        /// <param name="fileName">The name of the file to save (including extension).</param>
        public void SaveHtmlDocument(HtmlDocument htmlDocument, string relativeFolderPath, string fileName)
        {
            try
            {
                // Get the directory of the executable
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Move up directories to get to the project root
                // Adjust the number of Parent calls based on your project structure
                string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.FullName;

                // Combine the project root with the relative folder path
                string fullFolderPath = Path.Combine(projectRoot, relativeFolderPath);

                // Ensure the directory exists
                Directory.CreateDirectory(fullFolderPath);

                // Combine the full folder path with the file name
                string fullFilePath = Path.Combine(fullFolderPath, fileName);

                // Save the HtmlDocument to the specified file
                htmlDocument.Save(fullFilePath);

                Console.WriteLine($"File saved successfully at: {fullFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving HTML document: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads an HtmlDocument from a specified file in the OfflineData folder.
        /// </summary>
        /// <param name="relativeFolderPath">The relative folder path from the project root (e.g., "OfflineData/EventDetails").</param>
        /// <param name="fileName">The name of the HTML file to load (including extension).</param>
        /// <returns>The loaded HtmlDocument, or null if the file does not exist.</returns>
        public HtmlDocument LoadHtmlDocument(string relativeFolderPath, string fileName)
        {
            try
            {
                // Get the directory of the executable
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                // Move up directories to get to the project root
                // Adjust the number of Parent calls based on your project structure
                string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.FullName;

                // Combine the project root with the relative folder path and file name
                string fullFilePath = Path.Combine(projectRoot, relativeFolderPath, fileName);

                // Check if the file exists
                if (!File.Exists(fullFilePath))
                {
                    Console.WriteLine($"File not found: {fullFilePath}");
                    return null;
                }

                // Load the HTML document from the file
                var htmlDocument = new HtmlDocument();
                htmlDocument.Load(fullFilePath);

                Console.WriteLine($"File loaded successfully from: {fullFilePath}");

                return htmlDocument;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading HTML document: {ex.Message}");
                return null;
            }
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Sanitizes an HTML field by decoding and trimming its content to ensure clean text.
        /// Also removes common redundant strings from the fieldValue.
        /// </summary>
        /// <param name="fieldValue">The HTML content to sanitize.</param>
        /// <returns>The sanitized string.</returns>
        private string SanitizeWebField(string fieldValue)
        {
            return System.Net.WebUtility.HtmlDecode(
                fieldValue
                .Replace("Event Date:", "")
                .Replace("Event Time:", "")
                .Replace("Event Times:", "")
                .Replace("Time:", "")
                .Replace("Times:", "")
                .Replace("Venue:", "")
                .Replace("Website:", ""))
                .Trim();
        }
    }
}
//---------------------------------------EOF-------------------------------------------