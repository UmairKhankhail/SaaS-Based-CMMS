using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using System.Drawing.Drawing2D;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitOfMeasurementsController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriesController> _logger;


        public UnitOfMeasurementsController(InventoryDbContext context, HttpClient httpClient, ILogger<CategoriesController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;

        }

        // GET: api/UnitOfMeasurements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnitOfMeasurement>>> GetunitOfMeasurements(string companyId)
        {
            try
            {

                return await _context.unitOfMeasurements.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/UnitOfMeasurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitOfMeasurement>> GetUnitOfMeasurement(string id, string companyId)
        {
            var unitOfMeasurement = await _context.unitOfMeasurements.Where(x => x.uomId == id && x.companyId == companyId && x.status == "Active").FirstOrDefaultAsync();


            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            return unitOfMeasurement;
        }

        // PUT: api/UnitOfMeasurements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnitOfMeasurement(string id,string companyId, UnitOfMeasurement unitOfMeasurement)
        {
            if (UnitOfMeasurementExists(id) && CompanyExists(companyId))
            {
                UnitOfMeasurement uom = new UnitOfMeasurement();
                uom.uomAutoId = unitOfMeasurement.uomAutoId;
                uom.uomId = unitOfMeasurement.uomId;
                uom.uomName = unitOfMeasurement.uomName;
                uom.status = unitOfMeasurement.status;
                uom.companyId = unitOfMeasurement.companyId;

                _context.Entry(uom).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();

        }

        // POST: api/UnitOfMeasurements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnitOfMeasurement>> PostUnitOfMeasurement(UnitOfMeasurement unitOfMeasurement,string companyId)
        {
            var comId = _context.unitOfMeasurements.Where(uom => uom.companyId == companyId).Select(uom => uom.uomId).ToList();

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
                uom.companyId = unitOfMeasurement.companyId;
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
                uom.companyId = unitOfMeasurement.companyId;
                _context.unitOfMeasurements.Add(uom);
                await _context.SaveChangesAsync();
            }

            return Ok();

        }

        // DELETE: api/UnitOfMeasurements/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnitOfMeasurement(string id, string companyId)
        {
            var uom = await _context.unitOfMeasurements.Where(x => x.uomId == id && x.companyId == companyId).FirstOrDefaultAsync();
            if (uom == null)
            {
                return NotFound();
            }

            _context.unitOfMeasurements.Remove(uom);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnitOfMeasurementExists(string id)
        {
            return _context.unitOfMeasurements.Any(e => e.uomId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
