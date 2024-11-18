# Startup Guide
## How to run the application
Navigate to bin -> Release -> run Municipal App.exe
#### or 
Open Municipal App.exe in Visual Studio and click the run button

# Features
## Landing page
1. Navigation to the Report Issues page
2. Navigation to the Events and Announcements page
3. Navigation to the Service Request Status page
4. Feedback section: For this page, users can suggest new services to add to the app. (Engagement Feature)

## Report Issue page
#### 1. Creating an Issue report:
  1. Location of issue input
  2. Category of issue selection
  3. Description of issue input
  4. Suggesting a solution input (Engagement Feature)
#### 2. Feedback section: For this page, users can suggest improvements to the report issue service

## Events and Announcements page
**Note:** Access to the internet is required for this page to work as the events and announcemetns are scraped from
https://eventsincapetown.com/all-events/ and https://eventsincapetown.com/blog/ respectively.

### Searching For Events and Announcements
You can search for events and annoucements using the searchbar, which searches the titles of both.
The search button needs to be clicked for the search to apply.
You can reset the search terms by simply emptying the search bar and clicking the search button.

### Filtering Events and Annoucements
Filtering either events or announcements can be accessed via the filter button next at the top of the respective section.

Upon clicking the filter button a popup will show allowing for the filtering by category and date period for events
and by date for annoucements.

Filtering can be done in conjunction with searching.

### Recommendations
To trigger recommendations for either events or annoucements, a specific event/announcement's details must match 
with at least three previously searched/filtered terms. 

**For example, If an event's title contains a previously searched term, and its category has been filtered by before, and it's part of a date group that's been filtered by before**

Code for recommendations can be found in RecommendationService.GetRecommendation()

## Service Request Status Page
The purpose of this page is to allow the user to view the status of service requests they've made previously.
**For example, an issue report is considered a service request.**
You can create new service requests by adding them through the report issues page from part 1.

**On this page you can:**
- View previously created service requests on the list seen on the right of the page.
- Search for specific requests using the search bar, which will update the list with matching results.
- Filter requests by a specific category, which will also be combined with any current search terms.
- View the Most recent request made at the top right of the page (if there are any)
- Click on a service request to view the details of that request.

Link to GitHub Repo: https://github.com/ST10180919/Municipal-App-Y3-S2-PROG- (There are releases for each POE part)

All icons were sourced for free from www.flaticon.com

