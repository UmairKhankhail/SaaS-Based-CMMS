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
using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<RolesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public RolesController(UserDbContext context, ILogger<RolesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Roles
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Role>>> Getroles()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.roles.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Roles/5
        [HttpGet("getRole")]
        [Authorize]
        public async Task<ActionResult<Role>> GetRole(int rId)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getRoleId = await _context.roles
                   .Join(_context.roleAndPermissions, d => d.roleAutoId, sd => sd.roleAutoId, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyId == claimresponse.companyId && x.d.companyId == claimresponse.companyId && x.d.roleAutoId == rId)
                   .Select(result => new
                   {
                       result.sd.roleAutoId,
                       result.sd.permissionId,
                       result.sd.rolePermissionId,
                       result.sd.companyId,
                   }).ToListAsync();

                    return Ok(getRoleId);
                }
                return Unauthorized();
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
        [Authorize]
        public async Task<IActionResult> PutRole(Role role)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (RoleExists(role.roleAutoId))
                    {
                        var listPermissions = role.listPermissions;
                        var listDbPermissions = new List<string>();
                        var getRolePermissions = _context.roleAndPermissions.Where(x => x.roleAutoId == role.roleAutoId && x.companyId == claimresponse.companyId).Select(x => x.permissionId);
                        foreach (var x in getRolePermissions)
                        {
                            listDbPermissions.Add(x.ToString());
                            Console.WriteLine("DB Permisions: " + x);
                        }
                        foreach (var x in listPermissions)
                        {
                            Console.WriteLine("New Permissions: " + x);
                        }

                        var resultListLeft = listDbPermissions.Except(listPermissions).ToList();
                        var resultListRight = listPermissions.Except(listDbPermissions).ToList();
                        var rll = new List<string>();
                        var rlr = new List<string>();
                        if (resultListLeft != null)
                        {
                            foreach (var x in resultListLeft)
                            {
                                rll.Add(x);
                            }
                        }
                        if (resultListRight != null)
                        {
                            foreach (var x in resultListRight)
                            {
                                rlr.Add(x);
                            }
                        }
                        if (rll != null)
                        {
                            foreach (var item in rll)
                            {
                                var delUser = _context.roleAndPermissions.Where(x => x.companyId == claimresponse.companyId && x.roleAutoId == role.roleAutoId && x.permissionId == item).FirstOrDefault();
                                if (delUser == null)
                                {
                                    return NotFound();
                                }

                                _context.roleAndPermissions.Remove(delUser);
                            }
                        }
                        if (rlr != null)
                        {
                            foreach (var item in rlr)
                            {
                                RoleandPermission rolePermissions = new RoleandPermission();
                                rolePermissions.roleAutoId = role.roleAutoId;
                                rolePermissions.permissionId = item;
                                rolePermissions.companyId = claimresponse.companyId;
                                _context.roleAndPermissions.Add(rolePermissions);
                            }

                        }

                        var listDepartments = role.listDepartments;
                        var listDbDepartments = new List<string>();
                        var getRoleDepartments = _context.roleAndDepartments.Where(x => x.roleAutoId == role.roleAutoId && x.companyId == claimresponse.companyId).Select(x => x.deptAutoId);
                        foreach (var x in getRoleDepartments)
                        {
                            listDbDepartments.Add(x.ToString());
                            Console.WriteLine("DB Departments: " + x);
                        }
                        foreach (var x in listDepartments)
                        {
                            Console.WriteLine("New Departments: " + x);
                        }

                        var resultListDeptLeft = listDbDepartments.Except(listDepartments).ToList();
                        var resultListDeptRight = listDepartments.Except(listDbDepartments).ToList();
                        var rlld = new List<string>();
                        var rlrd = new List<string>();
                        if (resultListDeptLeft != null)
                        {
                            foreach (var x in resultListDeptLeft)
                            {
                                rlld.Add(x);
                            }
                        }
                        if (resultListDeptRight != null)
                        {
                            foreach (var x in resultListDeptRight)
                            {
                                rlrd.Add(x);
                            }
                        }
                        if (rlld != null)
                        {
                            foreach (var item in rlld)
                            {
                                var delUser = _context.roleAndDepartments.Where(x => x.companyId == claimresponse.companyId && x.roleAutoId == role.roleAutoId && x.deptAutoId == int.Parse(item)).FirstOrDefault();
                                if (delUser == null)
                                {
                                    return NotFound();
                                }

                                _context.roleAndDepartments.Remove(delUser);
                            }
                        }
                        if (rlrd != null)
                        {
                            foreach (var item in rlrd)
                            {
                                RoleandDepartment roleDept = new RoleandDepartment();
                                roleDept.roleAutoId = role.roleAutoId;
                                roleDept.deptAutoId = int.Parse(item);
                                roleDept.companyId = claimresponse.companyId;
                                _context.roleAndDepartments.Add(roleDept);
                            }

                        }


                        await _context.SaveChangesAsync();

                        var getRoleUsersList = _context.userAndRoles.Where(x => x.roleAutoId == role.roleAutoId && x.companyId == claimresponse.companyId).Select(x => x.userAutoId).ToList();

                        var resultDestroyingUsers = _JwtTokenHandler.DestroyingCacheByAdminAsync(new DestroyCacheRequest { userAutoIds = getRoleUsersList });
                        if (resultDestroyingUsers == true)
                            return Ok();
                        return NotFound();

                    }
                    return NoContent();
                
                }
                return Unauthorized();
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
        [Authorize]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    
                        int getRoleAutoId = 0;
                        var compId = _context.roles.Where(d => d.companyId == claimresponse.companyId).Select(d => d.roleId).ToList();

                        var autoId = "";
                        if (compId.Count > 0)
                        {

                            autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                        }

                        if (autoId == "")
                        {
                            _context.ChangeTracker.Clear();
                            Role c = new Role();
                            string comId = "R1";
                            c.roleId = comId;
                            c.roleName = role.roleName;
                            c.companyId = claimresponse.companyId;
                            c.status = role.status;
                            _context.roles.Add(c);
                            await _context.SaveChangesAsync();
                            getRoleAutoId = c.roleAutoId;
                        }
                        if (autoId != "")
                        {
                            _context.ChangeTracker.Clear();
                            Role c = new Role();
                            string comId = "R" + (int.Parse(autoId) + 1);
                            c.roleId = comId;
                            c.roleName = role.roleName;
                            c.companyId = claimresponse.companyId;
                            c.status = role.status;
                            _context.roles.Add(c);
                            await _context.SaveChangesAsync();
                            getRoleAutoId = c.roleAutoId;
                        }

                        //Console.WriteLine("Id: "+ getroleautoid.ToString());
                        //_context.roles.Add(role);
                        //await _context.SaveChangesAsync();


                        var listSubdepart = role.listDepartments;
                        var listPermissions = role.listPermissions;

                        foreach (var items in listSubdepart)
                        {
                            RoleandDepartment roleDept = new RoleandDepartment();
                            roleDept.deptAutoId = Convert.ToInt32(items);
                            roleDept.roleAutoId = getRoleAutoId;
                            roleDept.companyId = claimresponse.companyId;
                            _context.roleAndDepartments.Add(roleDept);
                            await _context.SaveChangesAsync();
                        }
                        foreach (var items in listPermissions)
                        {
                            RoleandPermission rolePerm = new RoleandPermission();
                            rolePerm.permissionId = items.ToString();
                            rolePerm.roleAutoId = getRoleAutoId;
                            rolePerm.companyId = claimresponse.companyId;
                            _context.roleAndPermissions.Add(rolePerm);
                            await _context.SaveChangesAsync();
                        }

                        return Ok();
                    
                }
                return Unauthorized();

            }
            catch (Exception ex)
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
        [Authorize]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (RoleExists(id))
                    {
                        var role = await _context.roles.FindAsync(id);
                        if (role == null)
                        {
                            return NotFound();
                        }

                        _context.roles.Remove(role);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    return NoContent();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool RoleExists(int id)
        {
            return _context.roles.Any(x => x.roleAutoId == id);
        }
    }
}
