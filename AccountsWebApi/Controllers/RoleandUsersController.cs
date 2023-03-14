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
    public class RoleandUsersController : ControllerBase
    {
        private readonly UserDbContext _context;

        public RoleandUsersController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/RoleandUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleandUser>>> Getuserandroles()
        {
            return await _context.userandroles.ToListAsync();
        }

        // GET: api/RoleandUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleandUser>> GetRoleandUser(string id)
        {
            var roleandUser = await _context.userandroles.FindAsync(id);

            if (roleandUser == null)
            {
                return NotFound();
            }

            return roleandUser;
        }

        // PUT: api/RoleandUsers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleandUser(int id, RoleandUser roleandUser)
        {
            if (id != roleandUser.roleuserid)
            {
                return BadRequest();
            }

            _context.Entry(roleandUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleandUserExists(id))
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

        // POST: api/RoleandUsers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleandUser>> PostRoleandUser(RoleandUser roleandUser)
        {
            _context.userandroles.Add(roleandUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleandUserExists(roleandUser.roleuserid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoleandUser", new { id = roleandUser.roleuserid }, roleandUser);
        }

        // DELETE: api/RoleandUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleandUser(string id)
        {
            var roleandUser = await _context.userandroles.FindAsync(id);
            if (roleandUser == null)
            {
                return NotFound();
            }

            _context.userandroles.Remove(roleandUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleandUserExists(int id)
        {
            return _context.userandroles.Any(e => e.roleuserid == id);
        }
    }
}
