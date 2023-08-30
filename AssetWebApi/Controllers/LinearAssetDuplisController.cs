using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinearAssetsDuplisController : ControllerBase
    {
        private readonly AssetDbContext _context;
        private readonly ILogger<LinearAssetsDuplisController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;

        public LinearAssetsDuplisController(AssetDbContext context, ILogger<LinearAssetsDuplisController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/LinearAssets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<LinearAssetDupli>>> GetlinearAssets()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.linearAssetDuplis.Where(x => x.companyId == claimresponse.companyId ).ToListAsync();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/LinearAssets/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<LinearAssetDupli>> GetLinearAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearAsset = await _context.linearAssetDuplis.Where(x => x.companyId == claimresponse.companyId  && x.lAssetAuotId==id).ToListAsync();
                    if (linearAsset == null)
                    {
                        return NotFound();
                    }

                    return Ok(linearAsset);

                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/LinearAssets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutLinearAsset(LinearAssetDupli linearAssetDupli)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    linearAssetDupli.companyId = claimresponse.companyId;
                    _context.Entry(linearAssetDupli).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LinearAssetExists(linearAssetDupli.lAssetAuotId))
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

        // POST: api/LinearAssets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<LinearAssetDupli>> PostLinearAsset(LinearAssetDupli linearAssetDupli)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    try
                    {

                        int getlaAutoId = 0;
                        var compId = _context.linearAssetDuplis.Where(d => d.companyId == claimresponse.companyId).Select(d => d.lAssetId).ToList();

                        var autoId = "";
                        if (compId.Count > 0)
                        {
                            autoId = compId.Max(x => {
                                int startIndex = x.IndexOf("ALL") + 3;
                                string numbers = x.Substring(startIndex).TrimStart('0');
                                return int.Parse(numbers);
                            }).ToString();

                        }

                        if (autoId == "")
                        {
                            _context.ChangeTracker.Clear();
                            LinearAssetDupli la = new LinearAssetDupli();
                            string comId = linearAssetDupli.flId + "ALL1";
                            la.lAssetId = comId;
                            la.laAssetName = linearAssetDupli.laAssetName;
                            la.companyId = claimresponse.companyId;
                            la.code = linearAssetDupli.code;
                            la.description = linearAssetDupli.description;
                            la.deptId = linearAssetDupli.deptId;
                            la.subDeptId = linearAssetDupli.subDeptId;
                            la.flId = linearAssetDupli.flId;
                            _context.linearAssetDuplis.Add(la);
                            await _context.SaveChangesAsync();

                        }
                        if (autoId != "")
                        {
                            _context.ChangeTracker.Clear();
                            LinearAssetDupli la = new LinearAssetDupli();
                            string comId = linearAssetDupli.flId + "ALL" + (int.Parse(autoId) + 1);
                            la.lAssetId = comId;
                            la.laAssetName = linearAssetDupli.laAssetName;
                            la.companyId = claimresponse.companyId;
                            la.code = linearAssetDupli.code;
                            la.deptId = linearAssetDupli.deptId;
                            la.description = linearAssetDupli.description;
                            la.subDeptId = linearAssetDupli.subDeptId;
                            la.flId = linearAssetDupli.flId;
                          
                            _context.linearAssetDuplis.Add(la);
                            await _context.SaveChangesAsync();

                        }

                        //Console.WriteLine("Id: "+ getroleautoid.ToString());
                        //_context.roles.Add(role);
                        //await _context.SaveChangesAsync()


                        return Ok();

                        //    }
                        //    return Unauthorized();

                    }
                    catch (Exception ex)
                    {
                        //_logger.LogError(ex.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
            //_context.equipmentModels.Add(equipmentModel);
            //await _context.SaveChangesAsync();
        }

        // DELETE: api/LinearAssets/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLinearAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearAsset = await _context.linearAssetDuplis.FindAsync(id);
                    if (linearAsset == null)
                    {
                        return NotFound();
                    }

                    _context.linearAssetDuplis.Remove(linearAsset);
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

        private bool LinearAssetExists(int id)
        {
            return _context.linearAssetDuplis.Any(e => e.lAssetAuotId == id);
        }
    }
}
