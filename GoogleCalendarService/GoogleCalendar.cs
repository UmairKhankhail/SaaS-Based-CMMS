using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.IO;

namespace GoogleCalendarService
{
    public class GoogleCalendar
    {
        private readonly CalendarService _calendarService;
        private string serviceAccountKeyFilePath = "C:\\titancmms-965210431d86.json"; // Replace with your service account key file path

        public GoogleCalendar()
        {
       
            var credential = GetCredential(serviceAccountKeyFilePath);

            _calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "TitanCMMS"
            });
        }

        private ServiceAccountCredential GetCredential(string serviceAccountKeyFilePath)
        {
            GoogleCredential credential;
            using (var stream = new FileStream(serviceAccountKeyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(CalendarService.Scope.Calendar);
            }

            return (ServiceAccountCredential)credential.UnderlyingCredential;
        }

        public string InsertRecurringEvent(Event newEvent, int frequencyInDays, string calendardbId, string timeZone)
        {
            // Set timezone to Pakistan Standard Time
            newEvent.Start.TimeZone = timeZone;
            newEvent.End.TimeZone = timeZone;

            // Create a recurrence rule with the desired frequency
            string recurrence = $"RRULE:FREQ=DAILY;INTERVAL={frequencyInDays};";

            // Set the recurrence rule for the event
            newEvent.Recurrence = new string[] { recurrence };

            // Insert the recurring event into the specified calendar
            var calendarId = calendardbId;
            var createdEvent = _calendarService.Events.Insert(newEvent, calendarId).Execute();

            // Return the created event ID
            return createdEvent.Id;
        }

        public string InsertEvent(Event newEvent, string calendardbId, string timeZone)
        {
            // Set timezone to Pakistan Standard Time
            newEvent.Start.TimeZone = timeZone;
            newEvent.End.TimeZone = timeZone;

            // Insert the event into the specified calendar
            var calendarId = calendardbId;
            var createdEvent = _calendarService.Events.Insert(newEvent, calendarId).Execute();

            // Return the created event ID
            return createdEvent.Id;
        }

        public void UpdateRecurringEvent(string calendarId, string eventId, Event updatedEvent, int frequencyInDays, string timeZone)
        {
            // Update event start and end time zone
            updatedEvent.Start.TimeZone = timeZone;
            updatedEvent.End.TimeZone = timeZone;

            // Create a recurrence rule with the updated frequency
            string recurrence = $"RRULE:FREQ=DAILY;INTERVAL={frequencyInDays};";

            // Set the recurrence rule for the event
            updatedEvent.Recurrence = new string[] { recurrence };

            // Perform the update
            _calendarService.Events.Update(updatedEvent, calendarId, eventId).Execute();
        }

        public void DeleteEvent(string eventId, string calendarId)
        {
            _calendarService.Events.Delete(calendarId, eventId).Execute();
        }

        public Event GetEvent(string eventId, string calendarId)
        {
            var eventResult = _calendarService.Events.Get(calendarId, eventId).Execute();
            return eventResult;
        }

        public string CreateCalendar(string calendarSummary, string calendarDescription, string timeZone)
        {
            var newCalendar = new Calendar
            {
                Summary = calendarSummary,
                Description = calendarDescription,
                TimeZone = timeZone
            };

            var createdCalendar = _calendarService.Calendars.Insert(newCalendar).Execute();

            return createdCalendar.Id;
        }

        public string UpdateCalendar(string calendarId, string calendarSummary, string calendarDescription, string timeZone)
        {
            var updatedCalendar = _calendarService.Calendars.Get(calendarId).Execute();
            updatedCalendar.Summary = calendarSummary;
            updatedCalendar.Description = calendarDescription;
            updatedCalendar.TimeZone = timeZone;

            var updatedCalendarResult = _calendarService.Calendars.Update(updatedCalendar, calendarId).Execute();

            return updatedCalendarResult.Id;
        }

        public void DeleteCalendar(string calendarId)
        {
            _calendarService.Calendars.Delete(calendarId).Execute();
        }
    }
}
