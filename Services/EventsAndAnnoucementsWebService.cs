using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Services
{
    internal class EventsAndAnnoucementsWebService
    {
        public EventsAndAnnoucementsWebService()
        {
        }

        public async Task<string> GetEvents()
        {
            var eventsTitles = new StringBuilder();

            // URL of the web page you want to parse
            string url = "https://eventsincapetown.com/all-events/";

            // Initialize HttpClient to download the page
            HttpClient httpClient = new HttpClient();

            // Download the HTML content as a string
            string html = await httpClient.GetStringAsync(url);

            // Create an HtmlDocument object
            HtmlDocument htmlDocument = new HtmlDocument();

            // Load the HTML into the document
            htmlDocument.LoadHtml(html);

            // Select all the event titles (using XPath for the specific structure in your HTML)
            var eventTitleNodes = htmlDocument.DocumentNode.SelectNodes("//h4[@class='mec-event-title']/a");

            // Check if nodes are found
            if (eventTitleNodes != null)
            {
                foreach (var eventTitle in eventTitleNodes)
                {
                    string titleText = eventTitle.InnerText.Trim();

                    eventsTitles.AppendLine(titleText);
                    Console.WriteLine(titleText);
                }
            }
            else
            {
                Console.WriteLine("No event titles found.");
            }

            return eventsTitles.ToString();
        }
    }
}
