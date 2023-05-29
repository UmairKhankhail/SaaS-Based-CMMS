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
    public class TypeofmaintenancesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<TypeofmaintenancesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public TypeofmaintenancesController(UserDbContext context, ILogger<TypeofmaintenancesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Typeofmaintenances
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Typeofmaintenance>>> Gettypeofmaintenances()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.typeOfMaintenances.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Typeofmaintenances/5
        [HttpGet("getTypeOfMaintenance")]
        [Authorize]
        public async Task<ActionResult<Typeofmaintenance>> GetTypeofmaintenance(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var typeOfMaintenance = await _context.typeOfMaintenances.FindAsync(id);

                    if (typeOfMaintenance == null)
                    {
                        return NotFound();
                    }

                    return typeOfMaintenance;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Typeofmaintenances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTypeofmaintenance(Typeofmaintenance typeOfMaintenance)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (TypeofmaintenanceExists(typeOfMaintenance.tomAutoId))
                    {
                        typeOfMaintenance.companyId = claimresponse.companyId;
                        _context.Entry(typeOfMaintenance).State = EntityState.Modified;
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

        // POST: api/Typeofmaintenances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Typeofmaintenance>> PostTypeofmaintenance(Typeofmaintenance typeOfMaintenance)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var compId = _context.typeOfMaintenances.Where(d => d.companyId == claimresponse.companyId).Select(d => d.tomId).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Typeofmaintenance m = new Typeofmaintenance();
                        string comId = "TM1";
                        m.tomId = comId;
                        m.tomName = typeOfMaintenance.tomName;
                        m.companyId = claimresponse.companyId;
                        m.status = typeOfMaintenance.status;
                        _context.typeOfMaintenances.Add(m);
                        await _context.SaveChangesAsync();
                        //return Ok(c);
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Typeofmaintenance m = new Typeofmaintenance();
                        string comId = "TM" + (int.Parse(autoId) + 1);
                        m.tomId = comId;
                        m.tomName = typeOfMaintenance.tomName;
                        m.companyId = claimresponse.companyId;
                        m.status = typeOfMaintenance.status;
                        _context.typeOfMaintenances.Add(m);
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

        // DELETE: api/Typeofmaintenances/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTypeofmaintenance(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (TypeofmaintenanceExists(id))
                    {
                        var typeOfMaintenance = await _context.typeOfMaintenances.FindAsync(id);
                        if (typeOfMaintenance == null)
                        {
                            return NotFound();
                        }

                        _context.typeOfMaintenances.Remove(typeOfMaintenance);
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

        private bool TypeofmaintenanceExists(int id)
        {
            return _context.typeOfMaintenances.Any(e => e.tomAutoId == id);
        }
    }
}
