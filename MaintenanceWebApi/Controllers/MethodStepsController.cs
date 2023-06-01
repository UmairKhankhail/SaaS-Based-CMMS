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
    public class MethodStepsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public MethodStepsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/MethodSteps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MethodSteps>>> GetmethodSteps(string companyId)
        {
            return await _context.methodSteps.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/MethodSteps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MethodSteps>> GetMethodSteps(int id,string companyId)
        {
            var methodSteps = await _context.methodSteps.Where(x=>x.msAutoId==id && x.companyId==companyId).ToListAsync();

            if (methodSteps == null)
            {
                return NotFound();
            }

            return Ok(methodSteps);
        }

        // PUT: api/MethodSteps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutMethodSteps(int id, string companyId, MethodSteps methodSteps)
        {
            if (methodSteps.msAutoId==id && methodSteps.companyId==companyId)
            {
                _context.Entry(methodSteps).State = EntityState.Modified;
            }

            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodStepsExists(id))
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

        // POST: api/MethodSteps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MethodSteps>> PostMethodSteps(MethodSteps methodSteps)
        {
            _context.methodSteps.Add(methodSteps);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/MethodSteps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodSteps(int id)
        {
            var methodSteps = await _context.methodSteps.FindAsync(id);
            if (methodSteps == null)
            {
                return NotFound();
            }

            _context.methodSteps.Remove(methodSteps);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MethodStepsExists(int id)
        {
            return _context.methodSteps.Any(e => e.msAutoId == id);
        }
    }
}
