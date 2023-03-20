using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Cryptography;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FacilitiesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public FacilitiesController(UserDbContext context, ILogger<FacilitiesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Facilities
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Facility>>> Getfacilities()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.facilities.Where(x => x.companyId == claimresponse.companyId && x.status == "Active" ).ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Facilities/5
        [HttpGet("getFacility")]
        [Authorize]
        public async Task<ActionResult<Facility>> GetFacility(string id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    //var facility = await _context.facilities.FindAsync(id);
                    var facility= await _context.facilities.Where(x => x.companyId == claimresponse.companyId).ToListAsync();

                    if (facility == null)
                    {
                        return NotFound();
                    }

                    return Ok(facility);
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Facilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFacility(int id, Facility facility)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    facility.companyId = claimresponse.companyId;
                    _context.Entry(facility).State = EntityState.Modified;

                    
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // POST: api/Facilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Facility>> PostFacility(Facility facility)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var compId = _context.facilities.Where(d => d.companyId == claimresponse.companyId).Select(d => d.facilityId).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Facility f = new Facility();
                        string comId = "B1";
                        f.facilityId = comId;
                        f.facilityName = facility.facilityName;
                        f.companyId = claimresponse.companyId;
                        f.status = facility.status;
                        _context.facilities.Add(f);
                        await _context.SaveChangesAsync();
                        //return Ok(c);
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Facility f = new Facility();
                        string comId = "B" + (int.Parse(autoId) + 1);
                        f.facilityId = comId;
                        f.facilityName = facility.facilityName;
                        f.companyId = claimresponse.companyId;
                        f.status = facility.status;
                        _context.facilities.Add(f);
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
        }

        // DELETE: api/Facilities/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (FacilityExists(id))
                    {
                        var facility = await _context.facilities.FindAsync(id);
                        if (facility == null)
                        {
                            return NotFound();
                        }

                        _context.facilities.Remove(facility);
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

        private bool FacilityExists(int id)
        {
            return _context.facilities.Any(e => e.facilityAutoId == id);
        }
    }
}
