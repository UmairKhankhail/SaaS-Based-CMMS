using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using NuGet.Protocol;
using System.Security.Cryptography;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<RolesController> _logger;
        public RolesController(UserDbContext context, ILogger<RolesController> logger)
        {
            _context = context;
            _logger = logger;   
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> Getroles(string id)
        {
            try
            {
                return await _context.roles.Where(x => x.companyid == id).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(string id, int rid)
        {
            try
            {
                var getroleid = await _context.roles
                   .Join(_context.roleandpermissions, d => d.roleautoid, sd => sd.roleautoid, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyid == id && x.d.companyid == id && x.d.roleautoid == rid)
                   .Select(result => new
                   {
                       result.sd.roleautoid,
                       result.sd.permissionid,
                       result.sd.rolepermissionid,
                       result.sd.companyid,
                   }).ToListAsync();

                return Ok(getroleid);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //var role = _context.roles.Where(x => x.companyid == id && x.roleid == rid).FirstOrDefault();

            //if (role == null)
            //{
            //    return NotFound();
            //}

            //return role;
        }


        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(string id, Role role)
        {
            try
            {
                if (RoleExists(id, role.companyid) == true)
                {
                    var list_permissions = role.list_permissions;
                    var list_db_permissions = new List<string>();
                    var getrolepermissions = _context.roleandpermissions.Where(x => x.roleautoid == role.roleautoid && x.companyid == role.companyid).Select(x => x.permissionid);
                    foreach (var x in getrolepermissions)
                    {
                        list_db_permissions.Add(x.ToString());
                        Console.WriteLine("DB Permisions: " + x);
                    }
                    foreach (var x in list_permissions)
                    {
                        Console.WriteLine("New Permissions: " + x);
                    }

                    var resultlistleft = list_db_permissions.Except(list_permissions).ToList();
                    var resultlistright = list_permissions.Except(list_db_permissions).ToList();
                    var rll = new List<string>();
                    var rlr = new List<string>();
                    if (resultlistleft != null)
                    {
                        foreach (var x in resultlistleft)
                        {
                            rll.Add(x);
                        }
                    }
                    if (resultlistright != null)
                    {
                        foreach (var x in resultlistright)
                        {
                            rlr.Add(x);
                        }
                    }
                    if (rll != null)
                    {
                        foreach (var item in rll)
                        {
                            var deluser = _context.roleandpermissions.Where(x => x.companyid == role.companyid && x.roleautoid == role.roleautoid && x.permissionid == item).FirstOrDefault();
                            if (deluser == null)
                            {
                                return NotFound();
                            }

                            _context.roleandpermissions.Remove(deluser);
                        }
                    }
                    if (rlr != null)
                    {
                        foreach (var item in rlr)
                        {
                            RoleandPermission rolepermissions = new RoleandPermission();
                            rolepermissions.roleautoid = role.roleautoid;
                            rolepermissions.permissionid = item;
                            rolepermissions.companyid = role.companyid;
                            _context.roleandpermissions.Add(rolepermissions);
                        }

                    }

                    var list_departments = role.list_Departments;
                    var list_db_departments = new List<string>();
                    var getroledepartments = _context.roleanddepartments.Where(x => x.roleautoid == role.roleautoid && x.companyid == role.companyid).Select(x => x.deptautoid);
                    foreach (var x in getroledepartments)
                    {
                        list_db_departments.Add(x.ToString());
                        Console.WriteLine("DB Departments: " + x);
                    }
                    foreach (var x in list_departments)
                    {
                        Console.WriteLine("New Departments: " + x);
                    }

                    var resultlistdeptleft = list_db_departments.Except(list_departments).ToList();
                    var resultlistdeptright = list_departments.Except(list_db_departments).ToList();
                    var rlld = new List<string>();
                    var rlrd = new List<string>();
                    if (resultlistdeptleft != null)
                    {
                        foreach (var x in resultlistdeptleft)
                        {
                            rlld.Add(x);
                        }
                    }
                    if (resultlistdeptright != null)
                    {
                        foreach (var x in resultlistdeptright)
                        {
                            rlrd.Add(x);
                        }
                    }
                    if (rlld != null)
                    {
                        foreach (var item in rlld)
                        {
                            var deluser = _context.roleanddepartments.Where(x => x.companyid == role.companyid && x.roleautoid == role.roleautoid && x.deptautoid == int.Parse(item)).FirstOrDefault();
                            if (deluser == null)
                            {
                                return NotFound();
                            }

                            _context.roleanddepartments.Remove(deluser);
                        }
                    }
                    if (rlrd != null)
                    {
                        foreach (var item in rlrd)
                        {
                            RoleandDepartment roledept = new RoleandDepartment();
                            roledept.roleautoid = role.roleautoid;
                            roledept.deptautoid = int.Parse(item);
                            roledept.companyid = role.companyid;
                            _context.roleanddepartments.Add(roledept);
                        }

                    }


                    await _context.SaveChangesAsync();

                }
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //if (id != role.roleid)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(role).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    throw;
            //    //if (!RoleExists(id))
            //    //{
            //    //    return NotFound();
            //    //}
            //    //else
            //    //{
            //    //    throw;
            //    //}
            //}

            //return NoContent();
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            try
            {
                int getroleautoid = 0;
                var compid = _context.roles.Where(d => d.companyid == role.companyid).Select(d => d.roleid).ToList();

                var autoid = "";
                if (compid.Count > 0)
                {

                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Role c = new Role();
                    string comid = "R1";
                    c.roleid = comid;
                    c.rolename = role.rolename;
                    c.companyid = role.companyid;
                    c.status = role.status;
                    _context.roles.Add(c);
                    await _context.SaveChangesAsync();
                    getroleautoid = c.roleautoid;
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Role c = new Role();
                    string comid = "R" + (int.Parse(autoid) + 1);
                    c.roleid = comid;
                    c.rolename = role.rolename;
                    c.companyid = role.companyid;
                    c.status = role.status;
                    _context.roles.Add(c);
                    await _context.SaveChangesAsync();
                    getroleautoid = c.roleautoid;
                }

                //Console.WriteLine("Id: "+ getroleautoid.ToString());
                //_context.roles.Add(role);
                //await _context.SaveChangesAsync();


                var list_subdepart = role.list_Departments;
                var list_permissions = role.list_permissions;

                foreach (var items in list_subdepart)
                {
                    RoleandDepartment roledept = new RoleandDepartment();
                    roledept.deptautoid = Convert.ToInt32(items);
                    roledept.roleautoid = getroleautoid;
                    roledept.companyid = role.companyid;
                    _context.roleanddepartments.Add(roledept);
                    await _context.SaveChangesAsync();
                }
                foreach (var items in list_permissions)
                {
                    RoleandPermission roleperm = new RoleandPermission();
                    roleperm.permissionid = items.ToString();
                    roleperm.roleautoid = getroleautoid;
                    roleperm.companyid = role.companyid;
                    _context.roleandpermissions.Add(roleperm);
                    await _context.SaveChangesAsync();
                }

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok(GetRole(getroleautoid));
            
            //_context.roles.Add(role);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (RoleExists(role.roleid))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}


        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id, string rid)
        {
            try
            {
                var role = _context.roles.Where(x => x.companyid == id && x.roleid == rid).FirstOrDefault();
                if (role == null)
                {
                    return NotFound();
                }

                _context.roles.Remove(role);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool RoleExists(string id, string cid)
        {
            return _context.roles.Any(x => x.roleid == id && x.companyid==cid);
        }
    }
}
