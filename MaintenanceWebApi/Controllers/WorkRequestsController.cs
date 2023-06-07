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
    public class WorkRequestsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public WorkRequestsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkRequests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkRequest>>> GetworkRequests(string companyId)
        {
            return await _context.workRequests.Where(x =>x.companyId == companyId).ToListAsync(); ;
        }

        // GET: api/WorkRequests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkRequest>> GetWorkRequest(int id,string companyId)
        {
            var workRequest = await _context.workRequests.Where(x=>x.wrAutoId==id && x.companyId==companyId).ToListAsync();

            if (workRequest == null)
            {
                return NotFound();
            }

            return Ok(workRequest);
        }

        // PUT: api/WorkRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutWorkRequest(int id,string companyId ,WorkRequest workRequest)
        {
            if (workRequest.wrAutoId==id && workRequest.companyId==companyId)
            {
                _context.Entry(workRequest).State = EntityState.Modified;
            }

            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkRequestExists(id))
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

        // POST: api/WorkRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkRequest>> PostWorkRequest(WorkRequest workRequest)
        {
            _context.workRequests.Add(workRequest);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/WorkRequests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkRequest(int id)
        {
            var workRequest = await _context.workRequests.FindAsync(id);
            if (workRequest == null)
            {
                return NotFound();
            }

            _context.workRequests.Remove(workRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkRequestExists(int id)
        {
            return _context.workRequests.Any(e => e.wrAutoId == id);
        }
    }
}
