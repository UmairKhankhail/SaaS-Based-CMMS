using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusAndRepairsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<StatusAndRepairsController> _logger;


        public StatusAndRepairsController(MaintenanceDbContext context, JwtTokenHandler jwtTokenHandler, ILogger<StatusAndRepairsController> logger)
        {
            _context = context;
            _JwtTokenHandler= jwtTokenHandler;
            _logger= logger;
        }

        // GET: api/StatusAndRepairs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StatusAndRepair>>> GetstatusAndRepairs()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.statusAndRepairs.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/StatusAndRepairs/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<StatusAndRepair>> GetStatusAndRepair(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var statusAndRepair = await _context.statusAndRepairs.Where(x => x.srAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (statusAndRepair == null)
                    {
                        return NotFound();
                    }

                    return Ok(statusAndRepair);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/StatusAndRepairs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutStatusAndRepair(int id, string companyId,StatusAndRepair statusAndRepair)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (statusAndRepair.srAutoId == id && statusAndRepair.companyId == claimresponse.companyId)
                    {

                        _context.Entry(statusAndRepair).State = EntityState.Modified;
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StatusAndRepairExists(id))
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

        // POST: api/StatusAndRepairs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<StatusAndRepair>> PostStatusAndRepair(StatusAndRepair statusAndRepair)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var compId = _context.statusAndRepairs.Where(i => i.companyId == claimresponse.companyId).Select(d => d.srId).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {

                        autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {

                        _context.ChangeTracker.Clear();
                        StatusAndRepair sr = new StatusAndRepair();
                        string comId = "SR1";
                        sr.srAutoId = statusAndRepair.srAutoId;
                        sr.srId = comId;
                        sr.username = statusAndRepair.username;
                        sr.itemName = statusAndRepair.itemName;
                        sr.faultyNotFaulty = statusAndRepair.faultyNotFaulty;
                        sr.inhouseOrOutsource = statusAndRepair.inhouseOrOutsource;
                        sr.worker = statusAndRepair.worker;
                        sr.woAutoId = statusAndRepair.woAutoId;
                        sr.companyId = claimresponse.companyId;
                        _context.statusAndRepairs.Add(sr);
                        await _context.SaveChangesAsync();
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        StatusAndRepair sr = new StatusAndRepair();
                        string comId = "SR" + (int.Parse(autoId) + 1);
                        sr.srAutoId = statusAndRepair.srAutoId;
                        sr.srId = comId;
                        sr.username = statusAndRepair.username;
                        sr.itemName = statusAndRepair.itemName;
                        sr.faultyNotFaulty = statusAndRepair.faultyNotFaulty;
                        sr.inhouseOrOutsource = statusAndRepair.inhouseOrOutsource;
                        sr.worker = statusAndRepair.worker;
                        sr.woAutoId = statusAndRepair.woAutoId;
                        sr.companyId = claimresponse.companyId;
                        _context.statusAndRepairs.Add(sr);
                        await _context.SaveChangesAsync();

                    }

                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // DELETE: api/StatusAndRepairs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStatusAndRepair(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var statusAndRepair = await _context.statusAndRepairs.FindAsync(id);
                    if (statusAndRepair == null)
                    {
                        return NotFound();
                    }

                    _context.statusAndRepairs.Remove(statusAndRepair);
                    await _context.SaveChangesAsync();

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

        private bool StatusAndRepairExists(int id)
        {
            return _context.statusAndRepairs.Any(e => e.srAutoId == id);
        }
    }
}
