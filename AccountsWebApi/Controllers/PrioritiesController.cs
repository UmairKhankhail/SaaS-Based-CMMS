using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.Security.Cryptography;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritiesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<PrioritiesController> _logger;
        public PrioritiesController(UserDbContext context, ILogger<PrioritiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Priorities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Priority>>> Getpriorities(string cid)
        {
            try
            {
                return await _context.priorities.Where(x => x.companyId == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Priorities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Priority>> GetPriority(int id)
        {
            try
            {
                var priority = await _context.priorities.FindAsync(id);

                if (priority == null)
                {
                    return NotFound();
                }

                return priority;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Priorities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPriority(int id, Priority priority)
        {
            try
            {
                if (id != priority.priorityAutoId)
                {
                    return BadRequest();
                }

                _context.Entry(priority).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Priorities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Priority>> PostPriority(Priority priority)
        {
            try
            {
                var compId = _context.priorities.Where(d => d.companyId == priority.companyId).Select(d => d.priorityId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Priority p = new Priority();
                    string comid = "PR1";
                    p.priorityId = comid;
                    p.priorityName = priority.priorityName;
                    p.companyId = priority.companyId;
                    p.status = priority.status;
                    _context.priorities.Add(p);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Priority p = new Priority();
                    string comId = "PR" + (int.Parse(autoId) + 1);
                    p.priorityId = comId;
                    p.priorityName = priority.priorityName;
                    p.companyId = priority.companyId;
                    p.status = priority.status;
                    _context.priorities.Add(p);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Priorities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority(int id)
        {
            try
            {
                var priority = await _context.priorities.FindAsync(id);
                if (priority == null)
                {
                    return NotFound();
                }

                _context.priorities.Remove(priority);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
                
        }

        private bool PriorityExists(int id)
        {
            return _context.priorities.Any(e => e.priorityAutoId == id);
        }
    }
}
