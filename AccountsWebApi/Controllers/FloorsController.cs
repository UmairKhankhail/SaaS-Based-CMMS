using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using JwtAuthenticationManager.Models;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<FloorsController> _logger;

        public FloorsController(UserDbContext context, ILogger<FloorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Floors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Floor>>> Getfloors(string cid)
        {
            try
            {
                return await _context.floors.Where(x => x.companyId == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Floors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Floor>> GetFloor(int id)
        {
            try
            {
                var floor = await _context.floors.FindAsync(id);

                if (floor == null)
                {
                    return NotFound();
                }

                return floor;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Floors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFloor(int id, Floor floor)
        {
            try
            {
                if (id != floor.floorAutoId)
                {
                    return BadRequest();
                }

                _context.Entry(floor).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Floors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Floor>> PostFloor(Floor floor)
        {
            try
            {
                var facilityfloors = _context.floors.Where(d => d.companyId == floor.companyId).Select(f => f.floorId).ToList();
                List<string> floorlist = new List<string>();
                List<int> floornolist = new List<int>();
                foreach (var z in facilityfloors)
                {
                    if (z.Contains(floor.facilitySingleId + "F"))
                    {
                        floorlist.Add(z);
                    }
                }
                if (floorlist.Count > 0)
                {
                    foreach (var x in floorlist)
                    {
                        floornolist.Add(int.Parse(x.Split("F").Last().ToString()));
                    }
                }

                if (floornolist.Count == 0)
                {
                    _context.ChangeTracker.Clear();
                    Floor f = new Floor();
                    f.floorId = floor.facilitySingleId + "F1";
                    f.floorName = floor.floorName;
                    f.companyId = floor.companyId;
                    f.status = "Active";
                    f.facilityAutoId = floor.facilityAutoId;
                    _context.floors.Add(f);
                    await _context.SaveChangesAsync();
                }
                if (floornolist.Count > 0)
                {
                    _context.ChangeTracker.Clear();
                    Floor f = new Floor();
                    string comid = floor.facilitySingleId + "F" + (floornolist.Max() + 1);
                    f.floorId = comid;
                    f.floorName = floor.floorName;
                    f.companyId = floor.companyId;
                    f.status = "Active";
                    f.facilityAutoId = floor.facilityAutoId;
                    _context.floors.Add(f);
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

        // DELETE: api/Floors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            try
            {
                var floor = await _context.floors.FindAsync(id);
                if (floor == null)
                {
                    return NotFound();
                }

                _context.floors.Remove(floor);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool FloorExists(int id)
        {
            return _context.floors.Any(e => e.floorAutoId == id);
        }
    }
}
