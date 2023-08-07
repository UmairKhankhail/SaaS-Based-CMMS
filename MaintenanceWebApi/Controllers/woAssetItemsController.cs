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
    public class woAssetItemsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<woAssetItemsController> _logger;


        public woAssetItemsController(MaintenanceDbContext context,JwtTokenHandler jwtTokenHandler, ILogger<woAssetItemsController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _logger= logger;
        }

        // GET: api/woAssetItems
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<woAssetItem>>> GetwoAssetItems()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.woAssetItems.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/woAssetItems/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<woAssetItem>> GetwoAssetItem(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var woAssetItem = await _context.woAssetItems.Where(x => x.woAssetItemAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (woAssetItem == null)
                    {
                        return NotFound();
                    }

                    return Ok(woAssetItem);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/woAssetItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutwoAssetItem(int id,woAssetItem woAssetItem)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (woAssetItem.woAssetItemAutoId == id && woAssetItem.companyId == claimresponse.companyId)
                    {
                        _context.Entry(woAssetItem).State = EntityState.Modified;

                    }


                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!woAssetItemExists(id))
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

        // POST: api/woAssetItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<woAssetItem>> PostwoAssetItem(woAssetItem woAssetItem)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    _context.woAssetItems.Add(woAssetItem);
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

        // DELETE: api/woAssetItems/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletewoAssetItem(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var woAssetItem = await _context.woAssetItems.FindAsync(id);
                    if (woAssetItem == null)
                    {
                        return NotFound();
                    }

                    _context.woAssetItems.Remove(woAssetItem);
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

        private bool woAssetItemExists(int id)
        {
            return _context.woAssetItems.Any(e => e.woAssetItemAutoId == id);
        }
    }
}
