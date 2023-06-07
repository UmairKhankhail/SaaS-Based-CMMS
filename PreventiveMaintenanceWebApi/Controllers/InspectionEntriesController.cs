using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly HttpClient _httpClient;

        public InspectionEntriesController(PreventiveMaintenanceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
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
        //    var getinsId = 0;

        //    InspectionEntry inse = new InspectionEntry();
        //    inse.assetModelId = inspectionEntry.assetModelId;
        //    inse.assetId = inspectionEntry.assetId;
        //    inse.question = inspectionEntry.question;
        //    inse.selectedOption = inspectionEntry.selectedOption;
        //    inse.companyId = inspectionEntry.companyId;
        //    inse.remarks = inspectionEntry.remarks;
        //    _context.inspectionEntries.Add(inse);
        //    await _context.SaveChangesAsync();
        //    getinsId = inse.inspectionEntryAutoId;
        //    Console.WriteLine(getinsId);
        //    return Ok();

            var getmrId = 0;

            InspectionEntry inse = new InspectionEntry();
            inse.assetModelId = inspectionEntry.assetModelId;
            inse.assetId = inspectionEntry.assetId;
            inse.question = inspectionEntry.question;
            inse.selectedOption = inspectionEntry.selectedOption;
            inse.companyId = inspectionEntry.companyId;
            inse.remarks = inspectionEntry.remarks;


            var inspection = _context.inspections.Where(x => x.question == inse.question && x.companyId == inse.companyId && x.assetId == inse.assetId).FirstOrDefault();
            if (inspection == null)
            {
                return NotFound();
            }

            string listOptions = inspection.options;
            string listWorkRequests = inspection.workRequestOfOptions;

            // Convert the strings to lists
            List<string> options = JsonConvert.DeserializeObject<List<string>>(listOptions);
            List<int> workRequests = JsonConvert.DeserializeObject<List<int>>(listWorkRequests);


            int index = options.FindIndex(item => item == inse.selectedOption);
            if (workRequests[index]==1)
            {
                Console.WriteLine("Generate Work Request");
                var url = "http://localhost:5145/api/WorkRequests";
                var parameters = new Dictionary<string, string>
                    {
                        { "topName", inse.question + " " + inse.selectedOption },
                    { "description", inse.remarks  },
                    { "approve", "Y" },
                    { "companyId", inse.companyId }
                    };


                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);
            }

            _context.inspectionEntries.Add(inse);
            await _context.SaveChangesAsync();
            getmrId = inse.inspectionEntryAutoId;
            Console.WriteLine(getmrId);

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
