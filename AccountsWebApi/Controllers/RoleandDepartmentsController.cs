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
    public class RoleandDepartmentsController : ControllerBase
    {
        private readonly UserDbContext _context;

        public RoleandDepartmentsController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/RoleandDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleandDepartment>>> Getroleanddepartments()
        {
            return await _context.roleanddepartments.ToListAsync();
        }

        // GET: api/RoleandDepartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleandDepartment>> GetRoleandDepartment(string id)
        {
            var roleandDepartment = await _context.roleanddepartments.FindAsync(id);

            if (roleandDepartment == null)
            {
                return NotFound();
            }

            return roleandDepartment;
        }

        // PUT: api/RoleandDepartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoleandDepartment(int id, RoleandDepartment roleandDepartment)
        {
            if (id != roleandDepartment.roledeptid)
            {
                return BadRequest();
            }

            _context.Entry(roleandDepartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleandDepartmentExists(id))
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

        // POST: api/RoleandDepartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RoleandDepartment>> PostRoleandDepartment(RoleandDepartment roleandDepartment)
        {
            _context.roleanddepartments.Add(roleandDepartment);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RoleandDepartmentExists(roleandDepartment.roledeptid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRoleandDepartment", new { id = roleandDepartment.roledeptid }, roleandDepartment);
        }

        // DELETE: api/RoleandDepartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleandDepartment(string id)
        {
            var roleandDepartment = await _context.roleanddepartments.FindAsync(id);
            if (roleandDepartment == null)
            {
                return NotFound();
            }

            _context.roleanddepartments.Remove(roleandDepartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoleandDepartmentExists(int id)
        {
            return _context.roleanddepartments.Any(e => e.roledeptid == id);
        }
    }
}
