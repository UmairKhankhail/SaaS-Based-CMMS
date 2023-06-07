using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledWorkRequestsController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly GoogleCalendar _calendar;

        public ScheduledWorkRequestsController(PreventiveMaintenanceDbContext context, GoogleCalendar calendar)
        {
            _context = context;
            _calendar = calendar;
        }

        // GET: api/ScheduledWorkRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledWorkRequest>>> GetscheduledWorkRequests()
        {
            // Assuming you have a valid _calendarService object

            // Define the calendar ID of the calendar you want to retrieve events from
            // Assuming you have a valid _calendarService object

            // Define the calendar ID of the calendar you want to retrieve events from
            //string calendarId = "primary"; // Use "primary" for the primary calendar

            //GoogleCalendar googleCalender = new GoogleCalendar();
            ////Event newEvent = new Event
            ////{
            ////    Summary = "Scheduled Work",
            ////    Start = new EventDateTime { DateTime = swr.initialDateTime },
            ////    End = new EventDateTime { DateTime = swr.initialDateTime.AddMinutes(30) }, // Adjust the end time as needed
            ////    Description = swr.description
            ////};
            //var eventId = googleCalender.GetEvent(eId);
            //Console.WriteLine(eventId.Start.DateTime);
            //Console.WriteLine(eventId.End.DateTime);



            return await _context.scheduledWorkRequests.ToListAsync();
        }

        // GET: api/ScheduledWorkRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledWorkRequest>> GetScheduledWorkRequest(int id)
        {
            var scheduledWorkRequest = await _context.scheduledWorkRequests.FindAsync(id);

            if (scheduledWorkRequest == null)
            {
                return NotFound();
            }

            return scheduledWorkRequest;
        }

        // PUT: api/ScheduledWorkRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        
        [HttpGet("GetCalendar")]
        public async Task<ActionResult<GoogleCalendarRecord>> GetCalendar(int id)
        {
            var existingmodel = await _context.googleCalendarRecords.FindAsync(id);

            if (existingmodel == null)
            {
                return NotFound();
            }

            return existingmodel;
        }

        [HttpPost("PostGoogleCalendarApi")]
        public async Task<ActionResult> PostGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            int getgcrId=0;
            GoogleCalendarRecord gcr = new GoogleCalendarRecord();
            gcr.calendarSummary = googleCalendarRecord.calendarSummary;
            gcr.calendarDescription = googleCalendarRecord.calendarDescription;
            gcr.iFrame = googleCalendarRecord.iFrame.ToString();
            gcr.timeZoneRegional = googleCalendarRecord.timeZoneRegional;
            gcr.timeZoneWord = googleCalendarRecord.timeZoneWord;
            gcr.timeZoneGMT = googleCalendarRecord.timeZoneGMT;
            gcr.companyId = googleCalendarRecord.companyId;
           

            GoogleCalendar gc = new GoogleCalendar();
            var gcId=gc.CreateCalendar(gcr.calendarSummary, gcr.calendarDescription, gcr.timeZoneRegional);
            gcr.googleCalendarId = gcId;
            _context.googleCalendarRecords.Add(gcr);
            await _context.SaveChangesAsync();
            getgcrId = gcr.googleCalendarAutoId;

            return Ok();
        }
        [HttpPut("UpdateGoogleCalendarApi")]
        public async Task<IActionResult> UpdateGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            var existingModel = await _context.googleCalendarRecords.FindAsync(googleCalendarRecord.googleCalendarAutoId);
            if (existingModel != null)
            {
                existingModel.calendarDescription = googleCalendarRecord.calendarDescription;
                existingModel.calendarSummary = googleCalendarRecord.calendarSummary;
                existingModel.timeZoneRegional = googleCalendarRecord.timeZoneRegional;
                existingModel.timeZoneWord = googleCalendarRecord.timeZoneWord;
                existingModel.timeZoneGMT = googleCalendarRecord.timeZoneGMT;
                existingModel.iFrame = googleCalendarRecord.iFrame.ToString();
                existingModel.googleCalendarId = googleCalendarRecord.googleCalendarId;

                try
                {
                    await _context.SaveChangesAsync();
                    GoogleCalendar gc = new GoogleCalendar();
                    gc.UpdateCalendar(existingModel.googleCalendarId, existingModel.calendarSummary, existingModel.calendarDescription, existingModel.timeZoneRegional);
                    return Ok();
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return Unauthorized();
        }

        [HttpDelete("RemoveGoogleCalendarApi")]
        public async Task<IActionResult> DeleteGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            var existingModel = await _context.googleCalendarRecords.FindAsync(googleCalendarRecord.googleCalendarAutoId);
            if (existingModel != null)
            {
                try
                {
                    _context.googleCalendarRecords.Remove(existingModel);
                    await _context.SaveChangesAsync();
                    GoogleCalendar gc = new GoogleCalendar();
                    gc.DeleteCalendar(existingModel.googleCalendarId);
                    return Ok();
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            return Unauthorized();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutScheduledWorkRequest(ScheduledWorkRequest scheduledWorkRequest)
        {
            var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == scheduledWorkRequest.companyId).FirstOrDefaultAsync();
            if (calInfo == null)
            {
                return Unauthorized();
            }

            var existingModel = await _context.scheduledWorkRequests.FindAsync(scheduledWorkRequest.swrAutoId);

            existingModel.initialDateTime = scheduledWorkRequest.initialDateTime;
            existingModel.description= scheduledWorkRequest.description;
            existingModel.frequencyDays = scheduledWorkRequest.frequencyDays;

            try
            {
                GoogleCalendar googleCalendar = new GoogleCalendar();

                //DateTime conversion to users timezone
                var startDateTime = existingModel.initialDateTime;
                var endDateTime = startDateTime.AddDays(existingModel.frequencyDays);
                //Conversion Ends

                Event newEvent = new Event
                {
                    Summary = "Recurrent Scheduled Work Request",
                    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    Description = existingModel.description.ToString() + "/n" + existingModel.assetId + "/n" + "Updated"
                };

                googleCalendar.UpdateRecurringEvent(calInfo.googleCalendarId, existingModel.eventIdCalendar, newEvent, existingModel.frequencyDays, calInfo.timeZoneRegional);
                //Db DateTime Work to exact same format
                DateTime inputDateTime = scheduledWorkRequest.initialDateTime;
                string timeZoneId = calInfo.timeZoneWord;
                // Convert parsed datetime to universal datetime
                DateTime universalDateTime = inputDateTime.ToUniversalTime();

                // Convert universal datetime to desired timezone
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                existingModel.initialDateTime = convertedDateTime;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
         
        }


        // POST: api/ScheduledWorkRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ScheduledWorkRequest>> PostScheduledWorkRequest(string id, ScheduledWorkRequest scheduledWorkRequest)
        {
            //Getting CalendarInfo
            var calInfo= await _context.googleCalendarRecords.Where(x=>x.companyId==id).FirstOrDefaultAsync();
            if (calInfo == null)
            {
                return Unauthorized();
            }

            

                var getswrId = 0;

                ScheduledWorkRequest swr = new ScheduledWorkRequest();
                swr.assetModelId = scheduledWorkRequest.assetModelId;
                swr.assetId = scheduledWorkRequest.assetId;
                swr.headOfProblem = scheduledWorkRequest.headOfProblem;
                swr.frequencyDays = scheduledWorkRequest.frequencyDays;
                swr.initialDateTime = scheduledWorkRequest.initialDateTime;
                swr.description = scheduledWorkRequest.description;
                swr.companyId = scheduledWorkRequest.companyId;


                GoogleCalendar googleCalendar = new GoogleCalendar();

                //DateTime conversion to users timezone
                var startDateTime = swr.initialDateTime;
                var endDateTime = startDateTime.AddDays(swr.frequencyDays);
                //Conversion Ends

                Event newEvent = new Event
                {
                    Summary = "Recurrent Scheduled Work Request",
                    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    Description = swr.description.ToString()+"/n"+swr.assetId
                };


                Console.WriteLine(newEvent);
                var eventId = googleCalendar.InsertRecurringEvent(newEvent, swr.frequencyDays, calInfo.googleCalendarId, calInfo.timeZoneRegional );
                Console.WriteLine(eventId);

                //Db DateTime Work to exact same format
                DateTime inputDateTime = scheduledWorkRequest.initialDateTime;
                string timeZoneId = calInfo.timeZoneWord;
                // Convert parsed datetime to universal datetime
                DateTime universalDateTime = inputDateTime.ToUniversalTime();

                // Convert universal datetime to desired timezone
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                swr.initialDateTime = convertedDateTime;


                swr.eventIdCalendar = eventId;
                _context.scheduledWorkRequests.Add(swr);
                await _context.SaveChangesAsync();
                getswrId = swr.swrAutoId;
                Console.WriteLine(getswrId);
                return Ok();
            


            //_context.scheduledWorkRequests.Add(scheduledWorkRequest);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetScheduledWorkRequest", new { id = scheduledWorkRequest.swrAutoId }, scheduledWorkRequest);
        }

        // DELETE: api/ScheduledWorkRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScheduledWorkRequest(int id, string cId)
        {
                var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == cId).FirstOrDefaultAsync();
                if (calInfo == null)
                {
                    return Unauthorized();
                }
            
                var scheduledWorkRequest = await _context.scheduledWorkRequests.FindAsync(id);
                if (scheduledWorkRequest == null)
                {
                    return NotFound();
                }

                _context.scheduledWorkRequests.Remove(scheduledWorkRequest);
                await _context.SaveChangesAsync();

                GoogleCalendar gc = new GoogleCalendar();
                gc.DeleteEvent(scheduledWorkRequest.eventIdCalendar, calInfo.googleCalendarId);
                return Ok();
               
            
        }

        private bool ScheduledWorkRequestExists(int id)
        {
            return _context.scheduledWorkRequests.Any(e => e.swrAutoId == id);
        }
        private bool GoogleCalendarApiExists(int id)
        {
            return _context.googleCalendarRecords.Any(e => e.googleCalendarAutoId == id);
        }
    }
}
