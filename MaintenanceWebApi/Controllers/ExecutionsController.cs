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
    public class ExecutionsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public ExecutionsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/Executions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Execution>>> Getexecutions(string companyId)
        {
            return await _context.executions.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/Executions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Execution>> GetExecution(int id,string companyId)
        {
            var execution = await _context.executions.Where(x=>x.executionAutoId==id && x.companyId==companyId).ToListAsync();

            if (execution == null)
            {
                return NotFound();
            }

            return Ok(execution);
        }

        // PUT: api/Executions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExecution(int id,string companyId,Execution execution)
        {
            if (execution.executionAutoId==id && execution.companyId==companyId)
            {

                _context.Entry(execution).State = EntityState.Modified; 
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExecutionExists(id))
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

        // POST: api/Executions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Execution>> PostExecution(Execution execution)
        {
            var compId = _context.executions.Where(exe=> exe.companyId == execution.companyId && exe.woAutoId == execution.woAutoId).Select(d => d.executionId).ToList();
            var autoId = "";
            if (compId.Count > 0)
            {

                autoId = compId.Max(x => int.Parse(x.Substring(3))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                Execution exe = new Execution();
                string comId = "EXE1";
                exe.executionAutoId = execution.executionAutoId;
                exe.executionId = comId;
                exe.woAutoId = execution.woAutoId;
                exe.userName = execution.userName;
                exe.topName = execution.topName;
                exe.startTime = execution.startTime;
                exe.endTime = execution.endTime;
                exe.remarks = execution.remarks;
                exe.companyId = execution.companyId;
                _context.executions.Add(exe);
                await _context.SaveChangesAsync();
            }
            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                Execution exe = new Execution();
                string comId = "EXE" + (int.Parse(autoId) + 1);
                exe.executionAutoId = execution.executionAutoId;
                exe.executionId = comId;
                exe.woAutoId = execution.woAutoId;
                exe.userName = execution.userName;
                exe.topName = execution.topName;
                exe.startTime = execution.startTime;
                exe.endTime = execution.endTime;
                exe.remarks = execution.remarks;
                exe.companyId = execution.companyId;
                _context.executions.Add(exe);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // DELETE: api/Executions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExecution(int id)
        {
            var execution = await _context.executions.FindAsync(id);
            if (execution == null)
            {
                return NotFound();
            }

            _context.executions.Remove(execution);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExecutionExists(int id)
        {
            return _context.executions.Any(e => e.executionAutoId == id);
        }
    }
}
