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
        public async Task<ActionResult<IEnumerable<Methodtype>>> Getmethodtypes(string cId)
        {
            try
            {
                return await _context.methodTypes.Where(x => x.companyId == cId).ToListAsync();
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
                var methodType = await _context.methodTypes.FindAsync(id);

                if (methodType == null)
                {
                    return NotFound();
                }

                return methodType;
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
                if (id != methodtype.mtAutoId)
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
        public async Task<ActionResult<Methodtype>> PostMethodtype(Methodtype methodType)
        {
            try
            {
                var compId = _context.methodTypes.Where(d => d.companyId == methodType.companyId).Select(d => d.mtId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Methodtype m = new Methodtype();
                    string comId = "M1";
                    m.mtId = comId;
                    m.mtName = methodType.mtName;
                    m.companyId = methodType.companyId;
                    m.status = methodType.status;
                    _context.methodTypes.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Methodtype m = new Methodtype();
                    string comId = "M" + (int.Parse(autoId) + 1);
                    m.mtId = comId;
                    m.mtName = methodType.mtName;
                    m.companyId = methodType.companyId;
                    m.status = methodType.status;
                    _context.methodTypes.Add(m);
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
                var methodType = await _context.methodTypes.FindAsync(id);
                if (methodType == null)
                {
                    return NotFound();
                }

                _context.methodTypes.Remove(methodType);
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
            return _context.methodTypes.Any(e => e.mtAutoId == id);
        }
    }
}
