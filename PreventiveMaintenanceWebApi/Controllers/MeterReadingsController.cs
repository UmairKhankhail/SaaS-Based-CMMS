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
    public class MeterReadingsController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;

        public MeterReadingsController(PreventiveMaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/MeterReadings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReading>>> GetmeterReadings()
        {
            return await _context.meterReadings.ToListAsync();
        }

        // GET: api/MeterReadings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterReading>> GetMeterReading(int id)
        {
            var meterReading = await _context.meterReadings.FindAsync(id);

            if (meterReading == null)
            {
                return NotFound();
            }

            return meterReading;
        }

        // PUT: api/MeterReadings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeterReading(int id, MeterReading meterReading)
        {
            if (id != meterReading.mrAutoId)
            {
                return BadRequest();
            }

            _context.Entry(meterReading).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterReadingExists(id))
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

        // POST: api/MeterReadings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MeterReading>> PostMeterReading(MeterReading meterReading)
        {
            //var getmrId = 0;

            //MeterReading mr = new MeterReading();
            //mr.assetModelId = meterReading.assetModelId;
            //mr.assetId = meterReading.assetId;
            //mr.minValue = meterReading.minValue;
            //mr.maxValue = meterReading.maxValue;
            //mr.paramType = meterReading.paramType;
            //mr.paramName = meterReading.paramName;
            //mr.companyId = meterReading.companyId;
            //mr.initialDate = meterReading.initialDate;
            //mr.frequencyDays = meterReading.frequencyDays;


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

            //mr.eventIdCalendar = eventId;
            //_context.meterReadings.Add(mr);
            //await _context.SaveChangesAsync();
            //getmrId = mr.mrAutoId;
            //Console.WriteLine(getmrId);
            return Ok();

            //_context.meterReadings.Add(meterReading);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMeterReading", new { id = meterReading.mrAutoId }, meterReading);
        }

        // DELETE: api/MeterReadings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReading(int id)
        {
            var meterReading = await _context.meterReadings.FindAsync(id);
            if (meterReading == null)
            {
                return NotFound();
            }

            _context.meterReadings.Remove(meterReading);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeterReadingExists(int id)
        {
            return _context.meterReadings.Any(e => e.mrAutoId == id);
        }
    }
}
