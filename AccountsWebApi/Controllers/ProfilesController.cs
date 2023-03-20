using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ProfilesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public ProfilesController(UserDbContext context, ILogger<ProfilesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Profiles
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Profile>>> Getprofiles(string cid)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.profiles.Where(x => x.companyId == cid && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Profiles/5
        [HttpGet("getProfile")]
        [Authorize]
        public async Task<ActionResult<Profile>> GetProfile(string id, int pid)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var getRoleId = await _context.profiles
                   .Join(_context.userAndProfiles, d => d.profileAutoId, sd => sd.profileAutoId, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyId == id && x.d.companyId == id && x.d.profileAutoId == pid)
                   .Select(result => new
                   {
                       result.sd.userAutoId,
                       result.sd.profileAutoId,
                       result.d.profileId,
                       result.d.profileName,
                       result.d.status,
                       result.d.companyId
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
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProfile(int id, Profile profile)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var listUserName = profile.listUserName;
                    var listDbUsername = new List<string>();
                    var getProfileUser = _context.userAndProfiles.Where(x => x.profileAutoId == profile.profileAutoId && profile.companyId == profile.companyId).Select(x => x.userAutoId);
                    foreach (var x in getProfileUser)
                    {
                        listDbUsername.Add(x.ToString());
                        Console.WriteLine("DB Usernames: " + x);
                    }
                    foreach (var x in listUserName)
                    {
                        Console.WriteLine("New Usernames: " + x);

                    }

                    var resultListLeft = listDbUsername.Except(listUserName).ToList();
                    var resultListRight = listUserName.Except(listDbUsername).ToList();
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
                            var delUser = _context.userAndProfiles.Where(x => x.companyId == profile.companyId && x.profileAutoId == profile.profileAutoId && x.userAutoId == int.Parse(item)).FirstOrDefault();
                            if (delUser == null)
                            {
                                return NotFound();
                            }

                            _context.userAndProfiles.Remove(delUser);
                        }
                    }
                    if (rlr != null)
                    {
                        foreach (var item in rlr)
                        {
                            Userandprofile up = new Userandprofile();
                            up.profileAutoId = profile.profileAutoId;
                            up.userAutoId = int.Parse(item);
                            up.companyId = profile.companyId;
                            _context.userAndProfiles.Add(up);
                        }

                    }
                    await _context.SaveChangesAsync();
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

        // POST: api/Profiles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    int getprofileautoid = 0;
                    var compId = _context.profiles.Where(d => d.companyId == profile.companyId).Select(d => d.profileId).ToList();

                    var autoid = "";
                    if (compId.Count > 0)
                    {
                        autoid = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoid == "")
                    {
                        _context.ChangeTracker.Clear();
                        Profile p = new Profile();
                        string comid = "PF1";
                        p.profileId = comid;
                        p.profileName = profile.profileName;
                        p.status = profile.status;
                        p.companyId = profile.companyId;
                        _context.profiles.Add(p);
                        await _context.SaveChangesAsync();
                        getprofileautoid = p.profileAutoId;
                    }
                    if (autoid != "")
                    {
                        _context.ChangeTracker.Clear();
                        Profile p = new Profile();
                        string comid = "PF" + (int.Parse(autoid) + 1);
                        p.profileId = comid;
                        p.profileName = profile.profileName;
                        p.status = profile.status;
                        p.companyId = profile.companyId;
                        _context.profiles.Add(p);
                        await _context.SaveChangesAsync();
                        getprofileautoid = p.profileAutoId;
                    }

                    var newListProfiles = profile.listUserName;

                    foreach (var items in newListProfiles)
                    {
                        Userandprofile up = new Userandprofile();
                        up.userAutoId = int.Parse(items);
                        up.profileAutoId = getprofileautoid;
                        up.companyId = profile.companyId;
                        _context.userAndProfiles.Add(up);
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

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var profile = await _context.profiles.FindAsync(id);
                    if (profile == null)
                    {
                        return NotFound();
                    }

                    _context.profiles.Remove(profile);
                    await _context.SaveChangesAsync();

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

        private bool ProfileExists(int id)
        {
            return _context.profiles.Any(e => e.profileAutoId == id);
        }
    }
}
