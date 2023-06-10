﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspectionEntriesController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly ILogger<InspectionEntriesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public InspectionEntriesController(PreventiveMaintenanceDbContext context, HttpClient httpClient, ILogger<InspectionEntriesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }


        [HttpGet("GetInspectionEntriesByAssetId")]
        public async Task<ActionResult<IEnumerable<InspectionEntry>>> GetInspectionByAssetId(int assetModel, string assetId)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var inspections = await _context.inspectionEntries
                    .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == claimresponse.companyId)
                    .ToListAsync();

                    if (inspections == null)
                    {
                        return Unauthorized();
                    }

                    return inspections;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet("GetInspectionEntriesByAssetIdForQuestions")]
        public async Task<ActionResult<IEnumerable<string>>> GetInspectionByAssetIdForQuestions(int assetModel, string assetId, string question)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var inspection = await _context.inspections
                    .Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.question == question && x.companyId == claimresponse.companyId)
                    .Select(x => x.options)
                    .FirstOrDefaultAsync();

                    if (inspection == null)
                    {
                        return Unauthorized();
                    }

                    return Ok(new List<string> { inspection });
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }


        // GET: api/InspectionEntries
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<InspectionEntry>>> GetinspectionEntries()
        //{

        //    return await _context.inspectionEntries.ToListAsync();
        //}

        // GET: api/InspectionEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionEntry>> GetInspectionEntry(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var inspectionEntry = await _context.inspectionEntries.FindAsync(id);

                    if (inspectionEntry == null)
                    {
                        return NotFound();
                    }

                    return inspectionEntry;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        // PUT: api/InspectionEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInspectionEntry(int id, InspectionEntry inspectionEntry)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    inspectionEntry.companyId = claimresponse.companyId;
                    _context.Entry(inspectionEntry).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!InspectionEntryExists(id))
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

        // POST: api/InspectionEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InspectionEntry>> PostInspectionEntry(InspectionEntry inspectionEntry)
        {

            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getmrId = 0;

                    InspectionEntry inse = new InspectionEntry();
                    inse.assetModelId = inspectionEntry.assetModelId;
                    inse.assetId = inspectionEntry.assetId;
                    inse.question = inspectionEntry.question;
                    inse.selectedOption = inspectionEntry.selectedOption;
                    inse.companyId = claimresponse.companyId;
                    inse.remarks = inspectionEntry.remarks;


                    var inspection = _context.inspections.Where(x => x.question == inse.question && x.companyId == claimresponse.companyId && x.assetId == inse.assetId).FirstOrDefault();
                    if (inspection == null)
                    {
                        return NotFound();
                    }

                    string listOptions = inspection.options;
                    string listWorkRequests = inspection.workRequestOfOptions;

                    // Convert the strings to lists
                    List<string> options = JsonConvert.DeserializeObject<List<string>>(listOptions);
                    List<int> workRequests = JsonConvert.DeserializeObject<List<int>>(listWorkRequests);


                    int index = options.FindIndex(item => item == inse.selectedOption);
                    if (workRequests[index] == 1)
                    {
                        Console.WriteLine("Generate Work Request");
                        var url = "http://localhost:5145/api/WorkRequests";
                        var parameters = new Dictionary<string, string>
                    {
                        { "topName", inse.question + " " + inse.selectedOption },
                    { "description", inse.remarks  },
                    { "approve", "Y" },
                    { "companyId", claimresponse.companyId }
                    };


                        var json = JsonConvert.SerializeObject(parameters);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await _httpClient.PostAsync(url, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(responseContent);
                    }

                    _context.inspectionEntries.Add(inse);
                    await _context.SaveChangesAsync();
                    getmrId = inse.inspectionEntryAutoId;
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

            
            //_context.inspectionEntries.Add(inspectionEntry);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetInspectionEntry", new { id = inspectionEntry.inspectionEntryAutoId }, inspectionEntry);
        }

        // DELETE: api/InspectionEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionEntry(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var inspectionEntry = await _context.inspectionEntries.FindAsync(id);
                    if (inspectionEntry == null)
                    {
                        return NotFound();
                    }

                    _context.inspectionEntries.Remove(inspectionEntry);
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

        private bool InspectionEntryExists(int id)
        {
            return _context.inspectionEntries.Any(e => e.inspectionEntryAutoId == id);
        }
    }
}
