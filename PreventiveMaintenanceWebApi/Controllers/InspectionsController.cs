using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PreventiveMaintenanceWebApi.Models;
using StackExchange.Redis;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionsController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;

        public InspectionsController(PreventiveMaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/Inspections
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inspection>>> Getinspections()
        {
            // Get all available time zones
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();

            // Create a list to store time zone information
            List<string> timeZoneList = new List<string>();

            // Iterate through the time zones and add their IDs and display names to the list
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                string timeZoneEntry = $"{timeZone.Id} - {timeZone.DisplayName}";
                timeZoneList.Add(timeZoneEntry);
            }

            // Display the list of time zones
            foreach (string timeZoneEntry in timeZoneList)
            {
                Console.WriteLine(timeZoneEntry);
            }
            

            return await _context.inspections.ToListAsync();
        }

        // GET: api/Inspections/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inspection>> GetInspection(int id)
        {
            var inspection = await _context.inspections.FindAsync(id);

            if (inspection == null)
            {
                return NotFound();
            }

            return inspection;
        }

        // PUT: api/Inspections/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspection(int id, Inspection inspection)
        {
            var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == inspection.companyId).FirstOrDefaultAsync();
            if (calInfo == null)
            {
                return Unauthorized();
            }

            var existingModel = await _context.inspections.FindAsync(inspection.inspectionAutoId);



            existingModel.initialDate = inspection.initialDate;
            existingModel.question = inspection.question;
            existingModel.options = inspection.options.ToString();
            existingModel.workRequestOfOptions = inspection.workRequestOfOptions.ToString();
            existingModel.frequencyDays = inspection.frequencyDays;

            try
            {
                
                GoogleCalendar googleCalendar = new GoogleCalendar();

                //DateTime conversion to users timezone
                var startDateTime = existingModel.initialDate;
                var endDateTime = startDateTime.AddDays(existingModel.frequencyDays);
                //Conversion Ends

                Event newEvent = new Event
                {
                    Summary = "Recurrent Inspection",
                    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                    Description = existingModel.question.ToString() + "/n" + existingModel.assetId + "/n" + "Updated"
                };

                googleCalendar.UpdateRecurringEvent(calInfo.googleCalendarId, existingModel.eventIdCalendar, newEvent, existingModel.frequencyDays, calInfo.timeZoneRegional);

                //Db DateTime Work to exact same format
                DateTime inputDateTime = inspection.initialDate;
                string timeZoneId = calInfo.timeZoneWord;
                // Convert parsed datetime to universal datetime
                DateTime universalDateTime = inputDateTime.ToUniversalTime();

                // Convert universal datetime to desired timezone
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                existingModel.initialDate = convertedDateTime;
                await _context.SaveChangesAsync();


                return Ok();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Inspections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inspection>> PostInspection(Inspection inspection)
        {
            //Getting CalendarInfo
            var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == inspection.companyId).FirstOrDefaultAsync();
            if (calInfo == null)
            {
                return Unauthorized();
            }



            var getswrId = 0;

            Inspection ins = new Inspection();
            ins.assetModelId = inspection.assetModelId;
            ins.assetId = inspection.assetId;
            ins.question = inspection.question;
            ins.options = inspection.options.ToString();
            ins.workRequestOfOptions = inspection.workRequestOfOptions.ToString();
            ins.frequencyDays = inspection.frequencyDays;
            ins.initialDate = inspection.initialDate;
            ins.companyId = inspection.companyId;


            GoogleCalendar googleCalendar = new GoogleCalendar();

            //DateTime conversion to users timezone
            var startDateTime = ins.initialDate;
            var endDateTime = startDateTime.AddDays(ins.frequencyDays);
            //Conversion Ends

            Event newEvent = new Event
            {
                Summary = "Recurrent Inspection",
                Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                Description = ins.question.ToString() + "/n" + ins.assetId
            };

            
            Console.WriteLine(newEvent);
            var eventId = googleCalendar.InsertRecurringEvent(newEvent, ins.frequencyDays, calInfo.googleCalendarId, calInfo.timeZoneRegional);
            Console.WriteLine(eventId);

            //Db DateTime Work to exact same format
            DateTime inputDateTime = inspection.initialDate;
            string timeZoneId = calInfo.timeZoneWord;
            // Convert parsed datetime to universal datetime
            DateTime universalDateTime = inputDateTime.ToUniversalTime();

            // Convert universal datetime to desired timezone
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
            ins.initialDate=convertedDateTime;
           


            ins.eventIdCalendar = eventId;
            _context.inspections.Add(ins);
            await _context.SaveChangesAsync();
            getswrId = ins.inspectionAutoId;
            Console.WriteLine(getswrId);
            return Ok();

            //_context.inspections.Add(inspection);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetInspection", new { id = inspection.inspectionAutoId }, inspection);
        }

        // DELETE: api/Inspections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id, string cId)
        {
            var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == cId).FirstOrDefaultAsync();
            if (calInfo == null)
            {
                return Unauthorized();
            }

            var ins = await _context.inspections.FindAsync(id);
            if (ins == null)
            {
                return NotFound();
            }

            _context.inspections.Remove(ins);
            await _context.SaveChangesAsync();

            GoogleCalendar gc = new GoogleCalendar();
            gc.DeleteEvent(ins.eventIdCalendar, calInfo.googleCalendarId);
            return Ok();
        }

        private bool InspectionExists(int id)
        {
            return _context.inspections.Any(e => e.inspectionAutoId == id);
        }
    }
}
