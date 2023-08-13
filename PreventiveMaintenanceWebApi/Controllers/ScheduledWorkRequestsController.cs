using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduledWorkRequestsController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly GoogleCalendar _calendar;
        private readonly ILogger<ScheduledWorkRequestsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public ScheduledWorkRequestsController(PreventiveMaintenanceDbContext context, GoogleCalendar calendar, ILogger<ScheduledWorkRequestsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _calendar = calendar;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }



        // GET: api/ScheduledWorkRequests
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ScheduledWorkRequest>>> GetscheduledWorkRequests()
        //{
        //    // Assuming you have a valid _calendarService object

        //    // Define the calendar ID of the calendar you want to retrieve events from
        //    // Assuming you have a valid _calendarService object

        //    // Define the calendar ID of the calendar you want to retrieve events from
        //    //string calendarId = "primary"; // Use "primary" for the primary calendar

        //    //GoogleCalendar googleCalender = new GoogleCalendar();
        //    ////Event newEvent = new Event
        //    ////{
        //    ////    Summary = "Scheduled Work",
        //    ////    Start = new EventDateTime { DateTime = swr.initialDateTime },
        //    ////    End = new EventDateTime { DateTime = swr.initialDateTime.AddMinutes(30) }, // Adjust the end time as needed
        //    ////    Description = swr.description
        //    ////};
        //    //var eventId = googleCalender.GetEvent(eId);
        //    //Console.WriteLine(eventId.Start.DateTime);
        //    //Console.WriteLine(eventId.End.DateTime);



        //    return await _context.scheduledWorkRequests.ToListAsync();
        //}

        [HttpGet("GetControllersAndMethods")]
        public async Task<List<string>> GetAllControllerMethods()
        {
            var methods = new List<string>();
            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

            foreach (var controllerType in controllerTypes)
            {
                var controllerMethods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(method => !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute)));

                foreach (var method in controllerMethods)
                {
                    methods.Add($"{controllerType.Name}.{method.Name}");
                }
            }

            return methods;
        }




        [HttpGet("GetscheduledWorkByAssetId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ScheduledWorkRequest>>> GetscheduledWorkDetails(int assetModel, string assetId)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var scheduledWorkRequest = _context.scheduledWorkRequests.Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == claimresponse.companyId).ToList();
                    if (scheduledWorkRequest == null)
                    {
                        return Unauthorized();
                    }
                    return scheduledWorkRequest;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        [HttpGet("GetscheduledWorkByAssetIdForHeadOfProblem")]
        [Authorize]
        public async Task<ActionResult<ScheduledWorkRequest>> GetscheduledWorkDetailsHOP(int assetModel, string assetId, string headOfProblems)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var scheduledWorkRequest = _context.scheduledWorkRequests.Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.headOfProblem == headOfProblems && x.companyId == claimresponse.companyId).FirstOrDefault();
                    if (scheduledWorkRequest == null)
                    {
                        return Unauthorized();
                    }
                    return scheduledWorkRequest;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        // GET: api/ScheduledWorkRequests/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ScheduledWorkRequest>> GetScheduledWorkRequest(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var scheduledWorkRequest = await _context.scheduledWorkRequests.Where(x => x.swrAutoId==id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();

                    if (scheduledWorkRequest == null)
                    {
                        return NotFound();
                    }

                    return scheduledWorkRequest;
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
        public async Task<ActionResult<IEnumerable<ScheduledWorkRequest>>> GetScheduledWorkRequest()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var scheduledWorkRequest = await _context.scheduledWorkRequests.Where(x=>x.companyId==claimresponse.companyId).ToListAsync();

                    if (scheduledWorkRequest == null)
                    {
                        return NotFound();
                    }

                    return scheduledWorkRequest;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/ScheduledWorkRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //[HttpGet("GetCalendar")]
        //[Authorize]
        //public async Task<ActionResult<GoogleCalendarRecord>> GetCalendar(int id)
        //{
        //    try
        //    {

        //        var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        //        var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
        //        if (claimresponse.isAuth == true)
        //        {
        //            var existingmodel = await _context.googleCalendarRecords.FindAsync(id);

        //            if (existingmodel == null)
        //            {
        //                return NotFound();
        //            }

        //            return existingmodel;
        //        }
        //        return Unauthorized();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }

        //}

        //[HttpGet("GetCalendarDetails")]
        //[Authorize]
        //public async Task<ActionResult> GetCalendarDetails(string calendarid)
        //{
        //    try
        //    {
        //        var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
        //        var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });

        //        if (claimresponse.isAuth == true)
        //        {
        //            // Create an instance of the GoogleCalendar class
        //            var googleCalendar = new GoogleCalendar();

        //            // Call the GetCalendarDetails method from the GoogleCalendar class
        //            var calendarDetails = googleCalendar.GetCalendarDetails(calendarid);

        //            // Return the calendar details as the response
        //            return Ok(calendarDetails);
        //        }

        //        return Unauthorized();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}


        [HttpGet("GetCalendarRecords")]
        [Authorize]
        public async Task<ActionResult<GoogleCalendarRecord>> GetCalendarRecords()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var existingModel = _context.googleCalendarRecords.Where(x => x.companyId == claimresponse.companyId).FirstOrDefault();
                    if (existingModel == null)
                    {
                        return Unauthorized();
                    }
                    return existingModel;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpPost("PostGoogleCalendarApi")]
        [Authorize]
        public async Task<ActionResult> PostGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    int getgcrId = 0;
                    GoogleCalendarRecord gcr = new GoogleCalendarRecord();
                    gcr.calendarSummary = googleCalendarRecord.calendarSummary;
                    gcr.calendarDescription = googleCalendarRecord.calendarDescription;
                    
                    gcr.timeZoneRegional = googleCalendarRecord.timeZoneRegional;
                    gcr.timeZoneWord = googleCalendarRecord.timeZoneWord;
                    gcr.timeZoneGMT = googleCalendarRecord.timeZoneGMT;
                    gcr.companyId = claimresponse.companyId;


                    GoogleCalendar gc = new GoogleCalendar();
                    var gcId = gc.CreateCalendar(gcr.calendarSummary, gcr.calendarDescription, gcr.timeZoneRegional);
                    gcr.googleCalendarId = gcId;

                    
                    //IFrame
                    gcr.iFrame = $"<iframe src='https://calendar.google.com/calendar/embed?src={gcr.googleCalendarId}' style='border: 0' width='800' height='600' frameborder='0' scrolling='no'></iframe>";

                    _context.googleCalendarRecords.Add(gcr);
                    await _context.SaveChangesAsync();
                    getgcrId = gcr.googleCalendarAutoId;

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
        [HttpPut("UpdateGoogleCalendarApi")]
        [Authorize]
        public async Task<IActionResult> UpdateGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var existingModel = await _context.googleCalendarRecords.FindAsync(googleCalendarRecord.googleCalendarAutoId);
                    if (existingModel != null)
                    {
                        existingModel.calendarDescription = googleCalendarRecord.calendarDescription;
                        existingModel.calendarSummary = googleCalendarRecord.calendarSummary;
                        existingModel.timeZoneRegional = googleCalendarRecord.timeZoneRegional;
                        existingModel.timeZoneWord = googleCalendarRecord.timeZoneWord;
                        existingModel.timeZoneGMT = googleCalendarRecord.timeZoneGMT;
                        existingModel.iFrame = googleCalendarRecord.iFrame.ToString();
                        existingModel.googleCalendarId = googleCalendarRecord.googleCalendarId;

                        try
                        {
                            await _context.SaveChangesAsync();
                            GoogleCalendar gc = new GoogleCalendar();
                            gc.UpdateCalendar(existingModel.googleCalendarId, existingModel.calendarSummary, existingModel.calendarDescription, existingModel.timeZoneRegional);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError(ex.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpDelete("RemoveGoogleCalendarApi")]
        [Authorize]
        public async Task<IActionResult> DeleteGoogleCalendarApi(GoogleCalendarRecord googleCalendarRecord)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var existingModel = await _context.googleCalendarRecords.FindAsync(googleCalendarRecord.googleCalendarAutoId);
                    if (existingModel != null)
                    {
                        try
                        {
                            _context.googleCalendarRecords.Remove(existingModel);
                            await _context.SaveChangesAsync();
                            GoogleCalendar gc = new GoogleCalendar();
                            gc.DeleteCalendar(existingModel.googleCalendarId);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            //_logger.LogError(ex.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
                    }
                    return Unauthorized();
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpPut("PutScheduledWorkRequest")]
        [Authorize]
        public async Task<IActionResult> PutScheduledWorkRequest(ScheduledWorkRequest scheduledWorkRequest)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (calInfo == null)
                    {
                        return Unauthorized();
                    }

                    var existingModel = await _context.scheduledWorkRequests.FindAsync(scheduledWorkRequest.swrAutoId);

                    existingModel.initialDateTime = scheduledWorkRequest.initialDateTime;
                    existingModel.description = scheduledWorkRequest.description;
                    existingModel.frequencyDays = scheduledWorkRequest.frequencyDays;

                    try
                    {
                        GoogleCalendar googleCalendar = new GoogleCalendar();

                        //DateTime conversion to users timezone
                        var startDateTime = existingModel.initialDateTime;
                        var endDateTime = startDateTime.AddDays(existingModel.frequencyDays);
                        //Conversion Ends

                        Event newEvent = new Event
                        {
                            Summary = "Recurrent Scheduled Work Request",
                            Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                            End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                            Description = existingModel.description.ToString() + "/n" + existingModel.assetId + "/n" + "Updated"
                        };

                        googleCalendar.UpdateRecurringEvent(calInfo.googleCalendarId, existingModel.eventIdCalendar, newEvent, existingModel.frequencyDays, calInfo.timeZoneRegional);
                        //Db DateTime Work to exact same format
                        DateTime inputDateTime = scheduledWorkRequest.initialDateTime;
                        string timeZoneId = calInfo.timeZoneWord;
                        // Convert parsed datetime to universal datetime
                        DateTime universalDateTime = inputDateTime.ToUniversalTime();

                        // Convert universal datetime to desired timezone
                        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                        DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                        existingModel.initialDateTime = convertedDateTime;
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        //_logger.LogError(ex.Message);
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
         
        }


        // POST: api/ScheduledWorkRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ScheduledWorkRequest>> PostScheduledWorkRequest(ScheduledWorkRequest scheduledWorkRequest)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    //Getting CalendarInfo
                    var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (calInfo == null)
                    {
                        return Unauthorized();
                    }



                    var getswrId = 0;

                    ScheduledWorkRequest swr = new ScheduledWorkRequest();
                    swr.assetModelId = scheduledWorkRequest.assetModelId;
                    swr.assetId = scheduledWorkRequest.assetId;
                    swr.headOfProblem = scheduledWorkRequest.headOfProblem;
                    swr.frequencyDays = scheduledWorkRequest.frequencyDays;
                    swr.initialDateTime = scheduledWorkRequest.initialDateTime;
                    swr.description = scheduledWorkRequest.description;
                    swr.companyId = claimresponse.companyId;


                    GoogleCalendar googleCalendar = new GoogleCalendar();

                    //DateTime conversion to users timezone
                    var startDateTime = swr.initialDateTime;
                    var endDateTime = startDateTime.AddDays(swr.frequencyDays);
                    //Conversion Ends

                    Event newEvent = new Event
                    {
                        Summary = "Recurrent Scheduled Work Request",
                        Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                        End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                        Description = swr.description.ToString() + "/n" + swr.assetId
                    };


                    Console.WriteLine(newEvent);
                    var eventId = googleCalendar.InsertRecurringEvent(newEvent, swr.frequencyDays, calInfo.googleCalendarId, calInfo.timeZoneRegional);
                    Console.WriteLine(eventId);

                    //Db DateTime Work to exact same format
                    DateTime inputDateTime = scheduledWorkRequest.initialDateTime;
                    string timeZoneId = calInfo.timeZoneWord;
                    // Convert parsed datetime to universal datetime
                    DateTime universalDateTime = inputDateTime.ToUniversalTime();

                    // Convert universal datetime to desired timezone
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                    swr.initialDateTime = convertedDateTime;


                    swr.eventIdCalendar = eventId;
                    _context.scheduledWorkRequests.Add(swr);
                    await _context.SaveChangesAsync();
                    getswrId = swr.swrAutoId;
                    Console.WriteLine(getswrId);
                    return Ok();

                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            


            //_context.scheduledWorkRequests.Add(scheduledWorkRequest);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetScheduledWorkRequest", new { id = scheduledWorkRequest.swrAutoId }, scheduledWorkRequest);
        }

        // DELETE: api/ScheduledWorkRequests/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteScheduledWorkRequest(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var calInfo = await _context.googleCalendarRecords.Where(x => x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (calInfo == null)
                    {
                        return Unauthorized();
                    }

                    var scheduledWorkRequest = await _context.scheduledWorkRequests.FindAsync(id);
                    if (scheduledWorkRequest == null)
                    {
                        return NotFound();
                    }

                    _context.scheduledWorkRequests.Remove(scheduledWorkRequest);
                    await _context.SaveChangesAsync();

                    GoogleCalendar gc = new GoogleCalendar();
                    gc.DeleteEvent(scheduledWorkRequest.eventIdCalendar, calInfo.googleCalendarId);
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

        private bool ScheduledWorkRequestExists(int id)
        {
            return _context.scheduledWorkRequests.Any(e => e.swrAutoId == id);
        }
        private bool GoogleCalendarApiExists(int id)
        {
            return _context.googleCalendarRecords.Any(e => e.googleCalendarAutoId == id);
        }
    }
}
