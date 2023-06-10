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

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinearAssetsController : ControllerBase
    {
        private readonly AssetDbContext _context;
        private readonly ILogger<LinearAssetsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;

        public LinearAssetsController(AssetDbContext context, ILogger<LinearAssetsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/LinearAssets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinearAsset>>> GetlinearAssets()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.linearAssets.Where(x => x.companyId == claimresponse.companyId ).ToListAsync();
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
        public async Task<ActionResult<LinearAsset>> GetLinearAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearAsset = await _context.linearAssets.Where(x => x.companyId == claimresponse.companyId  && x.lAssetAuotId==id).ToListAsync();
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
        public async Task<IActionResult> PutLinearAsset(LinearAsset linearAsset)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    linearAsset.companyId = claimresponse.companyId;
                    _context.Entry(linearAsset).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LinearAssetExists(linearAsset.lAssetAuotId))
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
        public async Task<ActionResult<LinearAsset>> PostLinearAsset(LinearAsset linearAsset)
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
                        var compId = _context.linearAssets.Where(d => d.companyId == claimresponse.companyId).Select(d => d.lAssetId).ToList();

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
                            LinearAsset la = new LinearAsset();
                            string comId = linearAsset.flId + "ALL1";
                            la.lAssetId = comId;
                            la.laAssetName = linearAsset.laAssetName;
                            la.companyId = claimresponse.companyId;
                            la.code = linearAsset.code;
                            la.description = linearAsset.description;
                            la.deptId = linearAsset.deptId;
                            la.subDeptId = linearAsset.subDeptId;
                            la.flId = linearAsset.flId;
                            la.laAutoId = linearAsset.laAutoId;
                            _context.linearAssets.Add(la);
                            await _context.SaveChangesAsync();

                        }
                        if (autoId != "")
                        {
                            _context.ChangeTracker.Clear();
                            LinearAsset la = new LinearAsset();
                            string comId = linearAsset.flId + "ALL" + (int.Parse(autoId) + 1);
                            la.lAssetId = comId;
                            la.laAssetName = linearAsset.laAssetName;
                            la.companyId = claimresponse.companyId;
                            la.code = linearAsset.code;
                            la.deptId = linearAsset.deptId;
                            la.description = linearAsset.description;
                            la.subDeptId = linearAsset.subDeptId;
                            la.flId = linearAsset.flId;
                            la.laAutoId = linearAsset.laAutoId;
                            _context.linearAssets.Add(la);
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
        public async Task<IActionResult> DeleteLinearAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearAsset = await _context.linearAssets.FindAsync(id);
                    if (linearAsset == null)
                    {
                        return NotFound();
                    }

                    _context.linearAssets.Remove(linearAsset);
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
            return _context.linearAssets.Any(e => e.lAssetAuotId == id);
        }
    }
}
