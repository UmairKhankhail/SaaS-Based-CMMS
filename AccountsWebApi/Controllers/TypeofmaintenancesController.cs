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
                return await _context.typeOfMaintenances.Where(x => x.companyId == cid).ToListAsync();
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
                var typeOfMaintenance = await _context.typeOfMaintenances.FindAsync(id);

                if (typeOfMaintenance == null)
                {
                    return NotFound();
                }

                return typeOfMaintenance;
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
        public async Task<IActionResult> PutTypeofmaintenance(int id, Typeofmaintenance typeOfMaintenance)
        {
            try
            {
                if (id != typeOfMaintenance.tomAutoId)
                {
                    return BadRequest();
                }

                _context.Entry(typeOfMaintenance).State = EntityState.Modified;
                await _context.SaveChangesAsync();


                return Ok();
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
        public async Task<ActionResult<Typeofmaintenance>> PostTypeofmaintenance(Typeofmaintenance typeOfMaintenance)
        {
            try
            {
                var compId = _context.typeOfMaintenances.Where(d => d.companyId == typeOfMaintenance.companyId).Select(d => d.tomId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Typeofmaintenance m = new Typeofmaintenance();
                    string comId = "TM1";
                    m.tomId = comId;
                    m.tomName = typeOfMaintenance.tomName;
                    m.companyId = typeOfMaintenance.companyId;
                    m.status = typeOfMaintenance.status;
                    _context.typeOfMaintenances.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Typeofmaintenance m = new Typeofmaintenance();
                    string comId = "TM" + (int.Parse(autoId) + 1);
                    m.tomId = comId;
                    m.tomName = typeOfMaintenance.tomName;
                    m.companyId = typeOfMaintenance.companyId;
                    m.status = typeOfMaintenance.status;
                    _context.typeOfMaintenances.Add(m);
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
                var typeOfMaintenance = await _context.typeOfMaintenances.FindAsync(id);
                if (typeOfMaintenance == null)
                {
                    return NotFound();
                }

                _context.typeOfMaintenances.Remove(typeOfMaintenance);
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
            return _context.typeOfMaintenances.Any(e => e.tomAutoId == id);
        }
    }
}
