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
    public class RoleandPermissionsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public RoleandPermissionsController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/RoleandPermissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleandPermission>>> Getroleandpermissions()
        {
            return await _context.roleandpermissions.ToListAsync();
        }

        // GET: api/RoleandPermissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleandPermission>> GetRoleandPermission(string id)
        {
            var roleandPermission = await _context.roleandpermissions.FindAsync(id);

            if (roleandPermission == null)
            {
                return NotFound();
            }

            return roleandPermission;
        }

        // PUT: api/RoleandPermissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleandPermission(int id, RoleandPermission roleandPermission)
        {
            if (id != roleandPermission.rolepermissionid)
            {
                return BadRequest();
            }

            _context.Entry(roleandPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleandPermissionExists(id))
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

        // POST: api/RoleandPermissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleandPermission>> PostRoleandPermission(RoleandPermission roleandPermission)
        {
            _context.roleandpermissions.Add(roleandPermission);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleandPermissionExists(roleandPermission.rolepermissionid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoleandPermission", new { id = roleandPermission.rolepermissionid }, roleandPermission);
        }

        // DELETE: api/RoleandPermissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleandPermission(string id)
        {
            var roleandPermission = await _context.roleandpermissions.FindAsync(id);
            if (roleandPermission == null)
            {
                return NotFound();
            }

            _context.roleandpermissions.Remove(roleandPermission);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleandPermissionExists(int id)
        {
            return _context.roleandpermissions.Any(e => e.rolepermissionid == id);
        }
    }
}
