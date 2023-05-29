using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.Security.Cryptography;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ToolsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public ToolsController(UserDbContext context, ILogger<ToolsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Tools
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Tool>>> Gettools()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.tools.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Tools/5
        [HttpGet("getTool")]
        [Authorize]
        public async Task<ActionResult<Tool>> GetTool(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var tool = await _context.tools.FindAsync(id);

                    if (tool == null)
                    {
                        return NotFound();
                    }

                    return tool;
                }
                return Unauthorized();
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
        [Authorize]
        public async Task<IActionResult> PutTool(Tool tool)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (ToolExists(tool.toolAutoId))
                    {
                        tool.companyId = claimresponse.companyId;
                        _context.Entry(tool).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    return NotFound();
                }
                return Unauthorized();
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
        [Authorize]
        public async Task<ActionResult<Tool>> PostTool(Tool tool)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var compId = _context.tools.Where(d => d.companyId == claimresponse.companyId).Select(d => d.toolId).ToList();
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
                        t.companyId = claimresponse.companyId;
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
                        t.companyId = claimresponse.companyId;
                        t.status = tool.status;
                        _context.tools.Add(t);
                        await _context.SaveChangesAsync();
                        //return Ok(c);
                    }

                    return Ok();
                }
                return Unauthorized();
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
        [Authorize]
        public async Task<IActionResult> DeleteTool(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (ToolExists(id))
                    {
                        var tool = await _context.tools.FindAsync(id);
                        if (tool == null)
                        {
                            return NotFound();
                        }

                        _context.tools.Remove(tool);
                        await _context.SaveChangesAsync();

                        return Ok();
                    }
                    return NoContent();
                }
                return Unauthorized();
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
