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
    public class ToolsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ToolsController> _logger;
        public ToolsController(UserDbContext context, ILogger<ToolsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tool>>> Gettools(string cid)
        {
            try
            {
                return await _context.tools.Where(x => x.companyId == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Tools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tool>> GetTool(int id)
        {
            try
            {
                var tool = await _context.tools.FindAsync(id);

                if (tool == null)
                {
                    return NotFound();
                }

                return tool;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Tools/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTool(int id, Tool tool)
        {
            try
            {
                if (id != tool.toolAutoId)
                {
                    return BadRequest();
                }

                _context.Entry(tool).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Tools
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tool>> PostTool(Tool tool)
        {
            try
            {
                var compId = _context.tools.Where(d => d.companyId == tool.companyId).Select(d => d.toolId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Tool t = new Tool();
                    string comId = "T1";
                    t.toolId = comId;
                    t.toolName = tool.toolName;
                    t.companyId = tool.companyId;
                    t.status = tool.status;
                    _context.tools.Add(t);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Tool t = new Tool();
                    string comid = "T" + (int.Parse(autoId) + 1);
                    t.toolId = comid;
                    t.toolName = tool.toolName;
                    t.companyId = tool.companyId;
                    t.status = tool.status;
                    _context.tools.Add(t);
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

            //return CreatedAtAction("GetTool", new { id = tool.toolautoid }, tool);
        }

        // DELETE: api/Tools/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTool(int id)
        {
            try
            {
                var tool = await _context.tools.FindAsync(id);
                if (tool == null)
                {
                    return NotFound();
                }

                _context.tools.Remove(tool);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        private bool ToolExists(int id)
        {
            return _context.tools.Any(e => e.toolAutoId == id);
        }
    }
}
