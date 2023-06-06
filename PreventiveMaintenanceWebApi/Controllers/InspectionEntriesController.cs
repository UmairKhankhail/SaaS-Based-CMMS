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
    public class InspectionEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;

        public InspectionEntriesController(PreventiveMaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/InspectionEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionEntry>>> GetinspectionEntries()
        {
            return await _context.inspectionEntries.ToListAsync();
        }

        // GET: api/InspectionEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionEntry>> GetInspectionEntry(int id)
        {
            var inspectionEntry = await _context.inspectionEntries.FindAsync(id);

            if (inspectionEntry == null)
            {
                return NotFound();
            }

            return inspectionEntry;
        }

        // PUT: api/InspectionEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspectionEntry(int id, InspectionEntry inspectionEntry)
        {
            if (id != inspectionEntry.inspectionEntryAutoId)
            {
                return BadRequest();
            }

            _context.Entry(inspectionEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionEntryExists(id))
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

        // POST: api/InspectionEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InspectionEntry>> PostInspectionEntry(InspectionEntry inspectionEntry)
        {
            var getinsId = 0;

            InspectionEntry inse = new InspectionEntry();
            inse.assetModelId = inspectionEntry.assetModelId;
            inse.assetId = inspectionEntry.assetId;
            inse.question = inspectionEntry.question;
            inse.selectedOption = inspectionEntry.selectedOption;
            inse.companyId = inspectionEntry.companyId;
            inse.remarks = inspectionEntry.remarks;
            _context.inspectionEntries.Add(inse);
            await _context.SaveChangesAsync();
            getinsId = inse.inspectionEntryAutoId;
            Console.WriteLine(getinsId);
            return Ok();
            //_context.inspectionEntries.Add(inspectionEntry);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetInspectionEntry", new { id = inspectionEntry.inspectionEntryAutoId }, inspectionEntry);
        }

        // DELETE: api/InspectionEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionEntry(int id)
        {
            var inspectionEntry = await _context.inspectionEntries.FindAsync(id);
            if (inspectionEntry == null)
            {
                return NotFound();
            }

            _context.inspectionEntries.Remove(inspectionEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InspectionEntryExists(int id)
        {
            return _context.inspectionEntries.Any(e => e.inspectionEntryAutoId == id);
        }
    }
}
