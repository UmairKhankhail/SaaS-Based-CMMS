using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;

namespace GoogleCalendarService
{
    public class GoogleCalendar
    {
        private readonly CalendarService _calendarService;

        public GoogleCalendar()
        {
            var credential = GetCredential();

            _calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar Api"
            });
        }

        private UserCredential GetCredential()
        {
            // Specify the path to your client secrets JSON file
            string clientSecretsFilePath = "C:\\TitanCMMSCloud\\GoogleCalendarService\\client_secret_123.json";

            // Specify the scopes required for accessing the Calendar API
            string[] scopes = { CalendarService.Scope.Calendar };

            // Load the client secrets from the JSON file
            using (var stream = new FileStream(clientSecretsFilePath, FileMode.Open, FileAccess.Read))
            {
                var credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    scopes,
                    "user",
                    System.Threading.CancellationToken.None
                    
                ).Result;

                return credentials;
            }
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

            // Insert the recurring event into the primary calendar
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

            // Insert the recurring event into the primary calendar
            var calendarId = calendardbId;
            var createdEvent = _calendarService.Events.Insert(newEvent, calendarId).Execute();

            // Return the created event ID
            return createdEvent.Id;
        }

        //public string InsertRecurringEvent(Event newEvent, int recurrenceCount)
        //{
        //    // Set timezone to Pakistan Standard Time
        //    newEvent.Start.TimeZone = "Asia/Karachi";
        //    newEvent.End.TimeZone = "Asia/Karachi";

        //    // Set recurrence
        //    newEvent.Recurrence = new string[] { $"RRULE:FREQ=DAILY;COUNT={recurrenceCount}" };

        //    // Insert the recurring event into the primary calendar
        //    var calendarId = "primary";
        //    var createdEvent = _calendarService.Events.Insert(newEvent, calendarId).Execute();

        //    // Return the created event ID
        //    return createdEvent.Id;
        //}

        //public string UpdateEvent(string eventId, Event updatedEvent)
        //{
        //    // Set timezone to Pakistan Standard Time
        //    updatedEvent.Start.TimeZone = "Asia/Karachi";
        //    updatedEvent.End.TimeZone = "Asia/Karachi";

        //    // Update the event in the primary calendar
        //    var calendarId = "primary";
        //    var updatedEventResult = _calendarService.Events.Update(updatedEvent, calendarId, eventId).Execute();

        //    // Return the updated event ID
        //    return updatedEventResult.Id;
        //}

        public void UpdateRecurringEvent(string calendarId, string eventId, Event updatedEvent, int frequencyInDays, string timeZone)
        {
            // Set the eventId of the recurring event to be updated

            // Update the event start and end time zone if needed
            updatedEvent.Start.TimeZone = timeZone;
            updatedEvent.End.TimeZone = timeZone;

            // Create a recurrence rule with the updated frequency
            string recurrence = $"RRULE:FREQ=DAILY;INTERVAL={frequencyInDays};";

            // Set the recurrence rule for the event
            updatedEvent.Recurrence = new string[] { recurrence };

            // Perform the update
            _calendarService.Events.Update(updatedEvent, calendarId, eventId).Execute();
        }


        public void DeleteEvent(string eventId, string calenderDbId)
        {
            // Delete the event from the primary calendar
            var calendarId = calenderDbId;
            _calendarService.Events.Delete(calendarId, eventId).Execute();
        }

        public Event GetEvent(string eventId, string calenderDbId)
        {
            // Retrieve the event from the primary calendar
            var calendarId = calenderDbId;
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
