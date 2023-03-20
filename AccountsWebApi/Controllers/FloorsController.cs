using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FloorsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public FloorsController(UserDbContext context, ILogger<FloorsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Floors
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Floor>>> Getfloors()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.floors.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Floors/5
        [HttpGet("getFloor")]
        [Authorize]
        public async Task<ActionResult<Floor>> GetFloor(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var floor = await _context.floors.FindAsync(id);

                    if (floor == null)
                    {
                        return NotFound();
                    }

                    return floor;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Floors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFloor(Floor floor)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    floor.companyId = claimresponse.companyId;
                    _context.Entry(floor).State = EntityState.Modified;
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

        // POST: api/Floors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Floor>> PostFloor(Floor floor)
        {
            try
            {
                
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (FloorExists(floor.floorAutoId))
                    {
                        var facilityFloors = _context.floors.Where(d => d.companyId == claimresponse.companyId).Select(f => f.floorId).ToList();
                        List<string> floorList = new List<string>();
                        List<int> floorNoList = new List<int>();
                        foreach (var z in facilityFloors)
                        {
                            if (z.Contains(floor.facilitySingleId + "F"))
                            {
                                floorList.Add(z);
                            }
                        }
                        if (floorList.Count > 0)
                        {
                            foreach (var x in floorList)
                            {
                                floorNoList.Add(int.Parse(x.Split("F").Last().ToString()));
                            }
                        }

                        if (floorNoList.Count == 0)
                        {
                            _context.ChangeTracker.Clear();
                            Floor f = new Floor();
                            f.floorId = floor.facilitySingleId + "F1";
                            f.floorName = floor.floorName;
                            f.companyId = claimresponse.companyId;
                            f.status = "Active";
                            f.facilityAutoId = floor.facilityAutoId;
                            _context.floors.Add(f);
                            await _context.SaveChangesAsync();
                        }
                        if (floorNoList.Count > 0)
                        {
                            _context.ChangeTracker.Clear();
                            Floor f = new Floor();
                            string comId = floor.facilitySingleId + "F" + (floorNoList.Max() + 1);
                            f.floorId = comId;
                            f.floorName = floor.floorName;
                            f.companyId = claimresponse.companyId;
                            f.status = "Active";
                            f.facilityAutoId = floor.facilityAutoId;
                            _context.floors.Add(f);
                            await _context.SaveChangesAsync();
                        }
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

        // DELETE: api/Floors/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (FloorExists(id))
                    {
                        var floor = await _context.floors.FindAsync(id);
                        if (floor == null)
                        {
                            return NotFound();
                        }

                        _context.floors.Remove(floor);
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

        private bool FloorExists(int id)
        {
            return _context.floors.Any(e => e.floorAutoId == id);
        }
    }
}
