﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using System.Dynamic;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EvaluationsController> _logger;
        public EvaluationsController(MaintenanceDbContext context, HttpClient httpClient, ILogger<EvaluationsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
            _httpClient=httpClient;


        }

        // GET: api/Evaluations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Evaluation>>> Getevaluations()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.evaluations.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Evaluations/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Evaluation>> GetEvaluation(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var evaluation = await _context.evaluations.Where(x => x.evaluationAutoId == id && x.companyId == claimresponse.companyId).ToListAsync();

                    if (evaluation == null)
                    {
                        return NotFound();
                    }
                    return Ok(evaluation);
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/Evaluations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutEvaluation(int id,Evaluation evaluation)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (evaluation.evaluationAutoId == id && evaluation.companyId == claimresponse.companyId)
                    {

                        _context.Entry(evaluation).State = EntityState.Modified;

                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EvaluationExists(id))
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

        // POST: api/Evaluations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Evaluation>> PostEvaluation(Evaluation evaluation)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                var compId = _context.evaluations.Where(eval => eval.companyId == claimresponse.companyId && eval.woAutoId == evaluation.woAutoId).Select(d => d.evaluationId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {

                            autoId = compId.Max(x => int.Parse(x.Substring(4))).ToString();
                        }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Evaluation eval = new Evaluation();
                    string comId = "EVAL1";
                    eval.evaluationAutoId = evaluation.evaluationAutoId;
                    eval.evaluationId = comId;
                    eval.woAutoId = evaluation.woAutoId;
                    eval.userName = evaluation.userName;
                    eval.topName =evaluation.topName;
                    eval.startTime = evaluation.startTime;
                    eval.endTime = evaluation.endTime;


                            var url = $"http://localhost:85/api/ScheduledWorkRequests/GetCalendarRecords";
                            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            // Set the Content-Type header to application/json
                            _httpClient.DefaultRequestHeaders.Accept.Clear();
                            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await _httpClient.GetAsync(url);
                            var responseContent = await response.Content.ReadAsStringAsync();

                            JObject dataObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                            var calenderId = Convert.ToString(dataObject["googleCalendarId"]);
                            var timeZoneRegional = Convert.ToString(dataObject["timeZoneRegional"]);

                            Console.WriteLine(timeZoneRegional + " " + calenderId);

                            Event newEvent = new Event
                            {
                                Summary = "Word Order Evaluation Request",
                                Start = new EventDateTime { DateTime = eval.startTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                                End = new EventDateTime { DateTime = eval.endTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                                Description = "Work Order Evaluation - " + eval.evaluationId
                            };

                            GoogleCalendar googleCalendar = new GoogleCalendar();

                            var eventId = googleCalendar.InsertEvent(newEvent, calenderId, timeZoneRegional);

                            
                    eval.eventId = eventId;
                    eval.remarks = evaluation.remarks;
                    eval.companyId = claimresponse.companyId;
                    _context.evaluations.Add(eval);
                    await _context.SaveChangesAsync();
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Evaluation eval = new Evaluation();
                    string comId = "EVAL" + (int.Parse(autoId) + 1);
                    eval.evaluationAutoId = evaluation.evaluationAutoId;
                    eval.evaluationId = comId;
                    eval.woAutoId = evaluation.woAutoId;
                    eval.userName = evaluation.userName;
                    eval.topName = evaluation.topName;
                    eval.startTime = evaluation.startTime;
                    eval.endTime = evaluation.endTime;



                        var url = $"http://localhost:85/api/ScheduledWorkRequests/GetCalendarRecords";
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        // Set the Content-Type header to application/json
                        _httpClient.DefaultRequestHeaders.Accept.Clear();
                        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var response = await _httpClient.GetAsync(url);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        JObject dataObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                        var calenderId = Convert.ToString(dataObject["googleCalendarId"]);
                        var timeZoneRegional = Convert.ToString(dataObject["timeZoneRegional"]);

                        Console.WriteLine(timeZoneRegional + " " + calenderId);

                        Event newEvent = new Event
                        {
                            Summary = "Word Order Evaluation Request",
                            Start = new EventDateTime { DateTime = eval.startTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                            End = new EventDateTime { DateTime = eval.endTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                            Description = "Work Order Evaluation - " + eval.evaluationId
                        };

                        GoogleCalendar googleCalendar = new GoogleCalendar();

                        var eventId = googleCalendar.InsertEvent(newEvent, calenderId, timeZoneRegional);


                    eval.eventId = eventId;
                    eval.remarks = evaluation.remarks;
                    eval.companyId = claimresponse.companyId;
                    _context.evaluations.Add(eval);
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



        // DELETE: api/Evaluations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvaluation(int id, string cId)
        {
            var evaluation = await _context.evaluations.FindAsync(id);
            if (evaluation == null)
            {
                return NotFound();
            }

            _context.evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvaluationExists(int id)
        {
            return _context.evaluations.Any(e => e.evaluationAutoId == id);
        }

    }
}
