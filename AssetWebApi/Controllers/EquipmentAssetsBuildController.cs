using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using NuGet.ContentModel;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentAssetsBuildController : ControllerBase
    {
        private readonly AssetDbContext _context;
        private readonly ILogger<EquipmentAssetsBuildController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;


        public EquipmentAssetsBuildController(AssetDbContext context, ILogger<EquipmentAssetsBuildController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/EquipmentAssetsBuild
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EquipmentAsset>>> GetequipmentAssets()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.equipmentAssets.Where(x=>x.companyId==claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/EquipmentAssetsBuild/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EquipmentAsset>> GetEquipmentAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var equipmentAsset = await _context.equipmentAssets.Where(x=>x.eAssetAuotoId==id && x.companyId==claimresponse.companyId).FirstOrDefaultAsync();

                    if (equipmentAsset == null)
                    {
                        return NotFound();
                    }

                    return equipmentAsset;

                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            
        }

        // PUT: api/EquipmentAssetsBuild/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutEquipmentAsset(EquipmentAsset equipmentAsset)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var existingModel = await _context.equipmentAssets.FindAsync(equipmentAsset.eAssetAuotoId);


                    existingModel.eAssetId = equipmentAsset.eAssetId;
                    existingModel.eAutoId = equipmentAsset.eAutoId;
                    existingModel.eAssetName = equipmentAsset.eAssetName;
                    existingModel.brandNmae = equipmentAsset.brandNmae;
                    existingModel.code = equipmentAsset.code;
                    existingModel.description = equipmentAsset.description;
                    existingModel.deptId = equipmentAsset.deptId;
                    existingModel.subDeptId = equipmentAsset.subDeptId;
                    existingModel.flId = equipmentAsset.flId;
                    existingModel.companyId = claimresponse.companyId;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {

                    }

                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


            
        }

        // POST: api/EquipmentAssetsBuild
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<EquipmentAsset>> PostEquipmentAsset(EquipmentAsset equipmentAsset)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    int getEquipmentAutoId = 0;
                    var compId = _context.equipmentAssets.Where(d => d.companyId == claimresponse.companyId).Select(d => d.eAssetId).ToList();

                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => {
                            int startIndex = x.IndexOf("AAE") + 3;
                            string numbers = x.Substring(startIndex).TrimStart('0');
                            return int.Parse(numbers);
                        }).ToString();

                    }

                    if (autoId == "")
                    {
                        //B1F1L1D1S1
                        _context.ChangeTracker.Clear();
                        EquipmentAsset e = new EquipmentAsset();
                        string comId = equipmentAsset.flId + "AAE1";
                        e.eAssetId = comId;
                        e.eAutoId = equipmentAsset.eAutoId;
                        e.eAssetName = equipmentAsset.eAssetName;
                        e.brandNmae = equipmentAsset.brandNmae;
                        e.code = equipmentAsset.code;
                        e.description = equipmentAsset.description;
                        e.deptId = equipmentAsset.deptId;
                        e.subDeptId = equipmentAsset.subDeptId;
                        e.flId = equipmentAsset.flId;
                        e.companyId = claimresponse.companyId;
                        _context.equipmentAssets.Add(e);
                        await _context.SaveChangesAsync();
                        getEquipmentAutoId = e.eAssetAuotoId;
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        EquipmentAsset e = new EquipmentAsset();
                        string comId = equipmentAsset.flId + "AAE" + (int.Parse(autoId) + 1);
                        e.eAssetId = comId;
                        e.eAutoId = equipmentAsset.eAutoId;
                        e.eAssetName = equipmentAsset.eAssetName;
                        e.brandNmae = equipmentAsset.brandNmae;
                        e.code = equipmentAsset.code;
                        e.description = equipmentAsset.description;
                        e.deptId = equipmentAsset.deptId;
                        e.subDeptId = equipmentAsset.subDeptId;
                        e.flId = equipmentAsset.flId;
                        e.companyId = claimresponse.companyId;
                        _context.equipmentAssets.Add(e);
                        await _context.SaveChangesAsync();
                        getEquipmentAutoId = e.eAutoId;
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

            
            //_context.equipmentAssets.Add(equipmentAsset);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEquipmentAsset", new { id = equipmentAsset.eAssetAuotoId }, equipmentAsset);
        }

        // DELETE: api/EquipmentAssetsBuild/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEquipmentAsset(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var equipmentAsset = await _context.equipmentAssets.FindAsync(id);
                    if (equipmentAsset == null)
                    {
                        return NotFound();
                    }

                    _context.equipmentAssets.Remove(equipmentAsset);
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

        private bool EquipmentAssetExists(int id)
        {
            return _context.equipmentAssets.Any(e => e.eAssetAuotoId == id);
        }
    }
}
