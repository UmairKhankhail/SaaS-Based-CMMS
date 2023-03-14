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
                return await _context.tools.Where(x => x.companyid == cid).ToListAsync();
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
                if (id != tool.toolautoid)
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
                var compid = _context.tools.Where(d => d.companyid == tool.companyid).Select(d => d.toolid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Tool t = new Tool();
                    string comid = "T1";
                    t.toolid = comid;
                    t.toolname = tool.toolname;
                    t.companyid = tool.companyid;
                    t.status = tool.status;
                    _context.tools.Add(t);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Tool t = new Tool();
                    string comid = "T" + (int.Parse(autoid) + 1);
                    t.toolid = comid;
                    t.toolname = tool.toolname;
                    t.companyid = tool.companyid;
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
            return _context.tools.Any(e => e.toolautoid == id);
        }
    }
}
