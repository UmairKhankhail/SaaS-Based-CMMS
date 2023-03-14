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
    public class ProfilesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<ProfilesController> _logger;

        public ProfilesController(UserDbContext context, ILogger<ProfilesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Profile>>> Getprofiles(string cid)
        {
            try
            {
                return await _context.profiles.Where(x => x.companyid == cid).ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Profile>> GetProfile(string id, int pid)
        {
            try
            {
                var getroleid = await _context.profiles
                   .Join(_context.userandprofiles, d => d.profileautoid, sd => sd.profileautoid, (d, sd) => new { d, sd })
                   .Where(x => x.sd.companyid == id && x.d.companyid == id && x.d.profileautoid == pid)
                   .Select(result => new
                   {
                       result.sd.userautoid,
                       result.sd.profileautoid,
                       result.d.profileid,
                       result.d.profilename,
                       result.d.status,
                       result.d.companyid
                   }).ToListAsync();

                return Ok(getroleid);
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
        public async Task<IActionResult> PutProfile(int id, Profile profile)
        {
            try
            {

                var list_username = profile.list_username;
                var list_db_username = new List<string>();
                var getprofileuser = _context.userandprofiles.Where(x => x.profileautoid == profile.profileautoid && profile.companyid == profile.companyid).Select(x => x.userautoid);
                foreach (var x in getprofileuser)
                {
                    list_db_username.Add(x.ToString());
                    Console.WriteLine("DB Usernames: " + x);
                }
                foreach (var x in list_username)
                {
                    Console.WriteLine("New Usernames: " + x);

                }

                var resultlistleft = list_db_username.Except(list_username).ToList();
                var resultlistright = list_username.Except(list_db_username).ToList();
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
                        var deluser = _context.userandprofiles.Where(x => x.companyid == profile.companyid && x.profileautoid == profile.profileautoid && x.userautoid == int.Parse(item)).FirstOrDefault();
                        if (deluser == null)
                        {
                            return NotFound();
                        }

                        _context.userandprofiles.Remove(deluser);
                    }
                }
                if (rlr != null)
                {
                    foreach (var item in rlr)
                    {
                        Userandprofile up = new Userandprofile();
                        up.profileautoid = profile.profileautoid;
                        up.userautoid = int.Parse(item);
                        up.companyid = profile.companyid;
                        _context.userandprofiles.Add(up);
                    }

                }
                await _context.SaveChangesAsync();
                return Ok();
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
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
            try
            {
                int getprofileautoid = 0;
                var compid = _context.profiles.Where(d => d.companyid == profile.companyid).Select(d => d.profileid).ToList();

                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(2))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Profile p = new Profile();
                    string comid = "PF1";
                    p.profileid = comid;
                    p.profilename = profile.profilename;
                    p.status = profile.status;
                    p.companyid = profile.companyid;
                    _context.profiles.Add(p);
                    await _context.SaveChangesAsync();
                    getprofileautoid = p.profileautoid;
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Profile p = new Profile();
                    string comid = "PF" + (int.Parse(autoid) + 1);
                    p.profileid = comid;
                    p.profilename = profile.profilename;
                    p.status = profile.status;
                    p.companyid = profile.companyid;
                    _context.profiles.Add(p);
                    await _context.SaveChangesAsync();
                    getprofileautoid = p.profileautoid;
                }

                var new_list_profiles = profile.list_username;

                foreach (var items in new_list_profiles)
                {
                    Userandprofile up = new Userandprofile();
                    up.userautoid = int.Parse(items);
                    up.profileautoid = getprofileautoid;
                    up.companyid = profile.companyid;
                    _context.userandprofiles.Add(up);
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

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile(int id)
        {
            try
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
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool ProfileExists(int id)
        {
            return _context.profiles.Any(e => e.profileautoid == id);
        }
    }
}
