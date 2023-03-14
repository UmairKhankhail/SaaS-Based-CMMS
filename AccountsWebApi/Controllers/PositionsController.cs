using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<PositionsController> _logger;

        public PositionsController(UserDbContext context, ILogger<PositionsController> logger)
        {
            _context = context;
            _logger = logger;   
        }

        // GET: api/Positions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Position>>> Getpositions(string cid)
        {
            try
            {
                return await _context.positions.Where(x => x.companyid == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Positions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPosition(int id)
        {
            try
            {
                var position = await _context.positions.FindAsync(id);

                if (position == null)
                {
                    return NotFound();
                }

                return position;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Positions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPosition(int id, Position position)
        {
            try
            {
                if (id != position.positionautoid)
                {
                    return BadRequest();
                }

                _context.Entry(position).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PositionExists(id))
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

        // POST: api/Positions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Position>> PostPosition(Position position)
        {
            try
            {
                var compid = _context.positions.Where(d => d.companyid == position.companyid).Select(d => d.positionid).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Position p = new Position();
                    string comid = "P1";
                    p.positionid = comid;
                    p.positionname = position.positionname;
                    p.companyid = position.companyid;
                    p.status = position.status;
                    _context.positions.Add(p);
                    await _context.SaveChangesAsync();
                    //return Ok(c);
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Position p = new Position();
                    string comid = "P" + (int.Parse(autoid) + 1);
                    p.positionid = comid;
                    p.positionname = position.positionname;
                    p.companyid = position.companyid;
                    p.status = position.status;
                    _context.positions.Add(p);
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

        // DELETE: api/Positions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosition(int id)
        {
            try
            {
                var position = await _context.positions.FindAsync(id);
                if (position == null)
                {
                    return NotFound();
                }

                _context.positions.Remove(position);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool PositionExists(int id)
        {
            return _context.positions.Any(e => e.positionautoid == id);
        }
    }
}
