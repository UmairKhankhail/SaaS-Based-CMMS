using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.Security.Cryptography;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeofmaintenancesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<TypeofmaintenancesController> _logger;
        public TypeofmaintenancesController(UserDbContext context, ILogger<TypeofmaintenancesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Typeofmaintenances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Typeofmaintenance>>> Gettypeofmaintenances(string cid)
        {
            try
            {
                return await _context.typeofmaintenances.Where(x => x.companyid == cid).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Typeofmaintenances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Typeofmaintenance>> GetTypeofmaintenance(int id)
        {
            try
            {
                var typeofmaintenance = await _context.typeofmaintenances.FindAsync(id);

                if (typeofmaintenance == null)
                {
                    return NotFound();
                }

                return typeofmaintenance;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Typeofmaintenances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeofmaintenance(int id, Typeofmaintenance typeofmaintenance)
        {
            try
            {
                if (id != typeofmaintenance.tomautoid)
                {
                    return BadRequest();
                }

                _context.Entry(typeofmaintenance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Typeofmaintenances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Typeofmaintenance>> PostTypeofmaintenance(Typeofmaintenance typeofmaintenance)
        {
            try
            {
                var compid = _context.typeofmaintenances.Where(d => d.companyid == typeofmaintenance.companyid).Select(d => d.tomid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Typeofmaintenance m = new Typeofmaintenance();
                    string comid = "TM1";
                    m.tomid = comid;
                    m.tomname = typeofmaintenance.tomname;
                    m.companyid = typeofmaintenance.companyid;
                    m.status = typeofmaintenance.status;
                    _context.typeofmaintenances.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Typeofmaintenance m = new Typeofmaintenance();
                    string comid = "TM" + (int.Parse(autoid) + 1);
                    m.tomid = comid;
                    m.tomname = typeofmaintenance.tomname;
                    m.companyid = typeofmaintenance.companyid;
                    m.status = typeofmaintenance.status;
                    _context.typeofmaintenances.Add(m);
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

        // DELETE: api/Typeofmaintenances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeofmaintenance(int id)
        {
            try
            {
                var typeofmaintenance = await _context.typeofmaintenances.FindAsync(id);
                if (typeofmaintenance == null)
                {
                    return NotFound();
                }

                _context.typeofmaintenances.Remove(typeofmaintenance);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool TypeofmaintenanceExists(int id)
        {
            return _context.typeofmaintenances.Any(e => e.tomautoid == id);
        }
    }
}
