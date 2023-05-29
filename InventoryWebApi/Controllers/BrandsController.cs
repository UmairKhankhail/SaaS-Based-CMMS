using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriesController> _logger;


        public BrandsController(InventoryDbContext context, HttpClient httpClient, ILogger<CategoriesController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> Getbrands(string companyId)
        {
            try
            {

                return await _context.brands.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(string id, string companyId)
        {
            var brand = await _context.brands.Where(x => x.brandId == id && x.companyId == companyId && x.status == "Active").FirstOrDefaultAsync();

            if (brand == null)
            {
                return NotFound();
            }

            return Ok();

        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(string id,string companyId, Brand brand)
        {
            if (BrandExists(id) && CompanyExists(companyId))
            {
                Brand b = new Brand();
                b.brandAutoId = brand.brandAutoId;
                b.brandId = brand.brandId;
                b.brandName = brand.brandName;
                b.status = brand.status;
                b.companyId = brand.companyId;
            
                _context.Entry(b).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand,string companyId)
        {
            var comId = _context.brands.Where(b => b.companyId == companyId).Select(b => b.brandId).ToList();

            var autoId = "";
            if (comId.Count > 0)
            {
                autoId = comId.Max(x => int.Parse(x.Substring(1))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                Brand b = new Brand();
                string comid = "B1";
                b.brandId = comid;
                b.brandName = brand.brandName;
                b.status = brand.status;
                b.companyId = brand.companyId;
                _context.brands.Add(b);
                await _context.SaveChangesAsync();
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                Brand b = new Brand();
                string comid = "B" + (int.Parse(autoId) + 1);
                b.brandId = comid;
                b.brandName = brand.brandName;
                b.status = brand.status;
                b.companyId = brand.companyId;
                _context.brands.Add(b);
                await _context.SaveChangesAsync();

            }

            return Ok();
        }

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(string id, string companyId)
        {
            var brand = await _context.brands.Where(x => x.brandId == id && x.companyId == companyId).FirstOrDefaultAsync();
            if (brand == null)
            {
                return NotFound();
            }

            _context.brands.Remove(brand);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BrandExists(string id)
        {
            return _context.brands.Any(e => e.brandId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
