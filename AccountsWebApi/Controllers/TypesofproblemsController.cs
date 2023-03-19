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
                return await _context.typesOfProblems.Where(x => x.companyId == cid).ToListAsync();
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
                var typesOfProblem = await _context.typesOfProblems.FindAsync(id);

                if (typesOfProblem == null)
                {
                    return NotFound();
                }

                return typesOfProblem;
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
                if (id != typesofproblem.topAutoId)
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
        public async Task<ActionResult<Typesofproblem>> PostTypesofproblem(Typesofproblem typesOfProblem)
        {
            try
            {
                var compId = _context.typesOfProblems.Where(d => d.companyId == typesOfProblem.companyId).Select(d => d.topId).ToList();
                var autoId = "";
                if (compId.Count > 0)
                {
                    autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Typesofproblem m = new Typesofproblem();
                    string comid = "TP1";
                    m.topId = comid;
                    m.topName = typesOfProblem.topName;
                    m.companyId = typesOfProblem.companyId;
                    m.status = typesOfProblem.status;
                    _context.typesOfProblems.Add(m);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Typesofproblem m = new Typesofproblem();
                    string comid = "TP" + (int.Parse(autoId) + 1);
                    m.topId = comid;
                    m.topName = typesOfProblem.topName;
                    m.companyId = typesOfProblem.companyId;
                    m.status = typesOfProblem.status;
                    _context.typesOfProblems.Add(m);
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
                var typesOfProblem = await _context.typesOfProblems.FindAsync(id);
                if (typesOfProblem == null)
                {
                    return NotFound();
                }

                _context.typesOfProblems.Remove(typesOfProblem);
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
            return _context.typesOfProblems.Any(e => e.topAutoId == id);
        }
    }
}
