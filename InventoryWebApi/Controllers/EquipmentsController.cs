using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EquipmentsController> _logger;


        public EquipmentsController(InventoryDbContext context, HttpClient httpClient, ILogger<EquipmentsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
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

        // GET: api/Equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> Getequipments()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.equipments.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();

                }
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var equipment = await _context.equipments.Where(x => x.equipAutoId == id && x.companyId == claimresponse.companyId && x.status == "Active").FirstOrDefaultAsync();

                    if (equipment == null)
                    {
                        return NotFound();
                    }

                    return equipment;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEquipment(string id,Equipment equipment)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (EquipmentExists(id) && CompanyExists(claimresponse.companyId))
                    {
                        Equipment equip = new Equipment();
                        equip.equipAutoId = equipment.equipAutoId;
                        equip.equipId = equipment.equipId;
                        equip.equipName = equipment.equipName;
                        equip.equipCost = equipment.equipCost;
                        equip.quantity = equipment.quantity;
                        equip.equipLeadTime = equipment.equipLeadTime;
                        equip.status = equipment.status;
                        equip.catAutoId = equipment.catAutoId;
                        equip.groupAutoId = equipment.groupAutoId;
                        equip.companyId = claimresponse.companyId;

                        _context.Entry(equip).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                        return Ok();
                    }

                        return NotFound();
                    }
                    return Unauthorized();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

        }

        // POST: api/Equipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var comId = _context.equipments.Where(equip => equip.companyId == claimresponse.companyId).Select(equip => equip.equipId).ToList();
                    var autoId = "";
                    if (comId.Count > 0)
                    {
                        autoId = comId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Equipment equip = new Equipment();
                        equip.equipAutoId = equipment.equipAutoId;
                        string comid = "E1";
                        equip.equipId = comid;
                        equip.equipName = equipment.equipName;
                        equip.equipCost = equipment.equipCost;
                        equip.quantity = equipment.quantity;
                        equip.equipLeadTime = equipment.equipLeadTime;
                        equip.status = equipment.status;
                        equip.catAutoId = equipment.catAutoId;
                        equip.groupAutoId = equipment.groupAutoId;
                        equip.companyId = claimresponse.companyId;

                        _context.equipments.Add(equip);
                        await _context.SaveChangesAsync();
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Equipment equip = new Equipment();
                        equip.equipAutoId = equipment.equipAutoId;
                        string comid = "E" + (int.Parse(autoId) + 1);
                        equip.equipId = comid;
                        equip.equipName = equipment.equipName;
                        equip.equipCost = equipment.equipCost;
                        equip.quantity = equipment.quantity;
                        equip.equipLeadTime = equipment.equipLeadTime;
                        equip.status = equipment.status;
                        equip.catAutoId = equipment.catAutoId;
                        equip.groupAutoId = equipment.groupAutoId;
                        equip.companyId = claimresponse.companyId;

                        _context.equipments.Add(equip);
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

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var equipment = await _context.equipments.Where(x => x.equipAutoId == id).FirstOrDefaultAsync();

                    if (equipment == null)
                    {
                        return NotFound();
                    }

                    _context.equipments.Remove(equipment);
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

        private bool EquipmentExists(string id)
        {
            return _context.equipments.Any(e => e.equipId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
