using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Drawing;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionallocationsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FunctionallocationsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;

        public FunctionallocationsController(UserDbContext context, ILogger<FunctionallocationsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Functionallocations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Functionallocation>>> Getfunctionallocations()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.functionalLocations.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Functionallocations/5
        [HttpGet("getFunctionalLocation")]
        [Authorize]
        public async Task<ActionResult<Functionallocation>> GetFunctionallocation(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var functionalLocation = await _context.functionalLocations.FindAsync(id);

                    if (functionalLocation == null)
                    {
                        return NotFound();
                    }

                    return functionalLocation;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Functionallocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFunctionallocation(Functionallocation functionalLocation)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (FunctionallocationExists(functionalLocation.facilityAutoId))
                    {
                        functionalLocation.companyId = claimresponse.companyId;
                        _context.Entry(functionalLocation).State = EntityState.Modified;
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

        // POST: api/Functionallocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Functionallocation>> PostFunctionallocation(Functionallocation functionallocation)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var faFlFunctionalLoc = _context.functionalLocations.Where(d => d.companyId == claimresponse.companyId).Select(f => f.flId).ToList();
                    List<string> flList = new List<string>();
                    List<int> flNoList = new List<int>();
                    foreach (var z in faFlFunctionalLoc)
                    {
                        if (z.Contains(functionallocation.floorSingleId + "L"))
                        {
                            flList.Add(z);
                        }
                    }
                    if (flList.Count > 0)
                    {
                        foreach (var x in flList)
                        {
                            int startIndex = x.IndexOf('L') + 1;
                            int endIndex = x.IndexOf('D');
                            string result = x.Substring(startIndex, endIndex - startIndex);
                            flNoList.Add(int.Parse(result));
                        }
                    }

                    if (flNoList.Count == 0)
                    {
                        _context.ChangeTracker.Clear();
                        Functionallocation f = new Functionallocation();
                        f.flId = functionallocation.floorSingleId + "L1" + functionallocation.subDeptSingleId;
                        f.flName = functionallocation.flName;
                        f.companyId = claimresponse.companyId;
                        f.status = "Active";
                        f.facilityAutoId = functionallocation.facilityAutoId;
                        f.floorAutoId = functionallocation.floorAutoId;
                        f.subDeptAutoId = functionallocation.subDeptAutoId;
                        _context.functionalLocations.Add(f);
                        await _context.SaveChangesAsync();
                    }
                    if (flNoList.Count > 0)
                    {
                        _context.ChangeTracker.Clear();
                        Functionallocation f = new Functionallocation();
                        string comId = functionallocation.floorSingleId + "L" + (flNoList.Max() + 1) + functionallocation.subDeptSingleId;
                        f.flId = comId;
                        f.flName = functionallocation.flName;
                        f.companyId = claimresponse.companyId;
                        f.status = "Active";
                        f.facilityAutoId = functionallocation.facilityAutoId;
                        f.floorAutoId = functionallocation.floorAutoId;
                        f.subDeptAutoId = functionallocation.subDeptAutoId;
                        _context.functionalLocations.Add(f);
                        await _context.SaveChangesAsync();
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

        // DELETE: api/Functionallocations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFunctionallocation(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (FunctionallocationExists(id))
                    {
                        var functionalLocation = await _context.functionalLocations.FindAsync(id);
                        if (functionalLocation == null)
                        {
                            return NotFound();
                        }

                        _context.functionalLocations.Remove(functionalLocation);
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

        private bool FunctionallocationExists(int id)
        {
            return _context.functionalLocations.Any(e => e.flAutoId == id);
        }
    }
}
