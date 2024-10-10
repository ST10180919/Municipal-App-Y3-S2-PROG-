using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Services
{
    internal class EventsWebService
    {
        private readonly string _eventsURL = "https://eventsincapetown.com/all-events/";
        private HtmlDocument _htmlDocument;

        public EventsWebService()
        {}

        public async Task InitializeEvents()
        {
            // Downloading the page from the internet
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(this._eventsURL);
            this._htmlDocument = new HtmlDocument();
            this._htmlDocument.LoadHtml(html);
        }

        public async Task<string> GetEventsTestString()
        {
            var events = new StringBuilder();

            // Selecting all events
            var eventNodes = this._htmlDocument.DocumentNode.SelectNodes("//article[contains(@class, 'mec-event-article') and contains(@class, 'mec-clear')]");

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
