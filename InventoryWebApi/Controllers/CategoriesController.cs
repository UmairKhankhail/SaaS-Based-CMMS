using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using JwtAuthenticationManager;
using System.Net.Http;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriesController> _logger;


        public CategoriesController(InventoryDbContext context, HttpClient httpClient, ILogger<CategoriesController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Getcategories(string companyId)
        {
            try
            {

                return await _context.categories.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(string id, string companyId)
        {
            var category= await _context.categories.Where(x => x.catId == id && x.companyId == companyId && x.status == "Active").FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory([FromBody] Category category, string id,string companyId)
        {
            if (CategoryExists(id) && CompanyExists(companyId))
            {
                Category c = new Category();
                 c.catAutoId = category.catAutoId;
                c.catId = category.catId;
                c.catName = category.catName;
                c.status = category.status;
                c.companyId = category.companyId;
                c.groupAutoId = category.groupAutoId;
                _context.Entry(c).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(string companyId,  Category category)
        {
            var comId = _context.categories.Where(g => g.companyId == companyId).Select(g => g.catId).ToList();

            var autoId = "";
            if (comId.Count > 0)
            {
                autoId = comId.Max(x => int.Parse(x.Substring(1))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                Category c = new Category();
                string comid = "C1";
                c.catId = comid;
                c.catName = category.catName;
                c.status = category.status;
                c.companyId = category.companyId;
                c.groupAutoId = category.groupAutoId;
                _context.categories.Add(c);
                await _context.SaveChangesAsync();
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                Category c = new Category();
                string comid = "C" + (int.Parse(autoId) + 1);
                c.catId = comid;
                c.catName = category.catName;
                c.status = category.status;
                c.companyId = category.companyId;
                c.groupAutoId=category.groupAutoId;
                _context.categories.Add(c);
                await _context.SaveChangesAsync();

            }

            return Ok();
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id,string companyId)
        {
            var category = await _context.categories.Where(x => x.catId == id && x.companyId == companyId).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            _context.categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(string id)
        {
            return _context.categories.Any(e => e.catId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.Inventorygroups.Any(x => x.companyId == id);
        }
    }
}
