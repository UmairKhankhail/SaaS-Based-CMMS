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
    public class InstructionsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public InstructionsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/Instructions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Instruction>>> Getinstructions(string companyId)
        {
            return await _context.instructions.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/Instructions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Instruction>> GetInstruction(int id,string companyId)
        {
            var instruction = await _context.instructions.Where(x=>x.insAutoId==id && x.companyId==companyId).ToListAsync();

            if (instruction == null)
            {
                return NotFound();
            }

            return Ok(instruction);
        }

        // PUT: api/Instructions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutInstruction(int id, string companyId,Instruction instruction)
        {
            if (instruction.insAutoId==id && instruction.companyId==companyId)
            { 
                _context.Entry(instruction).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstructionExists(id))
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

        // POST: api/Instructions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Instruction>> PostInstruction(Instruction instruction)
        {
            _context.instructions.Add(instruction);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Instructions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstruction(int id)
        {
            var instruction = await _context.instructions.FindAsync(id);
            if (instruction == null)
            {
                return NotFound();
            }

            _context.instructions.Remove(instruction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InstructionExists(int id)
        {
            return _context.instructions.Any(e => e.insAutoId == id);
        }
    }
}
