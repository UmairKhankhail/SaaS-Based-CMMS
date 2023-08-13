using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarService;
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PreventiveMaintenanceWebApi.Models;

namespace PreventiveMaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingsController : ControllerBase
    {
        private readonly PreventiveMaintenanceDbContext _context;
        private readonly ILogger<MeterReadingsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public MeterReadingsController(PreventiveMaintenanceDbContext context, ILogger<MeterReadingsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/MeterReadings
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MeterReading>>> GetmeterReadings()
        //{
        //    return await _context.meterReadings.ToListAsync();
        //}

        [HttpGet("GetMeterReadingsByAssetId")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MeterReading>>> GetmeterReadingsByAssetId(int assetModel, string assetId)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReading = _context.meterReadings.Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.companyId == claimresponse.companyId).ToList();
                    if (meterReading == null)
                    {
                        return Unauthorized();
                    }
                    return meterReading;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }
        [HttpGet("GetMeterReadingsByAssetIdForParameter")]
        [Authorize]
        public async Task<ActionResult<MeterReading>> GetmeterReadingsByAssetIdByParams(int assetModel, string assetId, string paramName)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReading = _context.meterReadings.Where(x => x.assetModelId == assetModel && x.assetId == assetId && x.paramName == paramName && x.companyId == claimresponse.companyId).FirstOrDefault();
                    if (meterReading == null)
                    {
                        return Unauthorized();
                    }
                    return meterReading;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            
        }



        // GET: api/MeterReadings/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MeterReading>> GetMeterReading(int id)
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReading = await _context.meterReadings.Where(x=>x.companyId==claimresponse.companyId && x.mrAutoId==id).FirstOrDefaultAsync();

                    if (meterReading == null)
                    {
                        return NotFound();
                    }

                    return meterReading;
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
        public async Task<ActionResult<IEnumerable<MeterReading>>> GetAllMeterReading()
        {
            try
            {

                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var meterReading = await _context.meterReadings.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                    if (meterReading == null)
                    {
                        return NotFound();
                    }

                    return meterReading;
                }
                return Unauthorized();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/MeterReadings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMeterReading(MeterReading meterReading)
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

                    var existingModel = await _context.meterReadings.FindAsync(meterReading.mrAutoId);

                    existingModel.initialDate = meterReading.initialDate;
                    existingModel.minValue = meterReading.minValue;
                    existingModel.maxValue = meterReading.maxValue;
                    existingModel.frequencyDays = meterReading.frequencyDays;

                    try
                    {
                        GoogleCalendar googleCalendar = new GoogleCalendar();

                        //DateTime conversion to users timezone
                        var startDateTime = existingModel.initialDate;
                        var endDateTime = startDateTime.AddDays(existingModel.frequencyDays);
                        //Conversion Ends

                        Event newEvent = new Event
                        {
                            Summary = "Recurrent Meter Reading",
                            Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                            End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                            Description = existingModel.paramName.ToString() + "/n" + existingModel.assetId + "/n" + "Updated"
                        };

                        googleCalendar.UpdateRecurringEvent(calInfo.googleCalendarId, existingModel.eventIdCalendar, newEvent, existingModel.frequencyDays, calInfo.timeZoneRegional);

                        //Db DateTime Work to exact same format
                        DateTime inputDateTime = meterReading.initialDate;
                        string timeZoneId = calInfo.timeZoneWord;
                        // Convert parsed datetime to universal datetime
                        DateTime universalDateTime = inputDateTime.ToUniversalTime();

                        // Convert universal datetime to desired timezone
                        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                        DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                        existingModel.initialDate = convertedDateTime;
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

        // POST: api/MeterReadings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<MeterReading>> PostMeterReading(MeterReading meterReading)
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

                    MeterReading mr = new MeterReading();
                    mr.assetModelId = meterReading.assetModelId;
                    mr.assetId = meterReading.assetId;
                    mr.paramType = meterReading.paramType;
                    mr.paramName = meterReading.paramName;
                    mr.minValue = meterReading.minValue;
                    mr.maxValue = meterReading.maxValue;
                    mr.frequencyDays = meterReading.frequencyDays;
                    mr.initialDate = meterReading.initialDate;
                    mr.companyId = claimresponse.companyId;


                    GoogleCalendar googleCalendar = new GoogleCalendar();

                    //DateTime conversion to users timezone
                    var startDateTime = mr.initialDate;
                    var endDateTime = startDateTime.AddDays(mr.frequencyDays);
                    //Conversion Ends

                    Event newEvent = new Event
                    {
                        Summary = "Recurrent Meter Reading",
                        Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                        End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = calInfo.timeZoneRegional },
                        Description = mr.paramName.ToString() + "/n" + mr.assetId
                    };


                    Console.WriteLine(newEvent);
                    var eventId = googleCalendar.InsertRecurringEvent(newEvent, mr.frequencyDays, calInfo.googleCalendarId, calInfo.timeZoneRegional);
                    Console.WriteLine(eventId);

                    //Db DateTime Work to exact same format
                    DateTime inputDateTime = meterReading.initialDate;
                    string timeZoneId = calInfo.timeZoneWord;
                    // Convert parsed datetime to universal datetime
                    DateTime universalDateTime = inputDateTime.ToUniversalTime();

                    // Convert universal datetime to desired timezone
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    DateTime convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(universalDateTime, timeZone);
                    mr.initialDate = convertedDateTime;


                    mr.eventIdCalendar = eventId;
                    _context.meterReadings.Add(mr);
                    await _context.SaveChangesAsync();
                    getswrId = mr.mrAutoId;
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
            


            //_context.meterReadings.Add(meterReading);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMeterReading", new { id = meterReading.mrAutoId }, meterReading);
        }

        // DELETE: api/MeterReadings/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMeterReading(int id)
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

                    var meterReading = await _context.meterReadings.FindAsync(id);
                    if (meterReading == null)
                    {
                        return NotFound();
                    }

                    _context.meterReadings.Remove(meterReading);
                    await _context.SaveChangesAsync();

                    GoogleCalendar gc = new GoogleCalendar();
                    gc.DeleteEvent(meterReading.eventIdCalendar, calInfo.googleCalendarId);
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

        private bool MeterReadingExists(int id)
        {
            return _context.meterReadings.Any(e => e.mrAutoId == id);
        }
    }
}
