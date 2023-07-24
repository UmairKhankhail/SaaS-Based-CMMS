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
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<LinearSubItem>>> GetlinearSubItems(int id)
        {

            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearSubItems = _context.linearSubItems
                    .Where(x => x.laAutoId == id && x.companyId == claimresponse.companyId)
                    .Select(x => new
                    {
                        lsName = x.lsName,
                        location = x.location,
                        description=x.description,
                        laAutoId=x.laAutoId,
                        lsAutoId=x.lsAutoId,
                        lsParentId = string.IsNullOrEmpty(x.lsParentId.ToString())
                            ? "No Parent"
                            : _context.linearSubItems.Any(s => s.lsAutoId.ToString() == x.lsParentId.ToString())
                                ? _context.linearSubItems.FirstOrDefault(s => s.lsAutoId.ToString() == x.lsParentId.ToString()).lsName
                                : "No Parent"
                    })
                    .ToList();

                    return Ok(linearSubItems);
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }


        [HttpGet("getAllModels")]
        [Authorize]
        public async Task<ActionResult<LinearAssetModel>> GetAllLinearModel()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearModel = await _context.linearAssetModels.ToListAsync();

                    if (linearModel == null)
                    {
                        return NotFound();
                    }

                    return Ok(linearModel);
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
        [Authorize]
        public async Task<ActionResult<LinearAssetModel>> GetLinearAssetModel(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var linearModel = await _context.linearAssetModels.FindAsync(id);

                    if (linearModel == null)
                    {
                        return NotFound();
                    }

                    return linearModel;
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
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<LinearAssetModel>> PostLinearAssetModel(LinearAssetModel linearAssetModel)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    try
                    {
                        if (linearAssetModel.validityCheck == 0)
                        {

                            int getlinearAutoId = 0;
                            var compId = _context.linearAssetModels.Where(d => d.companyId == claimresponse.companyId).Select(d => d.laID).ToList();

                            var autoId = "";
                            if (compId.Count > 0)
                            {

                                autoId = compId.Max(x => int.Parse(x.Substring(3))).ToString();
                            }

                            if (autoId == "")
                            {
                                _context.ChangeTracker.Clear();
                                LinearAssetModel e = new LinearAssetModel();
                                string comId = "LAM1";
                                e.laID = comId;
                                e.laName = linearAssetModel.laName;
                                e.companyId = claimresponse.companyId;
                                e.status = linearAssetModel.status;
                                _context.linearAssetModels.Add(e);
                                await _context.SaveChangesAsync();
                                getlinearAutoId = e.laAutoID;
                            }
                            if (autoId != "")
                            {
                                _context.ChangeTracker.Clear();
                                LinearAssetModel e = new LinearAssetModel();
                                string comId = "LAM" + (int.Parse(autoId) + 1);
                                e.laID = comId;
                                e.laName = linearAssetModel.laName;
                                e.companyId = claimresponse.companyId;
                                e.status = linearAssetModel.status;
                                _context.linearAssetModels.Add(e);
                                await _context.SaveChangesAsync();
                                getlinearAutoId = e.laAutoID;
                            }

                            //Console.WriteLine("Id: "+ getroleautoid.ToString());
                            //_context.roles.Add(role);
                            //await _context.SaveChangesAsync();

                            List<LinearSubItemList> linearSubItemLists = linearAssetModel.listSubItems;



                            foreach (var items in linearSubItemLists)
                            {
                                LinearSubItem eSubItem = new LinearSubItem();
                                eSubItem.laAutoId = getlinearAutoId;
                                eSubItem.lsName = items.lsName;
                                eSubItem.description = items.description;
                                eSubItem.location = items.location;
                                eSubItem.lsParentId = items.lsParentId;
                                eSubItem.companyId = claimresponse.companyId;
                                _context.linearSubItems.Add(eSubItem);
                                await _context.SaveChangesAsync();
                            }
                            //foreach (var items in listPermissions)
                            //{
                            //    RoleandPermission rolePerm = new RoleandPermission();
                            //    rolePerm.permissionId = items.ToString();
                            //    rolePerm.roleAutoId = getRoleAutoId;
                            //    rolePerm.companyId = claimresponse.companyId;
                            //    _context.roleAndPermissions.Add(rolePerm);
                            //    await _context.SaveChangesAsync();
                            //}
                        }
                        else if (linearAssetModel.validityCheck != 0)
                        {
                            var validityCheckValue = linearAssetModel.validityCheck;
                            if (LinearModelValidityExists(validityCheckValue))
                            {

                                List<LinearSubItemList> linearSubItemsLists = linearAssetModel.listSubItems;



                                foreach (var items in linearSubItemsLists)
                                {
                                    LinearSubItem eSubItem = new LinearSubItem();
                                    eSubItem.laAutoId = validityCheckValue;
                                    eSubItem.lsName = items.lsName;
                                    eSubItem.description = items.description;
                                    eSubItem.location = items.location;
                                    eSubItem.lsParentId = items.lsParentId;
                                    eSubItem.companyId = claimresponse.companyId;
                                    _context.linearSubItems.Add(eSubItem);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                return Unauthorized();
                            }
                        }


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

        }

        // DELETE: api/LinearAssetModels/5
        [HttpDelete("{id}")]
        [Authorize]
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
        [HttpDelete("DeleteFullLinearModel")]
        [Authorize]
        public async Task<IActionResult> DeleteFullLinearModel(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (id != null)
                    {
                        var equipmentModel = await _context.linearAssetModels.Where(x => x.laAutoID == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                        if (equipmentModel == null)
                        {
                            return NotFound();
                        }

                        _context.linearAssetModels.Remove(equipmentModel);
                        await _context.SaveChangesAsync();
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


        private bool LinearModelValidityExists(int id)
        {
            return _context.linearAssetModels.Any(e => e.laAutoID == id);
        }
        private bool LinearAssetModelExists(int id)
        {
            return _context.linearAssetModels.Any(e => e.laAutoID == id);
        }
    }
}
