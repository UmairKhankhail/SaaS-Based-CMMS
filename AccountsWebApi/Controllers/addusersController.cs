using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.Data;
using System.Security.Cryptography;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Filters;
using JwtAuthenticationManager;
using System.Security.Cryptography.Xml;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddusersController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<AddusersController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public AddusersController(UserDbContext context, ILogger<AddusersController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/addusers
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.users
                    .Where(x => x.companyId == claimresponse.companyId && x.status=="Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/addusers/5
        [HttpGet("getUser")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int uId)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getRoleId = await _context.users
                   .Join(_context.userAndRoles, u => u.userAutoId, ur => ur.userAutoId, (u, ur) => new { User = u, UserAndRoles = ur })
                   .Join(_context.roles, ur => ur.UserAndRoles.roleAutoId, r => r.roleAutoId, (ur, r) => new { User = ur.User, Role = r })
                   .Where(x => x.User.companyId == claimresponse.companyId && x.User.companyId == claimresponse.companyId && x.User.userAutoId == uId)
                   .Select(result => new
                   {
                       result.User.userAutoId,
                       result.Role.roleAutoId,
                       result.Role.roleName, // Include the role name from the Roles table
                       result.User.employeeAutoId,
                       result.User.deptAutoId,
                       result.User.password,
                       result.User.status,
                       result.User.companyId
                   })
                   .ToListAsync();

                    return Ok(getRoleId);
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //var user = _context.users.Where(x => x.companyid == id && x.userid == uid).FirstOrDefault();

            //if (user == null)
            //{
            //    return NotFound();
            //}

            //return user;

        }

        // PUT: api/addusers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser(string id, User user)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (UserExists(user.userAutoId))
                    {
                        var listRoles = user.listRoles;
                        var listDbRoles = new List<string>();
                        var getUserRoles = _context.userAndRoles.Where(x => x.userAutoId == user.userAutoId && user.companyId == claimresponse.companyId).Select(x => x.roleAutoId);
                        foreach (var x in getUserRoles)
                        {
                            listDbRoles.Add(x.ToString());
                            Console.WriteLine("DB Roles: " + x);
                        }
                        foreach (var x in listRoles)
                        {
                            Console.WriteLine("New Roles: " + x);

                        }

                        var resultListLeft = listDbRoles.Except(listRoles).ToList();
                        var resultListRight = listRoles.Except(listDbRoles).ToList();
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
                                var delUser = _context.userAndRoles.Where(x => x.companyId == claimresponse.companyId && x.userAutoId == user.userAutoId && x.roleAutoId == int.Parse(item)).FirstOrDefault();
                                if (delUser == null)
                                {
                                    return NotFound();
                                }

                                _context.userAndRoles.Remove(delUser);
                            }
                        }
                        if (rlr != null)
                        {
                            foreach (var item in rlr)
                            {
                                RoleandUser roleUser = new RoleandUser();
                                roleUser.userAutoId = user.userAutoId;
                                roleUser.roleAutoId = int.Parse(item);
                                roleUser.companyId = claimresponse.companyId;
                                _context.userAndRoles.Add(roleUser);
                            }

                        }
                        await _context.SaveChangesAsync();

                        var cacheResponse = await _JwtTokenHandler.RevokingCachePermissionsAsync(new CacheChangeRequest { uAutoId = user.userAutoId, cId = claimresponse.companyId, uId = user.userId });
                        if (cacheResponse == true)
                            return Ok();
                        return BadRequest();

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

        // POST: api/addusers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    int getUserAutoId = 0;
                    var compId = _context.users.Where(d => d.companyId == claimresponse.companyId).Select(d => d.userId).ToList();

                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        User c = new User();
                        string comId = "U1";
                        c.userId = comId;
                        c.userName = user.userName;
                        c.password = user.password;
                        c.status = user.status;
                        c.companyId = claimresponse.companyId;
                        c.role = user.role;
                        c.employeeAutoId = user.employeeAutoId;
                        c.deptAutoId = user.deptAutoId;
                        _context.users.Add(c);
                        await _context.SaveChangesAsync();
                        getUserAutoId = c.userAutoId;
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        User c = new User();
                        string comId = "U" + (int.Parse(autoId) + 1);
                        c.userId = comId;
                        c.userName = user.userName;
                        c.password = user.password;
                        c.status = user.status;
                        c.companyId = claimresponse.companyId;
                        c.role = user.role;
                        c.employeeAutoId = user.employeeAutoId;
                        c.deptAutoId = user.deptAutoId;
                        _context.users.Add(c);
                        await _context.SaveChangesAsync();
                        getUserAutoId = c.userAutoId;
                    }

                    var newListRoles = user.listRoles;

                    foreach (var items in newListRoles)
                    {
                        RoleandUser roledept = new RoleandUser();
                        roledept.roleAutoId = int.Parse(items);
                        roledept.userAutoId = getUserAutoId;
                        roledept.companyId = claimresponse.companyId;
                        _context.userAndRoles.Add(roledept);
                        await _context.SaveChangesAsync();
                    }

                    return Ok();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // DELETE: api/addusers/5
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (UserExists(id))
                    {
                        var user = await _context.users.FindAsync(id);
                        if (user == null)
                        {
                            return NotFound();
                        }

                        _context.users.Remove(user);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    return NotFound();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool UserExists(int id)
        {
            return _context.users.Any(x => x.userAutoId == id);
        }
    }
}
