using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using RedisCachingService;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly UserDbContext _context;
        private readonly ICacheService _cacheService;
        public string companyIdGlobal { get; set; }
        public CompaniesController(UserDbContext context, ILogger<CompaniesController> logger, ICacheService cacheService)
        {
            _context = context;
            _logger = logger;
            _cacheService = cacheService;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            
            try
            {

                //object m = null;
                //string s = m.ToString();
                return await _context.companies.ToListAsync();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("com")]
        public async Task<ActionResult<IEnumerable<Company>>> GetCaheData()
        {
            //var newcache = _cacheService.RemoveData("1");
            
            var newcache = _cacheService.GetData<IEnumerable<string>>("1");
            var newcache1 = _cacheService.GetData<string>("1Param");
            //if (cacheData != null && cacheData.Count()>0)
            //    return Ok(cacheData);
            //cacheData= await _context.companies.ToListAsync();

            //var expiryTime = DateTimeOffset.Now.AddSeconds(30);
            //_cacheService.SetData<IEnumerable<Company>>("com", cacheData, expiryTime);
            return Ok(newCache);
        }

        //[HttpPost("adddrivers")]
        //public async Task<ActionResult> PostCache()
        //{
        //    var addedObject=
        //}
            // GET: api/Companies/5
            [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(string id)
        {
            try
            {
                var company = await _context.companies.FindAsync(id);

                if (company == null)
                {
                    return NotFound();
                }

                return company;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(string id, Company company)
        {
            try
            {
                if (id != company.companyId)
                {
                    return BadRequest();
                }

                _context.Entry(company).State = EntityState.Modified;


                await _context.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }

        // POST: api/Companies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Company>>> PostCompany(Company company)
        {
            try
            {
                var compId = _context.companies.Select(x => x.companyId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                //string autoid = _context.companies.Max(p => p.companyid);
                //var autoid=_context.companies.OrderByDescending(x => x.companyid).FirstOrDefault().ToString();

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Company c = new Company();
                    string comId = "C1";
                    c.companyId = comId;
                    companyIdGlobal = comId;
                    c.companyName = company.companyName;
                    c.companyEmail = company.companyEmail;
                    c.companyPhone = company.companyPhone;
                    c.userFirstName = company.userFirstName;
                    c.userLastName = company.userLastName;
                    c.password = company.password;
                    c.status = company.status;
                    _context.companies.Add(c);
                    var record = await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Company c = new Company();
                    string comId = "C" + (int.Parse(autoId) + 1);
                    c.companyId = comId;
                    companyIdGlobal = comId;
                    c.companyName = company.companyName;
                    c.companyEmail = company.companyEmail;
                    c.companyPhone = company.companyPhone;
                    c.userFirstName = company.userFirstName;
                    c.userLastName = company.userLastName;
                    c.password = company.password;
                    c.status = company.status;
                    _context.companies.Add(c);
                    var record = await _context.SaveChangesAsync();
                    //return Ok(c);
                }

                return await _context.companies.ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                var company = await _context.companies.FindAsync(id);
                if (company == null)
                {
                    return NotFound();
                }

                _context.companies.Remove(company);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool CompanyExists(string id)
        {
            return _context.companies.Any(e => e.companyId == id);
        }
    }
}
