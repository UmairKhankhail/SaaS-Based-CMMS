using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreventiveMaintenanceWebApi.Models;

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
            if (id != inspection.inspectionAutoId)
            {
                return BadRequest();
            }

            _context.Entry(inspection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Inspections
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inspection>> PostInspection(Inspection inspection)
        {
            //var getinsId = 0;

            //Inspection ins = new Inspection();
            //ins.assetModelId = inspection.assetModelId;
            //ins.assetId = inspection.assetId;
            //ins.question = inspection.question;
            //ins.options = inspection.options;
            //ins.workRequestOfOptions = inspection.workRequestOfOptions;
            //ins.companyId = inspection.companyId;
            //ins.initialDate = inspection.initialDate;
            //ins.frequencyDays = inspection.frequencyDays;


            //GoogleCalendar googleCalendar = new GoogleCalendar();

            //var startDateTime = new DateTime(2023, 6, 2, 9, 0, 0);  // Specify the start date and time in "Asia/Karachi" time zone
            //var endDateTime = new DateTime(2023, 6, 2, 10, 0, 0);   // Specify the end date and time in "Asia/Karachi" time zone

            //Event newEvent = new Event
            //{
            //    Summary = "Meter Reading Request",
            //    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = "Asia/Karachi" },
            //    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = "Asia/Karachi" },
            //    Description = "Event description"
            //};


            //Console.WriteLine(newEvent);
            //var eventId = googleCalendar.InsertRecurringEvent(newEvent,10);
            //Console.WriteLine(eventId);

            //ins.eventIdCalendar = eventId;
            //_context.inspections.Add(ins);
            //await _context.SaveChangesAsync();
            //getinsId = ins.inspectionAutoId;
            //Console.WriteLine(getinsId);
            return Ok();
            //_context.inspections.Add(inspection);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetInspection", new { id = inspection.inspectionAutoId }, inspection);
        }

        // DELETE: api/Inspections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspection(int id)
        {
            var inspection = await _context.inspections.FindAsync(id);
            if (inspection == null)
            {
                return NotFound();
            }

            _context.inspections.Remove(inspection);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InspectionExists(int id)
        {
            return _context.inspections.Any(e => e.inspectionAutoId == id);
        }
    }
}
