using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using System.Drawing.Drawing2D;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasurementsController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<UnitOfMeasurementsController> _logger;


        public UnitOfMeasurementsController(InventoryDbContext context, HttpClient httpClient, ILogger<UnitOfMeasurementsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;

        }

        // GET: api/UnitOfMeasurements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitOfMeasurement>>> GetunitOfMeasurements()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.unitOfMeasurements.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();

                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/UnitOfMeasurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitOfMeasurement>> GetUnitOfMeasurement(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var unitOfMeasurement = await _context.unitOfMeasurements.Where(x => x.uomAutoId == id && x.companyId == claimresponse.companyId && x.status == "Active").FirstOrDefaultAsync();


                    if (unitOfMeasurement == null)
                    {
                        return NotFound();
                    }

                    return unitOfMeasurement;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/UnitOfMeasurements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitOfMeasurement(int id, UnitOfMeasurement unitOfMeasurement)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (UnitOfMeasurementExists(id) && CompanyExists(claimresponse.companyId))
                    {
                        UnitOfMeasurement uom = new UnitOfMeasurement();
                        uom.uomAutoId = unitOfMeasurement.uomAutoId;
                        uom.uomId = unitOfMeasurement.uomId;
                        uom.uomName = unitOfMeasurement.uomName;
                        uom.status = unitOfMeasurement.status;
                        uom.companyId = claimresponse.companyId;

                        _context.Entry(uom).State = EntityState.Modified;

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

        // POST: api/UnitOfMeasurements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnitOfMeasurement>> PostUnitOfMeasurement(UnitOfMeasurement unitOfMeasurement)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {


                    var comId = _context.unitOfMeasurements.Where(uom => uom.companyId == claimresponse.companyId).Select(uom => uom.uomId).ToList();

                    Console.WriteLine(comId);
                    var autoId = "";
                    if (comId.Count > 0)
                    {
                        autoId = comId.Max(x => int.Parse(x.Substring(3))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        UnitOfMeasurement uom = new UnitOfMeasurement();
                        string comid = "uom1";
                        uom.uomId = comid;
                        uom.uomName = unitOfMeasurement.uomName;
                        uom.status = unitOfMeasurement.status;
                        uom.companyId = claimresponse.companyId;
                        _context.unitOfMeasurements.Add(uom);
                        await _context.SaveChangesAsync();
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        UnitOfMeasurement uom = new UnitOfMeasurement();
                        string comid = "uom" + (int.Parse(autoId) + 1);
                        uom.uomId = comid;
                        uom.uomName = unitOfMeasurement.uomName;
                        uom.status = unitOfMeasurement.status;
                        uom.companyId = claimresponse.companyId;
                        _context.unitOfMeasurements.Add(uom);
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

        // DELETE: api/UnitOfMeasurements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasurement(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var uom = await _context.unitOfMeasurements.Where(x => x.uomAutoId == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (uom == null)
                    {
                        return NotFound();
                    }

                    _context.unitOfMeasurements.Remove(uom);
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

        private bool UnitOfMeasurementExists(int id)
        {
            return _context.unitOfMeasurements.Any(e => e.uomAutoId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
