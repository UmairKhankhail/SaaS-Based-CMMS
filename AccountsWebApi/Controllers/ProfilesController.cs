﻿using System;
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
                return await _context.profiles.Where(x => x.companyId == cid).ToListAsync();
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

                var list_username = profile.listUsername ;
                var list_db_username = new List<string>();
                var getprofileuser = _context.userAndProfiles.Where(x => x.profileAutoId == profile.profileAutoId && profile.companyId == profile.companyId).Select(x => x.userAutoId);
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
                        var deluser = _context.userAndProfiles.Where(x => x.companyId == profile.companyId && x.profileAutoId == profile.profileAutoId && x.userAutoId == int.Parse(item)).FirstOrDefault();
                        if (deluser == null)
                        {
                            return NotFound();
                        }

                        _context.userAndProfiles.Remove(deluser);
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

                var new_list_profiles = profile.listUsername;

                foreach (var items in new_list_profiles)
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
            return _context.profiles.Any(e => e.profileAutoId == id);
        }
    }
}
