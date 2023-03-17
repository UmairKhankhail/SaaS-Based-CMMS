using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Drawing;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionallocationsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FunctionallocationsController> _logger;

        public FunctionallocationsController(UserDbContext context, ILogger<FunctionallocationsController> logger)
        {
            _context = context;
            _logger = logger;   
        }

        // GET: api/Functionallocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Functionallocation>>> Getfunctionallocations(string cid)
        {
            try
            {
                return await _context.functionalLocations.Where(x => x.companyId == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Functionallocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Functionallocation>> GetFunctionallocation(int id)
        {
            try
            {
                var functionallocation = await _context.functionalLocations.FindAsync(id);

                if (functionallocation == null)
                {
                    return NotFound();
                }

                return functionallocation;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Functionallocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFunctionallocation(int id, Functionallocation functionallocation)
        {
            try
            {
                if (id != functionallocation.flAutoId)
                {
                    return BadRequest();
                }

                _context.Entry(functionallocation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Functionallocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Functionallocation>> PostFunctionallocation(Functionallocation functionallocation)
        {
            try
            {
                var faflfunctionalloc = _context.functionalLocations.Where(d => d.companyId == functionallocation.companyId).Select(f => f.flId).ToList();
                List<string> fllist = new List<string>();
                List<int> flnolist = new List<int>();
                foreach (var z in faflfunctionalloc)
                {
                    if (z.Contains(functionallocation.floorSingleId + "L"))
                    {
                        fllist.Add(z);
                    }
                }
                if (fllist.Count > 0)
                {
                    foreach (var x in fllist)
                    {
                        int startIndex = x.IndexOf('L') + 1;
                        int endIndex = x.IndexOf('D');
                        string result = x.Substring(startIndex, endIndex - startIndex);
                        flnolist.Add(int.Parse(result));
                    }
                }

                if (flnolist.Count == 0)
                {
                    _context.ChangeTracker.Clear();
                    Functionallocation f = new Functionallocation();
                    f.flId = functionallocation.floorSingleId + "L1" + functionallocation.subDeptSingleId;
                    f.flName = functionallocation.flName;
                    f.companyId = functionallocation.companyId;
                    f.status = "Active";
                    f.facilityAutoId = functionallocation.facilityAutoId;
                    f.floorAutoId = functionallocation.floorAutoId;
                    f.subDeptAutoId = functionallocation.subDeptAutoId;
                    _context.functionalLocations.Add(f);
                    await _context.SaveChangesAsync();
                }
                if (flnolist.Count > 0)
                {
                    _context.ChangeTracker.Clear();
                    Functionallocation f = new Functionallocation();
                    string comid = functionallocation.floorSingleId + "L" + (flnolist.Max() + 1) + functionallocation.subDeptSingleId;
                    f.flId = comid;
                    f.flName = functionallocation.flName;
                    f.companyId = functionallocation.companyId;
                    f.status = "Active";
                    f.facilityAutoId = functionallocation.facilityAutoId;
                    f.floorAutoId = functionallocation.floorAutoId;
                    f.subDeptAutoId = functionallocation.subDeptAutoId;
                    _context.functionalLocations.Add(f);
                    await _context.SaveChangesAsync();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Functionallocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFunctionallocation(int id)
        {
            try
            {

                var functionallocation = await _context.functionalLocations.FindAsync(id);
                if (functionallocation == null)
                {
                    return NotFound();
                }

                _context.functionalLocations.Remove(functionallocation);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool FunctionallocationExists(int id)
        {
            return _context.functionalLocations.Any(e => e.flAutoId == id);
        }
    }
}
