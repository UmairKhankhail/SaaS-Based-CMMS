using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using System.ComponentModel.Design;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthAndSafetiesController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<HealthAndSafetiesController> _logger;
        public HealthAndSafetiesController(MaintenanceDbContext context,JwtTokenHandler jwtTokenHandler,ILogger<HealthAndSafetiesController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _logger = logger;
        }

        // GET: api/HealthAndSafeties
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<HealthAndSafety>>> GethealthAndSafeties()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getHandS = await _context.healthAndSafeties.
                        Join(_context.HealthAndSafetyItems, hs => hs.hsAutoId, hsi => hsi.hsAutoId, (hs, hsi) => new { hs, hsi })
                        .Where(x => x.hs.companyId == claimresponse.companyId && x.hsi.companyId == claimresponse.companyId)
                        .Select(result => new
                        {
                            result.hs.hsAutoId,
                            result.hs.woAutoId,
                            result.hs.userName,
                            result.hs.companyId,
                            result.hsi.phsAutoId,

                        }).ToListAsync();

                    return Ok(getHandS);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/HealthAndSafeties/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<HealthAndSafety>> GetHealthAndSafety(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getHandS = await _context.healthAndSafeties.
                   Join(_context.HealthAndSafetyItems, hs => hs.hsAutoId, hsi => hsi.hsAutoId, (hs, hsi) => new { hs, hsi })
                   .Where(x => x.hs.companyId == claimresponse.companyId && x.hsi.companyId == claimresponse.companyId && x.hs.hsAutoId == id)
                   .Select(result => new
                   {
                       result.hs.hsAutoId,
                       result.hs.woAutoId,
                       result.hs.userName,
                       result.hs.companyId,
                       result.hsi.phsAutoId,

                   }).ToListAsync();

                    if (getHandS == null)
                    {
                        return NotFound();
                    }

                    return Ok(getHandS);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/HealthAndSafeties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutHealthAndSafety(int id,HealthAndSafety healthAndSafety)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (healthAndSafety.hsAutoId == id && healthAndSafety.companyId == claimresponse.companyId)
                    {
                        _context.ChangeTracker.Clear();
                        HealthAndSafety hs = new HealthAndSafety();
                        hs.hsAutoId = healthAndSafety.hsAutoId;
                        hs.woAutoId = healthAndSafety.woAutoId;
                        hs.userName = healthAndSafety.userName;
                        hs.companyId = claimresponse.companyId;
                        hs.remarks = healthAndSafety.remarks;


                        var listChecks = healthAndSafety.hsCheckList;
                        var listDbChecks = new List<string>();
                        var getChecks = _context.HealthAndSafetyItems.Where(x => x.hsAutoId == healthAndSafety.hsAutoId && x.companyId == claimresponse.companyId).Select(x => x.hsiAutoId);

                        foreach (var item in getChecks)
                        {
                            listDbChecks.Add(item.ToString());
                            Console.WriteLine("DB Equipments: " + item);
                        }

                        foreach (var equip in listChecks)
                        {
                            Console.WriteLine("New Equipments" + equip);

                        }

                        var resultListLeft = listDbChecks.Except(listChecks).ToList();
                        var resultListRight = listChecks.Except(listDbChecks).ToList();

                        var rll = new List<string>();
                        var rlr = new List<string>();
                        if (resultListLeft != null)
                        {
                            foreach (var x in resultListLeft)
                            {
                                rll.Add(x);
                            }
                        }
                        if (resultListRight != null)
                        {
                            foreach (var x in resultListRight)
                            {
                                rlr.Add(x);
                            }
                        }

                        if (rll != null)
                        {
                            foreach (var item in rll)
                            {
                                var delUser = _context.HealthAndSafetyItems.Where(x => x.companyId == claimresponse.companyId && x.hsAutoId == healthAndSafety.hsAutoId && x.hsiAutoId == int.Parse(item)).FirstOrDefault();
                                if (delUser == null)
                                {
                                    return NotFound();
                                }
                                _context.HealthAndSafetyItems.Remove(delUser);
                            }
                        }

                        if (rlr != null)
                        {
                            foreach (var item in rlr)
                            {
                                HealthAndSafetyItems hsitems = new HealthAndSafetyItems();
                                hsitems.phsAutoId = int.Parse(item.ToString());
                                hsitems.hsAutoId = healthAndSafety.hsAutoId;
                                hsitems.companyId = claimresponse.companyId;
                                hsitems.woAutoId = healthAndSafety.woAutoId;
                                _context.HealthAndSafetyItems.Add(hsitems);
                                await _context.SaveChangesAsync();
                            }
                        }


                        _context.Entry(hs).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                        return Ok();
                    }

                    return NotFound();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/HealthAndSafeties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HealthAndSafety>> PostHealthAndSafety(HealthAndSafety healthAndSafety)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getHealthAndSafetyId = 0;

                    _context.ChangeTracker.Clear();
                    HealthAndSafety hs = new HealthAndSafety();
                    hs.hsAutoId = healthAndSafety.hsAutoId;
                    hs.woAutoId = healthAndSafety.woAutoId;
                    hs.userName = healthAndSafety.userName;
                    hs.companyId = claimresponse.companyId;
                    hs.remarks = healthAndSafety.remarks;

                    _context.healthAndSafeties.Add(hs);
                    await _context.SaveChangesAsync();
                    getHealthAndSafetyId = hs.hsAutoId;

                    var listHSItems = healthAndSafety.hsCheckList;
                    foreach (var item in listHSItems)
                    {
                        HealthAndSafetyItems hsitems = new HealthAndSafetyItems();
                        hsitems.phsAutoId = int.Parse(item.ToString());
                        hsitems.hsAutoId = getHealthAndSafetyId;
                        hsitems.companyId = claimresponse.companyId;
                        hsitems.woAutoId = healthAndSafety.woAutoId;
                        _context.HealthAndSafetyItems.Add(hsitems);
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

            // DELETE: api/HealthAndSafeties/5
       [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteHealthAndSafety(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var healthAndSafety = await _context.healthAndSafeties.FindAsync(id);
                    if (healthAndSafety == null)
                    {
                        return NotFound();
                    }

                    _context.healthAndSafeties.Remove(healthAndSafety);
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

        private bool HealthAndSafetyExists(int id)
        {
            return _context.healthAndSafeties.Any(e => e.hsAutoId == id);
        }
    }
}
