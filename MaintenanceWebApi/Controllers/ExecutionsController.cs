using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using System.Net.Http;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutionsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExecutionsController> _logger;

        public ExecutionsController(MaintenanceDbContext context, HttpClient httpClient, ILogger<ExecutionsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
            _httpClient = httpClient;

        }

        // GET: api/Executions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Execution>>> Getexecutions()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.executions.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Executions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Execution>> GetExecution(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var execution = await _context.executions.Where(x => x.executionAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (execution == null)
                    {
                        return NotFound();
                    }

                    return Ok(execution);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Executions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutExecution(int id,Execution execution)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (execution.executionAutoId == id && execution.companyId == claimresponse.companyId)
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
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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
        public async Task<IActionResult> DeleteExecution(int id, string cId)
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
