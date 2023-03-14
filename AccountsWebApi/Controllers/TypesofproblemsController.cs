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
    public class TypesofproblemsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<TypesofproblemsController> _logger;
        public TypesofproblemsController(UserDbContext context, ILogger<TypesofproblemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Typesofproblems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Typesofproblem>>> Gettypesofproblems(string cid)
        {
            try
            {
                return await _context.typesofproblems.Where(x => x.companyid == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Typesofproblems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Typesofproblem>> GetTypesofproblem(int id)
        {
            try
            {
                var typesofproblem = await _context.typesofproblems.FindAsync(id);

                if (typesofproblem == null)
                {
                    return NotFound();
                }

                return typesofproblem;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Typesofproblems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypesofproblem(int id, Typesofproblem typesofproblem)
        {
            try
            {
                if (id != typesofproblem.topautoid)
                {
                    return BadRequest();
                }

                _context.Entry(typesofproblem).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Typesofproblems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Typesofproblem>> PostTypesofproblem(Typesofproblem typesofproblem)
        {
            try
            {
                var compid = _context.typesofproblems.Where(d => d.companyid == typesofproblem.companyid).Select(d => d.topid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Typesofproblem m = new Typesofproblem();
                    string comid = "TP1";
                    m.topid = comid;
                    m.topname = typesofproblem.topname;
                    m.companyid = typesofproblem.companyid;
                    m.status = typesofproblem.status;
                    _context.typesofproblems.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Typesofproblem m = new Typesofproblem();
                    string comid = "TP" + (int.Parse(autoid) + 1);
                    m.topid = comid;
                    m.topname = typesofproblem.topname;
                    m.companyid = typesofproblem.companyid;
                    m.status = typesofproblem.status;
                    _context.typesofproblems.Add(m);
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

        // DELETE: api/Typesofproblems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypesofproblem(int id)
        {
            try
            {
                var typesofproblem = await _context.typesofproblems.FindAsync(id);
                if (typesofproblem == null)
                {
                    return NotFound();
                }

                _context.typesofproblems.Remove(typesofproblem);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool TypesofproblemExists(int id)
        {
            return _context.typesofproblems.Any(e => e.topautoid == id);
        }
    }
}
