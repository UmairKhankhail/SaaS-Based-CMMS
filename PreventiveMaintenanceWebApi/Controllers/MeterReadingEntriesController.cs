using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MeterReadingEntriesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public MeterReadingEntriesController(PreventiveMaintenanceDbContext context, HttpClient httpClient, ILogger<MeterReadingEntriesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/MeterReadingEntries
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetmeterReadingEntries(string id)
        //{
        //    //var url = $"http://localhost:5145/api/WorkRequests?companyId={id}";
        //    //var response = await _httpClient.GetAsync(url);
        //    //var responseContent = await response.Content.ReadAsStringAsync();
        //    //Console.WriteLine(responseContent);
        //    //List<string> responseList = new List<string>();
        //    //var mynewlist = JsonConvert.DeserializeObject<List<string>>(responseContent);
            
        //    //Console.WriteLine(mynewlist);

        //    return await _context.meterReadingEntries.ToListAsync();
        //}

        // GET: api/MeterReadingEntries/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MeterReadingEntry>> GetMeterReadingEntry(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReadingEntry = await _context.meterReadingEntries.Where(x=>x.companyId==claimresponse.companyId && x.mreAutoId==id).FirstOrDefaultAsync();

                    if (meterReadingEntry == null)
                    {
                        return NotFound();
                    }

                    return meterReadingEntry;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetAllMeterReadingEntry()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReadingEntry = await _context.meterReadingEntries.Where(x=>x.companyId==claimresponse.companyId).ToListAsync();

                    if (meterReadingEntry == null)
                    {
                        return NotFound();
                    }

                    return meterReadingEntry;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        [HttpGet("GetMeterReadingEntriesByAssetId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MeterReadingEntry>>> GetMeterReadingEntriesByAssetId(int assetModel, string assetId)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var mre = await _context.meterReadingEntries
                .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == claimresponse.companyId)
                .ToListAsync();

                    if (mre == null)
                    {
                        return Unauthorized();
                    }

                    return mre;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet("GetMeterReadingEntriesByAssetIdForParams")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> GetMeterReadingEntriesByAssetIdForParams(int assetModel, string assetId, string paramName)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var mre = await _context.meterReadingEntries
                .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == claimresponse.companyId && x.paramName==paramName)
                .Select(x => x.paramName)
                .FirstOrDefaultAsync();

                    if (mre == null)
                    {
                        return Unauthorized();
                    }

                    return Ok(new List<string> { mre });
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        // PUT: api/MeterReadingEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutMeterReadingEntry(MeterReadingEntry meterReadingEntry)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    meterReadingEntry.companyId = claimresponse.companyId;
                    _context.Entry(meterReadingEntry).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MeterReadingEntryExists(meterReadingEntry.mreAutoId))
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

        // POST: api/MeterReadingEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MeterReadingEntry>> PostMeterReadingEntry(MeterReadingEntry meterReadingEntry)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getmrId = 0;

                    MeterReadingEntry mre = new MeterReadingEntry();
                    mre.assetModelId = meterReadingEntry.assetModelId;
                    mre.assetId = meterReadingEntry.assetId;
                    mre.paramName = meterReadingEntry.paramName;
                    mre.value = meterReadingEntry.value;
                    mre.companyId = claimresponse.companyId;
                    mre.remarks = meterReadingEntry.remarks;


                    var meterReading = _context.meterReadings.Where(x => x.paramName == mre.paramName && x.companyId == claimresponse.companyId && x.assetId == mre.assetId).FirstOrDefault();
                    if (meterReading == null)
                    {
                        return NotFound();
                    }
                    if (mre.value >= meterReading.minValue && mre.value <= meterReading.maxValue) { }
                    else
                    {
                        Console.WriteLine("Generate Work Request");
                        var url = "http://localhost:84/api/WorkRequests";
                        var parameters = new Dictionary<string, string>
                    {
                            { "username", "System Generated"},
                        { "topName", mre.paramName + " " + mre.value },
                    { "description", mre.remarks  },
                    { "approve", "Y" },
                    { "companyId", claimresponse.companyId }
                    };


                        var json = JsonConvert.SerializeObject(parameters);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        //var response = await _httpClient.PostAsync(url, content);
                        //var responseContent = await response.Content.ReadAsStringAsync();

                        //Console.WriteLine(responseContent);

                        // Set the authorization header with the JWT token
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        var response = await _httpClient.PostAsync(url, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(responseContent);

                    }

                    _context.meterReadingEntries.Add(mre);
                    await _context.SaveChangesAsync();
                    getmrId = mre.mreAutoId;
                    Console.WriteLine(getmrId);

                    return Ok();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            


            //_context.meterReadingEntries.Add(meterReadingEntry);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMeterReadingEntry", new { id = meterReadingEntry.mreAutoId }, meterReadingEntry);
        }

        // DELETE: api/MeterReadingEntries/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMeterReadingEntry(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReadingEntry = await _context.meterReadingEntries.FindAsync(id);
                    if (meterReadingEntry == null)
                    {
                        return NotFound();
                    }

                    _context.meterReadingEntries.Remove(meterReadingEntry);
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

        private bool MeterReadingEntryExists(int id)
        {
            return _context.meterReadingEntries.Any(e => e.mreAutoId == id);
        }
    }
}
