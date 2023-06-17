using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<BrandsController> _logger;


        public BrandsController(InventoryDbContext context, HttpClient httpClient, ILogger<BrandsController> logger , JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Brands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brand>>> Getbrands()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.brands.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();

                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        // GET: api/Brands/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brand>> GetBrand(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var brand = await _context.brands.Where(x => x.brandAutoId == id && x.companyId == claimresponse.companyId && x.status == "Active").FirstOrDefaultAsync();

                    if (brand == null)
                    {
                        return NotFound();
                    }
                    return Ok(brand);

                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/Brands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrand(int id, Brand brand)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (BrandExists(id) && CompanyExists(claimresponse.companyId))
                    {
                        Brand b = new Brand();
                        b.brandAutoId = brand.brandAutoId;
                        b.brandId = brand.brandId;
                        b.brandName = brand.brandName;
                        b.status = brand.status;
                        b.companyId = claimresponse.companyId;

                        _context.Entry(b).State = EntityState.Modified;

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

        // POST: api/Brands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {


                    var comId = _context.brands.Where(b => b.companyId == claimresponse.companyId).Select(b => b.brandId).ToList();

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
                        b.companyId = claimresponse.companyId;
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
                        b.companyId = claimresponse.companyId;
                        _context.brands.Add(b);
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

        // DELETE: api/Brands/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var brand = await _context.brands.Where(x => x.brandId == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (brand == null)
                    {
                        return NotFound();
                    }

                    _context.brands.Remove(brand);
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

        private bool BrandExists(int id)
        {
            return _context.brands.Any(e => e.brandAutoId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
