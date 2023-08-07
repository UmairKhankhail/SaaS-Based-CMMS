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
    public class InstructionsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<InstructionsController> _logger;
        public InstructionsController(MaintenanceDbContext context, JwtTokenHandler jwtTokenHandler, ILogger<InstructionsController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _logger = logger;
        }

        // GET: api/Instructions
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Instruction>>> Getinstructions()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.instructions.Where(x => x.companyId ==claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Instructions/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Instruction>> GetInstruction(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var instruction = await _context.instructions.Where(x => x.insAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (instruction == null)
                    {
                        return NotFound();
                    }

                    return Ok(instruction);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/Instructions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutInstruction(int id,Instruction instruction)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (instruction.insAutoId == id && instruction.companyId == claimresponse.companyId)
                    {
                        _context.Entry(instruction).State = EntityState.Modified;
                    }


                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InstructionExists(id))
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

        // POST: api/Instructions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Instruction>> PostInstruction(Instruction instruction)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    _context.instructions.Add(instruction);
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

        // DELETE: api/Instructions/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInstruction(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var instruction = await _context.instructions.FindAsync(id);
                    if (instruction == null)
                    {
                        return NotFound();
                    }

                    _context.instructions.Remove(instruction);
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

        private bool InstructionExists(int id)
        {
            return _context.instructions.Any(e => e.insAutoId == id);
        }
    }
}
