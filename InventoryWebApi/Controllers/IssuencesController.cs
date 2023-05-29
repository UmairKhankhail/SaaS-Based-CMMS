using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using AccountsWebApi.Models;
using StackExchange.Redis;
using System.ComponentModel.Design;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuencesController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EquipmentsController> _logger;

        public IssuencesController(InventoryDbContext context, HttpClient httpClient, ILogger<EquipmentsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Issuences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issuence>>> Getissuences(string companyId)
        {
            try
            {
                return await _context.issuences.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();
                            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Issuences/5
        [HttpGet("id")]
        public async Task<ActionResult<Issuence>> GetIssuence(string companyId, string issueId)
        {
            try
            {

                var getIssuenceRequests = await _context.issuences
                    .Join(_context.issuenceandEquipment, i => i.issuenceAutoId, ie => ie.issuenceAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == companyId && x.i.companyId == companyId && x.i.issuenceId == issueId)
                    .Select(result => new
                    {
                        result.i.issuenceAutoId,
                        result.i.issuenceId,
                        result.i.companyId,
                        result.ie.equipAutoId,
                        result.i.issuenceDescp,
                    }
                    ).ToListAsync();
                return Ok(getIssuenceRequests);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        // POST: api/Issuences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Issuence>> PostIssuence(int userAutoId,string companyId, Issuence issuence)
        {
            int getIssuenceId=0;
            var compId = _context.issuences.Where(i => i.companyId == issuence.companyId).Select(d => d.issuenceId).ToList();
            var autoId = "";
            if (compId.Count > 0)
            {

                autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                Issuence issue = new Issuence();
                string comId = "IR1";
                issue.issuenceAutoId = issuence.issuenceAutoId;
                issue.issuenceId = comId;
                issue.companyId = issuence.companyId;
                issue.status = issuence.status;
                issue.issuenceDescp = issuence.issuenceDescp;
                issue.qty = issuence.qty;
                issue.userAutoId = userAutoId;
                _context.issuences.Add(issue);
                await _context.SaveChangesAsync();
                getIssuenceId = issue.issuenceAutoId;
            }
            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                Issuence issue = new Issuence();
                string comId = "IR"+(int.Parse(autoId)+1);
                issue.issuenceAutoId = issuence.issuenceAutoId;
                issue.issuenceId = comId;
                issue.companyId = issuence.companyId;
                issue.status = issuence.status;
                issue.issuenceDescp = issuence.issuenceDescp;
                issue.qty = issuence.qty;
                issue.userAutoId = userAutoId;
                _context.issuences.Add(issue);
                await _context.SaveChangesAsync();
                getIssuenceId= issue.issuenceAutoId;
            }


            var listEquipments = issuence.equipList;
            foreach(var item in listEquipments)
            {
                IssuenceandEquipment issuenceandEquipment= new IssuenceandEquipment();
                issuenceandEquipment.equipAutoId = Convert.ToInt32(item);
                issuenceandEquipment.issuenceAutoId = getIssuenceId;
                issuenceandEquipment.companyId = issuence.companyId;
                _context.issuenceandEquipment.Add(issuenceandEquipment);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        //// PUT: api/Issuences
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssuence(string id, string companyId ,int userAutoId,Issuence issuence)
        {
            if (IssuenceExists(id) && CompanyExists(companyId))
            {
                Issuence issue = new Issuence();
                issue.issuenceAutoId = issuence.issuenceAutoId;
                issue.issuenceId = issuence.issuenceId;
                issue.companyId = issuence.companyId;
                issue.status = issuence.status;
                issue.qty = issuence.qty;
                issue.issuenceDescp = issuence.issuenceDescp;
                issue.userAutoId = userAutoId;

                var listEquipments = issuence.equipList;
                var listDbEquipments = new List<string>();
                var getEquipments = _context.issuenceandEquipment.Where(x => x.issuenceAutoId == issuence.issuenceAutoId && x.companyId == issuence.companyId).Select(x => x.equipAutoId);

                foreach (var item in getEquipments)
                {
                    listDbEquipments.Add(item.ToString());
                    Console.WriteLine("DB Equipments: " + item);
                }

                foreach (var equip in listEquipments)
                {
                    Console.WriteLine("New Equipments" + equip);

                }

                var resultListLeft = listDbEquipments.Except(listEquipments).ToList();
                var resultListRight = listEquipments.Except(listDbEquipments).ToList();

                var rll = new List<string>();
                var rlr = new List<string>();
                if (resultListLeft != null)
                {
                    foreach (var x in resultListLeft)
                    {
                        rll.Add(x);
                    }
                }
                if (resultListRight != null)
                {
                    foreach (var x in resultListRight)
                    {
                        rlr.Add(x);
                    }
                }

                if (rll != null)
                {
                    foreach (var item in rll)
                    {
                        var delUser = _context.issuenceandEquipment.Where(x => x.companyId == companyId && x.issuenceAutoId == issuence.issuenceAutoId && x.equipAutoId == int.Parse(item)).FirstOrDefault();
                        if (delUser == null)
                        {
                            return NotFound();
                        }
                        _context.issuenceandEquipment.Remove(delUser);
                    }
                }

                if (rlr != null)
                {
                    foreach (var item in rlr)
                    {
                        IssuenceandEquipment issuenceEquipment = new IssuenceandEquipment();
                        issuenceEquipment.issuenceAutoId = issuence.issuenceAutoId;
                        issuenceEquipment.equipAutoId = int.Parse(item);
                        issuenceEquipment.companyId = issuence.companyId;
                        _context.issuenceandEquipment.Add(issuenceEquipment);
                    }
                }


                _context.Entry(issue).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }


        // DELETE: api/Issuences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssuence(string id,string companyId)
        {
            var issuence = await _context.issuences.Where(x => x.issuenceId== id && x.companyId == companyId).FirstOrDefaultAsync();

            if (issuence == null)
            {
                return NotFound();
            }

            _context.issuences.Remove(issuence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssuenceExists(string id)
        {
            return _context.issuences.Any(e => e.issuenceId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.issuences.Any(x => x.companyId == id);
        }
    }
}
