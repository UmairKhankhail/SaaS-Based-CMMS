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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string id)
        {
            try
            {
                return await _context.users.Where(x => x.companyId == id).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/addusers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id, int uId)
        {
            try
            {
                var getRoleId = await _context.users
                   .Join(_context.userAndRoles, d => d.userAutoId, sd => sd.userAutoId, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyId == id && x.d.companyId == id && x.d.userAutoId == uId)
                   .Select(result => new
                   {
                       result.sd.userAutoId,
                       result.sd.roleAutoId,
                       result.d.employeeAutoId,
                       result.d.deptAutoId,
                       result.d.password,
                       result.d.status,
                       result.sd.companyId,
                   }).ToListAsync();

                return Ok(getRoleId);
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
        public async Task<IActionResult> PutUser(string id, User user)
        {
            try
            {
                if (UserExists(id, user.companyId) == true)
                {
                    var listRoles = user.listRoles;
                    var listDbRoles = new List<string>();
                    var getUserRoles = _context.userAndRoles.Where(x => x.userAutoId == user.userAutoId && user.companyId == user.companyId).Select(x => x.roleAutoId);
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
                            var delUser = _context.userAndRoles.Where(x => x.companyId == user.companyId && x.userAutoId == user.userAutoId && x.roleAutoId == int.Parse(item)).FirstOrDefault();
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
                            roleUser.companyId = user.companyId;
                            _context.userAndRoles.Add(roleUser);
                        }

                    }
                    await _context.SaveChangesAsync();

                    var cacheResponse=await _JwtTokenHandler.RevokingCachePermissionsAsync(new CacheChangeRequest { uAutoId=user.userAutoId, cId=user.companyId, uId=user.userId });
                    if (cacheResponse == true)
                        return Ok();
                    return BadRequest();

                }
                return NoContent();
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
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                int getUserAutoId = 0;
                var compId = _context.users.Where(d => d.companyId == user.companyId).Select(d => d.userId).ToList();

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
                    c.userName= user.userName;
                    c.password = user.password;
                    c.status = user.status;
                    c.companyId = user.companyId;
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
                    c.companyId = user.companyId;
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
                    roledept.companyId = user.companyId;
                    _context.userAndRoles.Add(roledept);
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

        // DELETE: api/addusers/5
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string cid, string uId)
        {
            try
            {
                var user = _context.users.Where(x => x.companyId == cid && x.userId == uId).FirstOrDefault();
                if (user == null)
                {
                    return NotFound();
                }

                _context.users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool UserExists(string id, string cId)
        {
            return _context.users.Any(x => x.userId == id && x.companyId ==cId);
        }
    }
}
