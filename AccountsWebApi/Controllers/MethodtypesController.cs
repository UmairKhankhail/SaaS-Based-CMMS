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
    public class MethodtypesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<MethodtypesController> _logger;

        public MethodtypesController(UserDbContext context, ILogger<MethodtypesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Methodtypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Methodtype>>> Getmethodtypes(string cid)
        {
            try
            {
                return await _context.methodtypes.Where(x => x.companyid == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Methodtypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Methodtype>> GetMethodtype(int id)
        {
            try
            {
                var methodtype = await _context.methodtypes.FindAsync(id);

                if (methodtype == null)
                {
                    return NotFound();
                }

                return methodtype;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Methodtypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMethodtype(int id, Methodtype methodtype)
        {
            try
            {
                if (id != methodtype.mtautoid)
                {
                    return BadRequest();
                }

                _context.Entry(methodtype).State = EntityState.Modified;

                
                await _context.SaveChangesAsync();
                

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Methodtypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Methodtype>> PostMethodtype(Methodtype methodtype)
        {
            try
            {
                var compid = _context.methodtypes.Where(d => d.companyid == methodtype.companyid).Select(d => d.mtid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Methodtype m = new Methodtype();
                    string comid = "M1";
                    m.mtid = comid;
                    m.mtname = methodtype.mtname;
                    m.companyid = methodtype.companyid;
                    m.status = methodtype.status;
                    _context.methodtypes.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Methodtype m = new Methodtype();
                    string comid = "M" + (int.Parse(autoid) + 1);
                    m.mtid = comid;
                    m.mtname = methodtype.mtname;
                    m.companyid = methodtype.companyid;
                    m.status = methodtype.status;
                    _context.methodtypes.Add(m);
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

        // DELETE: api/Methodtypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodtype(int id)
        {
            try
            {
                var methodtype = await _context.methodtypes.FindAsync(id);
                if (methodtype == null)
                {
                    return NotFound();
                }

                _context.methodtypes.Remove(methodtype);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool MethodtypeExists(int id)
        {
            return _context.methodtypes.Any(e => e.mtautoid == id);
        }
    }
}
