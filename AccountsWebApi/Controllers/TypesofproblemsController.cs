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
    public class TypesofproblemsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<TypesofproblemsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public TypesofproblemsController(UserDbContext context, ILogger<TypesofproblemsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Typesofproblems
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Typesofproblem>>> Gettypesofproblems()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.typesOfProblems.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Typesofproblems/5
        [HttpGet("getTypeOfProblem")]
        [Authorize]
        public async Task<ActionResult<Typesofproblem>> GetTypesofproblem(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var typesOfProblem = await _context.typesOfProblems.FindAsync(id);

                    if (typesOfProblem == null)
                    {
                        return NotFound();
                    }

                    return typesOfProblem;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Typesofproblems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutTypesofproblem(Typesofproblem typesofproblem)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (TypesofproblemExists(typesofproblem.topAutoId))
                    {
                        typesofproblem.companyId = claimresponse.companyId;
                        _context.Entry(typesofproblem).State = EntityState.Modified;

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

        // POST: api/Typesofproblems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Typesofproblem>> PostTypesofproblem(Typesofproblem typesOfProblem)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var compId = _context.typesOfProblems.Where(d => d.companyId == claimresponse.companyId).Select(d => d.topId).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Typesofproblem m = new Typesofproblem();
                        string comid = "TP1";
                        m.topId = comid;
                        m.topName = typesOfProblem.topName;
                        m.companyId = claimresponse.companyId;
                        m.description=typesOfProblem.description;
                        m.status = typesOfProblem.status;
                        _context.typesOfProblems.Add(m);
                        await _context.SaveChangesAsync();
                        //return Ok(c);
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Typesofproblem m = new Typesofproblem();
                        string comid = "TP" + (int.Parse(autoId) + 1);
                        m.topId = comid;
                        m.topName = typesOfProblem.topName;
                        m.companyId = claimresponse.companyId;
                        m.description = typesOfProblem.description;
                        m.status = typesOfProblem.status;
                        _context.typesOfProblems.Add(m);
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

        // DELETE: api/Typesofproblems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTypesofproblem(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (TypesofproblemExists(id))
                    {
                        var typesOfProblem = await _context.typesOfProblems.FindAsync(id);
                        if (typesOfProblem == null)
                        {
                            return NotFound();
                        }

                        _context.typesOfProblems.Remove(typesOfProblem);
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

        private bool TypesofproblemExists(int id)
        {
            return _context.typesOfProblems.Any(e => e.topAutoId == id);
        }
    }
}
