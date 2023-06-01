using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using System.Drawing.Imaging;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MethodsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public MethodsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/Methods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Method>>> Getmethods(string companyId)
        {
            return await _context.methods.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/Methods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Method>> GetMethod(int id, string companyId)
        {
            var @method = await _context.methods.Where(x=>x.mtAutoId==id && x.companyId==companyId).ToListAsync();

            if (@method == null)
            {
                return NotFound();
            }

            return Ok(@method);
        }

        // PUT: api/Methods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutMethod(int id,string companyId, Method @method)
        {
            if (@method.mtAutoId==id && @method.companyId==companyId)
            {

                _context.Entry(@method).State = EntityState.Modified;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MethodExists(id))
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

        // POST: api/Methods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Method>> PostMethod(Method @method)
        {
            _context.methods.Add(@method);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Methods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethod(int id)
        {
            var @method = await _context.methods.FindAsync(id);
            if (@method == null)
            {
                return NotFound();
            }

            _context.methods.Remove(@method);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MethodExists(int id)
        {
            return _context.methods.Any(e => e.mtAutoId == id);
        }
    }
}
