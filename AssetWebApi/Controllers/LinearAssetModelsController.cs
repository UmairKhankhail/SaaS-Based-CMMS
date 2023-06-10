using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using System.Reflection;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinearAssetModelsController : ControllerBase
    {
        private readonly AssetDbContext _context;
        private readonly ILogger<LinearAssetModelsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;

        public LinearAssetModelsController(AssetDbContext context, ILogger<LinearAssetModelsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        [HttpGet("GetControllersAndMethods")]
        public async Task<List<string>> GetAllControllerMethods()
        {
            var methods = new List<string>();
            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

            foreach (var controllerType in controllerTypes)
            {
                var controllerMethods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(method => !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute)));

                foreach (var method in controllerMethods)
                {
                    methods.Add($"{controllerType.Name}.{method.Name}");
                }
            }

            return methods;
        }

        // GET: api/LinearAssetModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinearAssetModel>>> GetlinearAssetModels()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var result = await _context.linearAssetModels.Join(
                _context.linearSubItems, la => la.laAutoID, lsm => lsm.laAutoId, (la, lsm) => new { la, lsm }).
                Where(x => x.la.laAutoID == x.lsm.laAutoId && x.la.companyId == claimresponse.companyId).
                Select(result => new
                {
                    result.la.laID,
                    result.la.laName,
                    result.la.status,
                    result.lsm.lsName,
                    result.lsm.location,
                    result.lsm.description

                }).ToListAsync();
                    return Ok(result);
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
         }
        // GET: api/LinearAssetModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LinearAssetModel>> GetLinearAssetModel(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearAsssetModel = await _context.linearAssetModels.Join(
                _context.linearSubItems, la => la.laAutoID, lsm => lsm.laAutoId, (la, lsm) => new { la, lsm }).
                Where(x => x.la.companyId == claimresponse.companyId && x.la.laAutoID == id && x.la.laAutoID == x.lsm.laAutoId).
                Select(result => new
                {
                    result.la.status,
                    result.lsm.lsName,
                    result.lsm.location,
                    result.lsm.description

                }).ToListAsync();
                    return Ok(linearAsssetModel);

                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            
        }

        // PUT: api/LinearAssetModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutLinearAssetModel(LinearAssetModel linearAssetModel)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (linearAssetModel.validityCheck != 0)
                    {
                        var existingModel = await _context.linearAssetModels.FindAsync(linearAssetModel.validityCheck);

                        existingModel.laName = linearAssetModel.laName;
                        try
                        {
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError(ex.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                    }


                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        // POST: api/LinearAssetModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinearAssetModel>> PostLinearAssetModel(LinearAssetModel linearAssetModel)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getLamAutoId = 0;
                    var compId = _context.linearAssetModels.Where(i => i.companyId == claimresponse.companyId).Select(d => d.laID).ToList();

                    var autoId = "";
                    if (compId.Count > 0)
                    {

                        autoId = compId.Max(x => int.Parse(x.Substring(3))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        LinearAssetModel laModel = new LinearAssetModel();
                        string comId = "LAM1";
                        laModel.laAutoID = linearAssetModel.laAutoID;
                        laModel.laID = comId;
                        laModel.laName = linearAssetModel.laName;
                        laModel.status = linearAssetModel.status;
                        laModel.companyId = claimresponse.companyId;
                        _context.linearAssetModels.Add(laModel);
                        await _context.SaveChangesAsync();
                        getLamAutoId = laModel.laAutoID;
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        LinearAssetModel laModel = new LinearAssetModel();
                        string comId = "LAM" + (int.Parse(autoId) + 1);
                        laModel.laAutoID = linearAssetModel.laAutoID;
                        laModel.laID = comId;
                        laModel.laName = linearAssetModel.laName;
                        laModel.status = linearAssetModel.status;
                        laModel.companyId = claimresponse.companyId;
                        _context.linearAssetModels.Add(laModel);
                        await _context.SaveChangesAsync();
                        getLamAutoId = laModel.laAutoID;
                    }

                    List<LinearSubItemList> linearSubItemLists = linearAssetModel.listSubItems;

                    foreach (var items in linearSubItemLists)
                    {
                        LinearSubItem laSubItem = new LinearSubItem();
                        laSubItem.laAutoId = getLamAutoId;
                        laSubItem.lsName = items.lsName;
                        laSubItem.description = items.description;
                        laSubItem.location = items.location;
                        laSubItem.companyId = claimresponse.companyId;
                        _context.linearSubItems.Add(laSubItem);
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

        // DELETE: api/LinearAssetModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinearAssetModel(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearSubItems = await _context.linearSubItems.FindAsync(id);
                    if (linearSubItems == null)
                    {
                        return NotFound();
                    }

                    _context.linearSubItems.Remove(linearSubItems);
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

        private bool LinearAssetModelExists(int id)
        {
            return _context.linearAssetModels.Any(e => e.laAutoID == id);
        }
    }
}
