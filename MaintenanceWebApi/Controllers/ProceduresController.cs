using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using System.Drawing.Imaging;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public ProceduresController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/Procedures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procedure>>> Getprocedures(string companyId)
        {
            return await _context.procedures.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/Procedures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Procedure>> GetProcedure(int id,string companyId)
        {
            var procedure = await _context.procedures.Where(x=>x.pAutoId==id && x.companyId==companyId).ToListAsync();

            if (procedure == null)
            {
                return NotFound();
            }

            return Ok(procedure);
        }

        // PUT: api/Procedures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutProcedure(int id, string companyId, Procedure procedure)
        {
            if (procedure.pAutoId == id && procedure.companyId==companyId)
            {
               _context.Entry(procedure).State = EntityState.Modified; ;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureExists(id))
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

        // POST: api/Procedures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Procedure>> PostProcedure(Procedure procedure)
        {
            _context.procedures.Add(procedure);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Procedures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            var procedure = await _context.procedures.FindAsync(id);

            if (procedure == null)
            {
                return NotFound();
            }

            _context.procedures.Remove(procedure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcedureExists(int id)
        {
            return _context.procedures.Any(e => e.pAutoId == id);
        }
    }
}
