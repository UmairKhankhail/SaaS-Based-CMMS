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
                return await _context.users.Where(x => x.companyid == id).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/addusers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id, int uid)
        {
            try
            {
                var getroleid = await _context.users
                   .Join(_context.userandroles, d => d.userautoid, sd => sd.userautoid, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyid == id && x.d.companyid == id && x.d.userautoid == uid)
                   .Select(result => new
                   {
                       result.sd.userautoid,
                       result.sd.roleautoid,
                       result.d.employeeautoid,
                       result.d.deptautoid,
                       result.d.password,
                       result.d.status,
                       result.sd.companyid,
                   }).ToListAsync();

                return Ok(getroleid);
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
                if (UserExists(id, user.companyid) == true)
                {
                    var list_roles = user.list_roles;
                    var list_db_roles = new List<string>();
                    var getuserroles = _context.userandroles.Where(x => x.userautoid == user.userautoid && user.companyid == user.companyid).Select(x => x.roleautoid);
                    foreach (var x in getuserroles)
                    {
                        list_db_roles.Add(x.ToString());
                        Console.WriteLine("DB Roles: " + x);
                    }
                    foreach (var x in list_roles)
                    {
                        Console.WriteLine("New Roles: " + x);

                    }

                    var resultlistleft = list_db_roles.Except(list_roles).ToList();
                    var resultlistright = list_roles.Except(list_db_roles).ToList();
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
                            var deluser = _context.userandroles.Where(x => x.companyid == user.companyid && x.userautoid == user.userautoid && x.roleautoid == int.Parse(item)).FirstOrDefault();
                            if (deluser == null)
                            {
                                return NotFound();
                            }

                            _context.userandroles.Remove(deluser);
                        }
                    }
                    if (rlr != null)
                    {
                        foreach (var item in rlr)
                        {
                            RoleandUser roleuser = new RoleandUser();
                            roleuser.userautoid = user.userautoid;
                            roleuser.roleautoid = int.Parse(item);
                            roleuser.companyid = user.companyid;
                            _context.userandroles.Add(roleuser);
                        }

                    }
                    await _context.SaveChangesAsync();

                    var cacheresponse=await _JwtTokenHandler.RevokingCachePermissionsAsync(new CacheChangeRequest { uAutoId=user.userautoid, cId=user.companyid, uId=user.userid });
                    if (cacheresponse == true)
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
                int getuserautoid = 0;
                var compid = _context.users.Where(d => d.companyid == user.companyid).Select(d => d.userid).ToList();

                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    User c = new User();
                    string comid = "U1";
                    c.userid = comid;
                    c.username = user.username;
                    c.password = user.password;
                    c.status = user.status;
                    c.companyid = user.companyid;
                    c.role = user.role;
                    c.employeeautoid = user.employeeautoid;
                    c.deptautoid = user.deptautoid;
                    _context.users.Add(c);
                    await _context.SaveChangesAsync();
                    getuserautoid = c.userautoid;
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    User c = new User();
                    string comid = "U" + (int.Parse(autoid) + 1);
                    c.userid = comid;
                    c.username = user.username;
                    c.password = user.password;
                    c.status = user.status;
                    c.companyid = user.companyid;
                    c.role = user.role;
                    c.employeeautoid = user.employeeautoid;
                    c.deptautoid = user.deptautoid;
                    _context.users.Add(c);
                    await _context.SaveChangesAsync();
                    getuserautoid = c.userautoid;
                }

                var new_list_roles = user.list_roles;

                foreach (var items in new_list_roles)
                {
                    RoleandUser roledept = new RoleandUser();
                    roledept.roleautoid = int.Parse(items);
                    roledept.userautoid = getuserautoid;
                    roledept.companyid = user.companyid;
                    _context.userandroles.Add(roledept);
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
        public async Task<IActionResult> DeleteUser(string cid, string uid)
        {
            try
            {
                var user = _context.users.Where(x => x.companyid == cid && x.userid == uid).FirstOrDefault();
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

        private bool UserExists(string id, string cid)
        {
            return _context.users.Any(x => x.userid == id && x.companyid==cid);
        }
    }
}
