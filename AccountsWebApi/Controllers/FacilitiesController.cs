using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Cryptography;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilitiesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FacilitiesController> _logger;
        public FacilitiesController(UserDbContext context, ILogger<FacilitiesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Facilities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facility>>> Getfacilities(string cid)
        {
            try
            {
                return await _context.facilities.Where(x => x.companyid == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Facilities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facility>> GetFacility(int id)
        {
            try
            {
                var facility = await _context.facilities.FindAsync(id);

                if (facility == null)
                {
                    return NotFound();
                }

                return facility;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Facilities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacility(int id, Facility facility)
        {
            try
            {
                if (id != facility.facilityautoid)
                {
                    return BadRequest();
                }

                _context.Entry(facility).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(id))
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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // POST: api/Facilities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Facility>> PostFacility(Facility facility)
        {
            try
            {
                var compid = _context.facilities.Where(d => d.companyid == facility.companyid).Select(d => d.facilityid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Facility f = new Facility();
                    string comid = "B1";
                    f.facilityid = comid;
                    f.facilityname = facility.facilityname;
                    f.companyid = facility.companyid;
                    f.status = facility.status;
                    _context.facilities.Add(f);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Facility f = new Facility();
                    string comid = "B" + (int.Parse(autoid) + 1);
                    f.facilityid = comid;
                    f.facilityname = facility.facilityname;
                    f.companyid = facility.companyid;
                    f.status = facility.status;
                    _context.facilities.Add(f);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Facilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility(int id)
        {
            try
            {
                var facility = await _context.facilities.FindAsync(id);
                if (facility == null)
                {
                    return NotFound();
                }

                _context.facilities.Remove(facility);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool FacilityExists(int id)
        {
            return _context.facilities.Any(e => e.facilityautoid == id);
        }
    }
}
