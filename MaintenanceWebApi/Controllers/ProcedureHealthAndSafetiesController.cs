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
    public class ProcedureHealthAndSafetiesController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<ProcedureHealthAndSafetiesController> _logger;

        public ProcedureHealthAndSafetiesController(MaintenanceDbContext context, JwtTokenHandler jwtTokenHandler, ILogger<ProcedureHealthAndSafetiesController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _logger = logger;
        }

        // GET: api/ProcedureHealthAndSafeties
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ProcedureHealthAndSafety>>> GetprocedureHealthAndSafeties()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.procedureHealthAndSafeties.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/ProcedureHealthAndSafeties/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ProcedureHealthAndSafety>> GetProcedureHealthAndSafety(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var procedureHealthAndSafety = await _context.procedureHealthAndSafeties.Where(x => x.phsAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (procedureHealthAndSafety == null)
                    {
                        return NotFound();
                    }

                    return Ok(procedureHealthAndSafety);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/ProcedureHealthAndSafeties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutProcedureHealthAndSafety(int id,ProcedureHealthAndSafety procedureHealthAndSafety)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (procedureHealthAndSafety.phsAutoId == id && procedureHealthAndSafety.companyId == claimresponse.companyId)
                    {
                        _context.Entry(procedureHealthAndSafety).State = EntityState.Modified;

                    }


                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProcedureHealthAndSafetyExists(id))
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

        // POST: api/ProcedureHealthAndSafeties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProcedureHealthAndSafety>> PostProcedureHealthAndSafety(ProcedureHealthAndSafety procedureHealthAndSafety)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    procedureHealthAndSafety.companyId=claimresponse.companyId;
                    _context.procedureHealthAndSafeties.Add(procedureHealthAndSafety);
                    await _context.SaveChangesAsync();

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

        // DELETE: api/ProcedureHealthAndSafeties/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProcedureHealthAndSafety(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var procedureHealthAndSafety = await _context.procedureHealthAndSafeties.FindAsync(id);
                    if (procedureHealthAndSafety == null)
                    {
                        return NotFound();
                    }

                    _context.procedureHealthAndSafeties.Remove(procedureHealthAndSafety);
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

        private bool ProcedureHealthAndSafetyExists(int id)
        {
            return _context.procedureHealthAndSafeties.Any(e => e.phsAutoId == id);
        }
    }
}
