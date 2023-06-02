using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using System.ComponentModel.Design;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthAndSafetiesController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public HealthAndSafetiesController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/HealthAndSafeties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthAndSafety>>> GethealthAndSafeties(string companyId)
        {
            var getHandS = await _context.healthAndSafeties.
                Join(_context.HealthAndSafetyItems, hs => hs.hsAutoId, hsi => hsi.hsAutoId, (hs, hsi) => new { hs, hsi })
                .Where(x => x.hs.companyId == companyId && x.hsi.companyId == companyId)
                .Select(result => new
                {
                    result.hs.hsAutoId,
                    result.hs.woAutoId,
                    result.hs.userName,
                    result.hs.companyId,
                    result.hsi.phsAutoId,

                }).ToListAsync();

            return Ok(getHandS);
        }

        // GET: api/HealthAndSafeties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthAndSafety>> GetHealthAndSafety(int id,string companyId)
        {
            var getHandS = await _context.healthAndSafeties.
                   Join(_context.HealthAndSafetyItems, hs => hs.hsAutoId, hsi => hsi.hsAutoId, (hs, hsi) => new { hs, hsi })
                   .Where(x => x.hs.companyId == companyId && x.hsi.companyId == companyId && x.hs.hsAutoId==id)
                   .Select(result => new
                   {
                       result.hs.hsAutoId,
                       result.hs.woAutoId,
                       result.hs.userName,
                       result.hs.companyId,
                       result.hsi.phsAutoId,

                   }).ToListAsync();

            if (getHandS == null)
            {
                return NotFound();
            }

            return Ok(getHandS);
        }

        // PUT: api/HealthAndSafeties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutHealthAndSafety(int id,string companyId ,HealthAndSafety healthAndSafety)
        {
            if (healthAndSafety.hsAutoId==id && healthAndSafety.companyId==companyId)
            {
                _context.ChangeTracker.Clear();
                HealthAndSafety hs = new HealthAndSafety();
                hs.hsAutoId = healthAndSafety.hsAutoId;
                hs.woAutoId = healthAndSafety.woAutoId;
                hs.userName = healthAndSafety.userName;
                hs.companyId = healthAndSafety.companyId;
                hs.remarks = healthAndSafety.remarks;


                var listChecks = healthAndSafety.hsCheckList;
                var listDbChecks = new List<string>();
                var getChecks = _context.HealthAndSafetyItems.Where(x =>x.hsAutoId == healthAndSafety.hsAutoId && x.companyId == healthAndSafety.companyId).Select(x => x.hsiAutoId);

                foreach (var item in getChecks)
                {
                    listDbChecks.Add(item.ToString());
                    Console.WriteLine("DB Equipments: " + item);
                }

                foreach (var equip in listChecks)
                {
                    Console.WriteLine("New Equipments" + equip);

                }

                var resultListLeft = listDbChecks.Except(listChecks).ToList();
                var resultListRight = listChecks.Except(listDbChecks).ToList();

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
                        var delUser = _context.HealthAndSafetyItems.Where(x => x.companyId == companyId && x.hsAutoId == healthAndSafety.hsAutoId && x.hsiAutoId == int.Parse(item)).FirstOrDefault();
                        if (delUser == null)
                        {
                            return NotFound();
                        }
                        _context.HealthAndSafetyItems.Remove(delUser);
                    }
                }

                if (rlr != null)
                {
                    foreach (var item in rlr)
                    {
                        HealthAndSafetyItems hsitems = new HealthAndSafetyItems();
                        hsitems.phsAutoId = int.Parse(item.ToString());
                        hsitems.hsAutoId = healthAndSafety.hsAutoId;
                        hsitems.companyId = healthAndSafety.companyId;
                        hsitems.woAutoId = healthAndSafety.woAutoId;
                        _context.HealthAndSafetyItems.Add(hsitems);
                        await _context.SaveChangesAsync();
                    }
                }


                _context.Entry(hs).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();
        }

        // POST: api/HealthAndSafeties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HealthAndSafety>> PostHealthAndSafety(HealthAndSafety healthAndSafety)
        {
            var getHealthAndSafetyId = 0;
            
            _context.ChangeTracker.Clear();
            HealthAndSafety hs = new HealthAndSafety();
            hs.hsAutoId= healthAndSafety.hsAutoId;
            hs.woAutoId = healthAndSafety.woAutoId;
            hs.userName = healthAndSafety.userName;
            hs.companyId = healthAndSafety.companyId;
            hs.remarks = healthAndSafety.remarks;

            _context.healthAndSafeties.Add(hs);
            await _context.SaveChangesAsync();
            getHealthAndSafetyId = hs.hsAutoId; 

            var listHSItems = healthAndSafety.hsCheckList;
            foreach (var item in listHSItems)
            {
                HealthAndSafetyItems hsitems = new HealthAndSafetyItems();
                hsitems.phsAutoId = int.Parse(item.ToString());
                hsitems.hsAutoId = getHealthAndSafetyId;
                hsitems.companyId = healthAndSafety.companyId;
                hsitems.woAutoId = healthAndSafety.woAutoId;
                _context.HealthAndSafetyItems.Add(hsitems);
                await _context.SaveChangesAsync();
            }
           
            return Ok();
        }

        // DELETE: api/HealthAndSafeties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthAndSafety(int id)
        {
            var healthAndSafety = await _context.healthAndSafeties.FindAsync(id);
            if (healthAndSafety == null)
            {
                return NotFound();
            }

            _context.healthAndSafeties.Remove(healthAndSafety);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthAndSafetyExists(int id)
        {
            return _context.healthAndSafeties.Any(e => e.hsAutoId == id);
        }
    }
}
