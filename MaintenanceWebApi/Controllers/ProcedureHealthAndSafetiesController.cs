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
    public class ProcedureHealthAndSafetiesController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public ProcedureHealthAndSafetiesController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/ProcedureHealthAndSafeties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcedureHealthAndSafety>>> GetprocedureHealthAndSafeties(string companyId)
        {
            return await _context.procedureHealthAndSafeties.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/ProcedureHealthAndSafeties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcedureHealthAndSafety>> GetProcedureHealthAndSafety(int id,string companyId)
        {
            var procedureHealthAndSafety = await _context.procedureHealthAndSafeties.Where(x=>x.hsAutoId==id && x.companyId==companyId).ToListAsync();

            if (procedureHealthAndSafety == null)
            {
                return NotFound();
            }

            return Ok(procedureHealthAndSafety);
        }

        // PUT: api/ProcedureHealthAndSafeties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProcedureHealthAndSafety(int id,string companyId, ProcedureHealthAndSafety procedureHealthAndSafety)
        {
            if (procedureHealthAndSafety.hsAutoId==id && procedureHealthAndSafety.companyId==companyId)
            {
                _context.Entry(procedureHealthAndSafety).State = EntityState.Modified;

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureHealthAndSafetyExists(id))
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

        // POST: api/ProcedureHealthAndSafeties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProcedureHealthAndSafety>> PostProcedureHealthAndSafety(ProcedureHealthAndSafety procedureHealthAndSafety)
        {
            _context.procedureHealthAndSafeties.Add(procedureHealthAndSafety);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/ProcedureHealthAndSafeties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedureHealthAndSafety(int id)
        {
            var procedureHealthAndSafety = await _context.procedureHealthAndSafeties.FindAsync(id);
            if (procedureHealthAndSafety == null)
            {
                return NotFound();
            }

            _context.procedureHealthAndSafeties.Remove(procedureHealthAndSafety);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcedureHealthAndSafetyExists(int id)
        {
            return _context.procedureHealthAndSafeties.Any(e => e.hsAutoId == id);
        }
    }
}
