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
    public class MethodtypesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<MethodtypesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;

        public MethodtypesController(UserDbContext context, ILogger<MethodtypesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Methodtypes
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Methodtype>>> Getmethodtypes()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.methodTypes.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Methodtypes/5
        [HttpGet("getMethodType")]
        [Authorize]
        public async Task<ActionResult<Methodtype>> GetMethodtype(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var methodType = await _context.methodTypes.FindAsync(id);

                    if (methodType == null)
                    {
                        return NotFound();
                    }

                    return methodType;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Methodtypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMethodtype(Methodtype methodtype)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (MethodtypeExists(methodtype.mtAutoId))
                    {
                        methodtype.companyId = claimresponse.companyId;
                        _context.Entry(methodtype).State = EntityState.Modified;
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

        // POST: api/Methodtypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Methodtype>> PostMethodtype(Methodtype methodType)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                    if (claimresponse.isAuth == true)
                    {
                        var compId = _context.methodTypes.Where(d => d.companyId == claimresponse.companyId).Select(d => d.mtId).ToList();
                        var autoId = "";
                        if (compId.Count > 0)
                        {
                            autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                        }

                        if (autoId == "")
                        {
                            _context.ChangeTracker.Clear();
                            Methodtype m = new Methodtype();
                            string comId = "M1";
                            m.mtId = comId;
                            m.mtName = methodType.mtName;
                            m.companyId = claimresponse.companyId;
                            m.status = methodType.status;
                            _context.methodTypes.Add(m);
                            await _context.SaveChangesAsync();
                            //return Ok(c);
                        }
                        if (autoId != "")
                        {
                            _context.ChangeTracker.Clear();
                            Methodtype m = new Methodtype();
                            string comId = "M" + (int.Parse(autoId) + 1);
                            m.mtId = comId;
                            m.mtName = methodType.mtName;
                            m.companyId = claimresponse.companyId;
                            m.status = methodType.status;
                            _context.methodTypes.Add(m);
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

        // DELETE: api/Methodtypes/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMethodtype(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (MethodtypeExists(id))
                    {
                        var methodType = await _context.methodTypes.FindAsync(id);

                        _context.methodTypes.Remove(methodType);
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

        private bool MethodtypeExists(int id)
        {
            return _context.methodTypes.Any(e => e.mtAutoId == id);
        }
    }
}
