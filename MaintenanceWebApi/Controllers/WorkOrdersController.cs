using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly ILogger<WorkOrdersController> _logger;


        public WorkOrdersController(MaintenanceDbContext context, JwtTokenHandler jwtTokenHandler, ILogger<WorkOrdersController> logger)
        {
            _context = context;
            _JwtTokenHandler= jwtTokenHandler;
            _logger = logger;
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

        // GET: api/WorkOrders
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetworkOrders()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.workOrders.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/WorkOrders/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WorkOrder>> GetWorkOrder(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var workOrder = await _context.workOrders.Where(x => x.woAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (workOrder == null)
                    {
                        return NotFound();
                    }

                    return Ok(workOrder);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/WorkOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutWorkOrder(int id,WorkOrder workOrder)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (workOrder.woAutoId == id && workOrder.companyId == claimresponse.companyId)
                    {
                        _context.Entry(workOrder).State = EntityState.Modified;
                    }



                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WorkOrderExists(id))
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

        // POST: api/WorkOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WorkOrder>> PostWorkOrder(WorkOrder workOrder)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var comId = _context.workOrders.Where(wo => wo.companyId ==claimresponse.companyId).Select(wo => wo.woId).ToList();

                    var autoId = "";
                    if (comId.Count > 0)
                    {
                        autoId = comId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        WorkOrder wo = new WorkOrder();
                        string comid = "WO1";
                        wo.woId = comid;
                        wo.woType = workOrder.woType;
                        wo.topName = workOrder.topName;
                        wo.requestId = workOrder.requestId;
                        wo.assetDetials = workOrder.assetDetials;
                        wo.companyId = claimresponse.companyId;
                        _context.workOrders.Add(wo);
                        await _context.SaveChangesAsync();
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        WorkOrder wo = new WorkOrder();
                        string comid = "WO" + (int.Parse(autoId) + 1);
                        wo.woId = comid;
                        wo.woType = workOrder.woType;
                        wo.topName = workOrder.topName; 
                        wo.requestId = workOrder.requestId;
                        wo.assetDetials = workOrder.assetDetials;
                        wo.companyId = claimresponse.companyId;
                        _context.workOrders.Add(wo);
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

        // DELETE: api/WorkOrders/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteWorkOrder(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var workOrder = await _context.workOrders.FindAsync(id);
                    if (workOrder == null)
                    {
                        return NotFound();
                    }

                    _context.workOrders.Remove(workOrder);
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

        private bool WorkOrderExists(int id)
        {
            return _context.workOrders.Any(e => e.woAutoId == id);
        }
    }
}
