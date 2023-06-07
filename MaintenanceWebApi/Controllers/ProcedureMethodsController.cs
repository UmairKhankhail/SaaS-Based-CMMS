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
    public class ProcedureMethodsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public ProcedureMethodsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/ProcedureMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProcedureMethod>>> GetprocedureMethods(string companyId)
        {
            return await _context.procedureMethods.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/ProcedureMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProcedureMethod>> GetProcedureMethod(int id,string companyId)
        {
            var procedureMethod = await _context.procedureMethods.Where(x=>x.pmAutoId==id && x.companyId==companyId).ToListAsync();

            if (procedureMethod == null)
            {
                return NotFound();
            }

            return Ok(procedureMethod);
        }

        // PUT: api/ProcedureMethods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProcedureMethod(int id, string companyId, ProcedureMethod procedureMethod)
        {
            if (procedureMethod.pmAutoId==id && procedureMethod.companyId==companyId)
            {
                _context.Entry(procedureMethod).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureMethodExists(id))
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

        // POST: api/ProcedureMethods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProcedureMethod>> PostProcedureMethod(ProcedureMethod procedureMethod)
        {
            _context.procedureMethods.Add(procedureMethod);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/ProcedureMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedureMethod(int id)
        {
            var procedureMethod = await _context.procedureMethods.FindAsync(id);
            if (procedureMethod == null)
            {
                return NotFound();
            }

            _context.procedureMethods.Remove(procedureMethod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcedureMethodExists(int id)
        {
            return _context.procedureMethods.Any(e => e.pmAutoId == id);
        }
    }
}
