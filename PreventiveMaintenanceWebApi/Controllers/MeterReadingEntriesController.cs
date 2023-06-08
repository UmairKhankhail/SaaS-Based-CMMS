using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly HttpClient _httpClient;
        public MeterReadingEntriesController(PreventiveMaintenanceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // GET: api/MeterReadingEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetmeterReadingEntries(string id)
        {
            //var url = $"http://localhost:5145/api/WorkRequests?companyId={id}";
            //var response = await _httpClient.GetAsync(url);
            //var responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(responseContent);
            //List<string> responseList = new List<string>();
            //var mynewlist = JsonConvert.DeserializeObject<List<string>>(responseContent);
            
            //Console.WriteLine(mynewlist);

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

        [HttpGet("GetMeterReadingEntriesByAssetId")]
        public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetMeterReadingEntriesByAssetId(int assetModel, string assetId, string cId)
        {
            var mre = await _context.meterReadingEntries
                .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == cId)
                .ToListAsync();

            if (mre == null)
            {
                return Unauthorized();
            }

            return mre;
        }

        [HttpGet("GetMeterReadingEntriesByAssetIdForParams")]
        public async Task<ActionResult<IEnumerable<string>>> GetMeterReadingEntriesByAssetIdForParams(int assetModel, string assetId, string cId)
        {
            var mre = await _context.meterReadingEntries
                .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == cId)
                .Select(x => x.paramName)
                .FirstOrDefaultAsync();

            if (mre == null)
            {
                return Unauthorized();
            }

            return Ok(new List<string> { mre });
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
            

            var meterReading = _context.meterReadings.Where(x=>x.paramName==mre.paramName && x.companyId==mre.companyId && x.assetId==mre.assetId).FirstOrDefault();
            if (meterReading == null)
            {
                return NotFound();
            }
            if(mre.value >= meterReading.minValue && mre.value<=meterReading.maxValue){}
            else
            {
                Console.WriteLine("Generate Work Request");
                var url = "http://localhost:5145/api/WorkRequests";
                var parameters = new Dictionary<string, string>
                    {
                        { "topName", mre.paramName + " " + mre.value },
                    { "description", mre.remarks  },
                    { "approve", "Y" },
                    { "companyId", mre.companyId }
                    };


                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine(responseContent);

            }

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
