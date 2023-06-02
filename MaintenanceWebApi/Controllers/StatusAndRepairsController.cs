using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusAndRepairsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public StatusAndRepairsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/StatusAndRepairs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusAndRepair>>> GetstatusAndRepairs(string companyId)
        {
            return await _context.statusAndRepairs.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/StatusAndRepairs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusAndRepair>> GetStatusAndRepair(int id,string companyId)
        {
            var statusAndRepair = await _context.statusAndRepairs.Where(x=>x.srAutoId==id && x.companyId==companyId).ToListAsync();

            if (statusAndRepair == null)
            {
                return NotFound();
            }

            return Ok(statusAndRepair);
        }

        // PUT: api/StatusAndRepairs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStatusAndRepair(int id, string companyId,StatusAndRepair statusAndRepair)
        {
            if (statusAndRepair.srAutoId==id && statusAndRepair.companyId==companyId)
            {

                _context.Entry(statusAndRepair).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StatusAndRepairExists(id))
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

        // POST: api/StatusAndRepairs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StatusAndRepair>> PostStatusAndRepair(StatusAndRepair statusAndRepair)
        {
            var compId = _context.statusAndRepairs.Where(i => i.companyId == statusAndRepair.companyId).Select(d => d.srId).ToList();
            var autoId = "";
            if (compId.Count > 0)
            {

                autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
            }

            if (autoId == "")
            {

                _context.ChangeTracker.Clear();
                StatusAndRepair sr = new StatusAndRepair();
                string comId = "SR1";
                sr.srAutoId=statusAndRepair.srAutoId;
                sr.srId = comId;
                sr.username=statusAndRepair.username;
                sr.itemName=statusAndRepair.itemName;
                sr.faultyNotFaulty = statusAndRepair.faultyNotFaulty;
                sr.worker = statusAndRepair.worker;
                sr.woAutoId = statusAndRepair.woAutoId;
                sr.companyId = statusAndRepair.companyId;
                _context.statusAndRepairs.Add(sr);
                await _context.SaveChangesAsync();
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                StatusAndRepair sr = new StatusAndRepair();
                string comId = "SR" + (int.Parse(autoId) + 1);
                sr.srAutoId = statusAndRepair.srAutoId;
                sr.srId = comId;
                sr.username = statusAndRepair.username;
                sr.itemName = statusAndRepair.itemName;
                sr.faultyNotFaulty = statusAndRepair.faultyNotFaulty;
                sr.worker = statusAndRepair.worker;
                sr.woAutoId = statusAndRepair.woAutoId;
                sr.companyId = statusAndRepair.companyId;
                _context.statusAndRepairs.Add(sr);
                await _context.SaveChangesAsync();

            }

            return Ok();

        }

        // DELETE: api/StatusAndRepairs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatusAndRepair(int id)
        {
            var statusAndRepair = await _context.statusAndRepairs.FindAsync(id);
            if (statusAndRepair == null)
            {
                return NotFound();
            }

            _context.statusAndRepairs.Remove(statusAndRepair);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StatusAndRepairExists(int id)
        {
            return _context.statusAndRepairs.Any(e => e.srAutoId == id);
        }
    }
}
