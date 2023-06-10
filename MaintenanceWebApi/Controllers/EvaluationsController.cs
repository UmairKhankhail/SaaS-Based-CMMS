using System;
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

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;
        private readonly HttpClient _httpClient;

        public EvaluationsController(MaintenanceDbContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        // GET: api/Evaluations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evaluation>>> Getevaluations(string companyId)
        {
            return await _context.evaluations.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/Evaluations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evaluation>> GetEvaluation(int id,string companyId)
        {
            var evaluation = await _context.evaluations.Where(x=>x.evaluationAutoId==id && x.companyId==companyId).ToListAsync();

            if (evaluation == null)
            {
                return NotFound();
            }

            return Ok(evaluation);
        }

        // PUT: api/Evaluations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEvaluation(int id,string companyId, Evaluation evaluation)
        {
            if (evaluation.evaluationAutoId==id && evaluation.companyId==companyId)
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

        // POST: api/Evaluations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Evaluation>> PostEvaluation(Evaluation evaluation)
        {
            var url = $"http://localhost:5182/api/ScheduledWorkRequests/GetCalendarRecords?cId={evaluation.companyId}";
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

            JObject jsonResponse = JObject.Parse(responseContent);

            var googleCalendarId = (string)jsonResponse["googleCalendarId"];
            var timeZoneRegional = (string)jsonResponse["timeZoneRegional"];
            var timeZoneWord = (string)jsonResponse["timeZoneWord"];

            Console.WriteLine(googleCalendarId);
            Console.WriteLine(timeZoneRegional);

            var compId = _context.evaluations.Where(eval => eval.companyId == evaluation.companyId && eval.woAutoId == evaluation.woAutoId).Select(d => d.evaluationId).ToList();
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
                eval.topName = evaluation.topName;
                eval.startTime = evaluation.startTime;
                eval.endTime = evaluation.endTime;
                eval.remarks = evaluation.remarks;
                eval.companyId = evaluation.companyId;

                


                GoogleCalendar googleCalendar = new GoogleCalendar();

                //DateTime conversion to users timezone
                var startDateTime = eval.startTime;
                var endDateTime = eval.endTime;
                //Conversion Ends

                Event newEvent = new Event
                {
                    Summary = "Work Order - Evaluation",
                    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                    Description = eval.userName.ToString() + " " + eval.topName
                };

                var eventId=googleCalendar.InsertEvent(newEvent, googleCalendarId, timeZoneRegional);

                //Db DateTime Work to exact same format
                DateTime inputDateTimeStart = eval.startTime;
                DateTime inputDateTimeEnd = eval.endTime;
                string timeZoneId = timeZoneWord;
                // Convert parsed datetime to universal datetime
                DateTime universalDateTimeStart = inputDateTimeStart.ToUniversalTime();
                DateTime universalDateTimeEnd = inputDateTimeEnd.ToUniversalTime();
                // Convert universal datetime to desired timezone
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime convertedDateTimeStart = TimeZoneInfo.ConvertTimeFromUtc(universalDateTimeStart, timeZone);
                DateTime convertedDateTimeEnd = TimeZoneInfo.ConvertTimeFromUtc(universalDateTimeEnd, timeZone);
                eval.startTime = convertedDateTimeStart;
                eval.endTime = convertedDateTimeEnd;
                eval.eventId = eventId;
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
                eval.remarks = evaluation.remarks;
                eval.companyId = evaluation.companyId;

                GoogleCalendar googleCalendar = new GoogleCalendar();

                //DateTime conversion to users timezone
                var startDateTime = eval.startTime;
                var endDateTime = eval.endTime;
                //Conversion Ends

                Event newEvent = new Event
                {
                    Summary = "Work Order - Evaluation",
                    Start = new EventDateTime { DateTime = startDateTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                    End = new EventDateTime { DateTime = endDateTime.ToUniversalTime(), TimeZone = timeZoneRegional },
                    Description = eval.userName.ToString() + " " + eval.topName
                };

                var eventId=googleCalendar.InsertEvent(newEvent, googleCalendarId, timeZoneRegional);

                //Db DateTime Work to exact same format
                DateTime inputDateTimeStart = eval.startTime;
                DateTime inputDateTimeEnd = eval.endTime;
                string timeZoneId = timeZoneWord;
                // Convert parsed datetime to universal datetime
                DateTime universalDateTimeStart = inputDateTimeStart.ToUniversalTime();
                DateTime universalDateTimeEnd = inputDateTimeEnd.ToUniversalTime();
                // Convert universal datetime to desired timezone
                TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime convertedDateTimeStart = TimeZoneInfo.ConvertTimeFromUtc(universalDateTimeStart, timeZone);
                DateTime convertedDateTimeEnd = TimeZoneInfo.ConvertTimeFromUtc(universalDateTimeEnd, timeZone);
                eval.startTime = convertedDateTimeStart;
                eval.endTime = convertedDateTimeEnd;
                eval.eventId = eventId;
                _context.evaluations.Add(eval);
                await _context.SaveChangesAsync();
            }

            return Ok();

        }



        // DELETE: api/Evaluations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluation(int id, string cId)
        {
            var url = $"http://localhost:5182/api/ScheduledWorkRequests/GetCalendarRecords?cId={cId}";
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseContent);

            JObject jsonResponse = JObject.Parse(responseContent);

            var googleCalendarId = (string)jsonResponse["googleCalendarId"];
            var timeZoneRegional = (string)jsonResponse["timeZoneRegional"];
            var timeZoneWord = (string)jsonResponse["timeZoneWord"];

            Console.WriteLine(googleCalendarId);
            Console.WriteLine(timeZoneRegional);


            var evaluation = await _context.evaluations.Where(x=>x.evaluationAutoId==id).FirstAsync();
            if (evaluation == null)
            {
                return NotFound();
            }
            Console.WriteLine(evaluation.eventId);
            
            _context.evaluations.Remove(evaluation);
            await _context.SaveChangesAsync();

            GoogleCalendar gc = new GoogleCalendar();
            gc.DeleteEvent(evaluation.eventId, googleCalendarId);

            return Ok();
        }

        private bool EvaluationExists(int id)
        {
            return _context.evaluations.Any(e => e.evaluationAutoId == id);
        }

    }
}
