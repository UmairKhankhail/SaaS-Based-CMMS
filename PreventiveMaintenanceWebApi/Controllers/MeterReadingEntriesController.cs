using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;

        public MeterReadingEntriesController(PreventiveMaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/MeterReadingEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetmeterReadingEntries()
        {
            return await _context.meterReadingEntries.ToListAsync();
        }

        // GET: api/MeterReadingEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MeterReadingEntry>> GetMeterReadingEntry(int id)
        {
            var meterReadingEntry = await _context.meterReadingEntries.FindAsync(id);

            if (meterReadingEntry == null)
            {
                return NotFound();
            }

            return meterReadingEntry;
        }

        // PUT: api/MeterReadingEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMeterReadingEntry(int id, MeterReadingEntry meterReadingEntry)
        {
            if (id != meterReadingEntry.mreAutoId)
            {
                return BadRequest();
            }

            _context.Entry(meterReadingEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeterReadingEntryExists(id))
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

        // POST: api/MeterReadingEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MeterReadingEntry>> PostMeterReadingEntry(MeterReadingEntry meterReadingEntry)
        {
            var getmrId = 0;

            MeterReadingEntry mre = new MeterReadingEntry();
            mre.assetModelId = meterReadingEntry.assetModelId;
            mre.assetId = meterReadingEntry.assetId;
            mre.paramName = meterReadingEntry.paramName;
            mre.value = meterReadingEntry.value;
            mre.companyId = meterReadingEntry.companyId;
            mre.remarks = meterReadingEntry.remarks;
            _context.meterReadingEntries.Add(mre);
            await _context.SaveChangesAsync();
            getmrId = mre.mreAutoId;
            Console.WriteLine(getmrId);
            return Ok();
            //_context.meterReadingEntries.Add(meterReadingEntry);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMeterReadingEntry", new { id = meterReadingEntry.mreAutoId }, meterReadingEntry);
        }

        // DELETE: api/MeterReadingEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMeterReadingEntry(int id)
        {
            var meterReadingEntry = await _context.meterReadingEntries.FindAsync(id);
            if (meterReadingEntry == null)
            {
                return NotFound();
            }

            _context.meterReadingEntries.Remove(meterReadingEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MeterReadingEntryExists(int id)
        {
            return _context.meterReadingEntries.Any(e => e.mreAutoId == id);
        }
    }
}
