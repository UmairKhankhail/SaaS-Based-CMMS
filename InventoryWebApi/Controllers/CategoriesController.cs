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
using JwtAuthenticationManager.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
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
        public async Task<ActionResult<IEnumerable<Category>>> Getcategories()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.categories.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }

                return Unauthorized();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var category = await _context.categories.Where(x => x.catAutoId == id && x.companyId == claimresponse.companyId && x.status == "Active").FirstOrDefaultAsync();

                    if (category == null)
                    {
                        return NotFound();
                    }

                    return category;
                }
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutCategory(Category category, int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (CategoryExists(id) && CompanyExists(claimresponse.companyId))
                    {
                        Category c = new Category();
                        c.catAutoId = category.catAutoId;
                        c.catId = category.catId;
                        c.catName = category.catName;
                        c.status = category.status;
                        c.companyId = claimresponse.companyId;
                        c.groupAutoId = category.groupAutoId;
                        _context.Entry(c).State = EntityState.Modified;

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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory( Category category)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var comId = _context.categories.Where(g => g.companyId == claimresponse.companyId).Select(g => g.catId).ToList();

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
                        c.companyId = claimresponse.companyId;
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
                        c.companyId = claimresponse.companyId;
                        c.groupAutoId = category.groupAutoId;
                        _context.categories.Add(c);
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

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var category = await _context.categories.Where(x => x.catId == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();

                    if (category == null)
                    {
                        return NotFound();
                    }

                    _context.categories.Remove(category);
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

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.catAutoId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.Inventorygroups.Any(x => x.companyId == id);
        }
    }
}
