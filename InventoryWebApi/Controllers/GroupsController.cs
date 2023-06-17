using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using JwtAuthenticationManager.Models;
namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(InventoryDbContext context, HttpClient httpClient, ILogger<GroupsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> Getgroups()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.Inventorygroups.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var group = await _context.Inventorygroups.Where(x => x.groupAutoID == id && x.companyId == claimresponse.companyId && x.status == "Active").FirstOrDefaultAsync();

                    if (group == null)
                    {
                        return NotFound();
                    }

                    return group;
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup([FromBody] Group group,int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (GroupExists(id) && CompanyExists(claimresponse.companyId))
                    {
                        Group g = new Group();
                        g.groupAutoID = group.groupAutoID;
                        g.groupID = group.groupID;
                        g.groupName = group.groupName;
                        g.status = group.status;
                        g.companyId = claimresponse.companyId;
                        _context.Entry(g).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
                        return Ok();
                    }

                    return NotFound();
                }
                return Unauthorized();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var comId = _context.Inventorygroups.Where(g => g.companyId == claimresponse.companyId).Select(g => g.groupID).ToList();

                    var autoId = "";
                    if (comId.Count > 0)
                    {
                        autoId = comId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Group g = new Group();
                        string comid = "G1";
                        g.groupID = comid;
                        g.groupName = group.groupName;
                        g.status = group.status;
                        g.companyId = claimresponse.companyId;
                        _context.Inventorygroups.Add(g);
                        await _context.SaveChangesAsync();
                    }

                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Group g = new Group();
                        string comid = "G" + (int.Parse(autoId) + 1);
                        g.groupID = comid;
                        g.groupName = group.groupName;
                        g.status = group.status;
                        g.companyId = claimresponse.companyId;
                        _context.Inventorygroups.Add(g);
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
        }





        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var @group = await _context.Inventorygroups.Where(x => x.groupAutoID == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();
                    if (@group == null)
                    {
                        return NotFound();
                    }

                    _context.Inventorygroups.Remove(@group);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool GroupExists(int id)
        {
            return _context.Inventorygroups.Any(e => e.groupAutoID == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.Inventorygroups.Any(x => x.companyId == id);
        }
    }
}
