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
            return await _context.userAndRoles.ToListAsync();
        }

        // GET: api/RoleandUsers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleandUser>> GetRoleandUser(string id)
        {
            var roleandUser = await _context.userAndRoles.FindAsync(id);

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
            if (id != roleandUser.roleUserId)
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
            _context.userAndRoles.Add(roleandUser);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleandUserExists(roleandUser.roleUserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoleandUser", new { id = roleandUser.roleUserId }, roleandUser);
        }

        // DELETE: api/RoleandUsers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleandUser(string id)
        {
            var roleAndUser = await _context.userAndRoles.FindAsync(id);
            if (roleAndUser == null)
            {
                return NotFound();
            }

            _context.userAndRoles.Remove(roleAndUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleandUserExists(int id)
        {
            return _context.userAndRoles.Any(e => e.roleUserId == id);
        }
    }
}
