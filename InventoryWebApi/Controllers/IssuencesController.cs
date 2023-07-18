using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using StackExchange.Redis;
using System.ComponentModel.Design;
using InventoryWebApi.Models;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuencesController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<IssuencesController> _logger;

        public IssuencesController(InventoryDbContext context, HttpClient httpClient, ILogger<IssuencesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Issuences
        [HttpGet]
        public async Task<ActionResult<Issuence>> GetIssuences()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getIssuenceRequests = await _context.issuences
                    .Join(_context.issuenceandEquipment, i => i.issuenceAutoId, ie => ie.issuenceAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.i.companyId == claimresponse.companyId && x.ie.companyId == claimresponse.companyId)
                     .Select(result => new
                     {
                         result.i.issuenceAutoId,
                         result.i.issuenceId,
                         result.i.companyId,
                         result.ie.equipAutoId,
                         result.i.issuenceDescp,
                         result.ie.equipName,
                         result.ie.quantity,
                         result.i.status


                     }
                     ).ToListAsync();

                    return Ok(getIssuenceRequests);

                }

                else
                {
                    return Unauthorized(); 
                }
            }


            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Issuences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Issuence>> GetIssuence(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getIssuenceRequests = await _context.issuences
                    .Join(_context.issuenceandEquipment, i => i.issuenceAutoId, ie => ie.issuenceAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == claimresponse.companyId && x.i.companyId == claimresponse.companyId && x.i.issuenceAutoId == id)
                    .Select(result => new
                    {
                        result.i.issuenceAutoId,
                        result.i.issuenceId,
                        result.i.companyId,
                        result.ie.equipAutoId,
                        result.i.issuenceDescp,
                        result.ie.equipName,
                        result.ie.quantity,
                        result.i.status


                    }
                    ).ToListAsync();

                    return Ok(getIssuenceRequests);
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        // POST: api/Issuences
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Issuence>> PostIssuence( Issuence issuence)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    int getIssueAutoId = 0;

                        if (issuence.validityCheck == 0)
                        {

                            var compId = _context.issuences.Where(d => d.companyId == claimresponse.companyId).Select(d => d.issuenceId).ToList();

                            var autoId = "";
                            if (compId.Count > 0)
                            {

                                autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                            }

                            if (autoId == "")
                            {
                                _context.ChangeTracker.Clear();
                                Issuence issue = new Issuence();
                                string comId = "IS1";
                                issue.issuenceId = comId;
                                issue.issuenceAutoId = issuence.issuenceAutoId;
                                issue.companyId = issuence.companyId;
                                issue.status = issuence.status;
                                issue.issuenceDescp = issuence.issuenceDescp;
                                issue.userAutoId = issuence.userAutoId;
                                issue.companyId = claimresponse.companyId;
                                _context.issuences.Add(issue);
                                await _context.SaveChangesAsync();
                                getIssueAutoId = issue.issuenceAutoId;
                            }
                            if (autoId != "")
                            {
                                _context.ChangeTracker.Clear();
                                Issuence issue = new Issuence();
                                string comId = "IS" + (int.Parse(autoId) + 1);
                                issue.issuenceId = comId;
                                issue.issuenceAutoId = issuence.issuenceAutoId;
                                issue.companyId = issuence.companyId;
                                issue.status = issuence.status;
                                issue.issuenceDescp = issuence.issuenceDescp;
                                issue.userAutoId = issuence.userAutoId;
                                issue.companyId = claimresponse.companyId;
                                _context.issuences.Add(issue);
                                await _context.SaveChangesAsync();
                                getIssueAutoId = issue.issuenceAutoId;
                            }

                            //Console.WriteLine("Id: "+ getroleautoid.ToString());
                            //_context.roles.Add(role);
                            //await _context.SaveChangesAsync();

                            List<IssuenceList> issuenceLists = issuence.equipList;



                            foreach (var items in issuenceLists)
                            {
                                IssuenceandEquipment issuenceandEquipment = new IssuenceandEquipment();
                                issuenceandEquipment.equipName = items.equipName;
                                issuenceandEquipment.quantity = items.equipQuantity;
                                issuenceandEquipment.equipAutoId = items.equipAutoId;
                                issuenceandEquipment.issuenceAutoId = getIssueAutoId;
                                issuenceandEquipment.companyId = claimresponse.companyId;
                                _context.issuenceandEquipment.Add(issuenceandEquipment);
                                await _context.SaveChangesAsync();
                            }
                            //foreach (var items in listPermissions)
                            //{
                            //    RoleandPermission rolePerm = new RoleandPermission();
                            //    rolePerm.permissionId = items.ToString();
                            //    rolePerm.roleAutoId = getRoleAutoId;
                            //    rolePerm.companyId = claimresponse.companyId;
                            //    _context.roleAndPermissions.Add(rolePerm);
                            //    await _context.SaveChangesAsync();
                            //}
                        }

                        else if (issuence.validityCheck != 0 && issuence.status=="pending")
                        {
                            var validityCheckValue = issuence.validityCheck;
                            if (IssuenceValidityExists(validityCheckValue))
                            {

                                List<IssuenceList> issuenceLists = issuence.equipList;



                                foreach (var items in issuenceLists)
                                {
                                    IssuenceandEquipment issuenceandEquipment = new IssuenceandEquipment();
                                    issuenceandEquipment.equipName = items.equipName;
                                    issuenceandEquipment.quantity = items.equipQuantity;
                            

                                    issuenceandEquipment.issuenceAutoId = issuence.issuenceAutoId;
                                    issuenceandEquipment.equipAutoId = items.equipAutoId;
                                    issuenceandEquipment.companyId = claimresponse.companyId;
                                    _context.issuenceandEquipment.Add(issuenceandEquipment);
                                    await _context.SaveChangesAsync();
                                }
                            }

                            else
                            {
                                return Unauthorized();
                            }
                        }


                        return Ok();

                }
               
                return Unauthorized();

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// PUT: api/Issuences
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIssuence(Issuence issuence)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    if (issuence.validityCheck != 0)
                    {
                        var existingIssue = await _context.issuences.FindAsync(issuence.validityCheck);
                        existingIssue.issuenceDescp = issuence.issuenceDescp;
                        existingIssue.status = issuence.status;

                        List<IssuenceList> issuenceLists = issuence.equipList;


                        foreach (var items in issuenceLists)
                        {
                            var equipment = _context.equipments.Where(x => x.equipAutoId == items.equipAutoId && issuence.status == "completed").FirstOrDefault();
                            //foreach( var item in equipment)
                            //{
                            //    item.quantity = item.quantity - items.equipQuanity;
                            //    _context.issuenceandEquipment.Add(issuenceandEquipment);
                            //    await _context.SaveChangesAsync();
                            //}
                            if (equipment.equipAutoId != null && equipment.quantity >= items.equipQuantity)

                            {
                                _context.ChangeTracker.Clear();
                                Equipment equi = new Equipment();
                                equi.equipAutoId = equipment.equipAutoId;
                                equi.equipId = equipment.equipId;
                                equi.status = equipment.status;
                                equi.catAutoId = equipment.catAutoId;
                                equi.groupAutoId = equipment.groupAutoId;
                                equi.companyId = claimresponse.companyId;
                                equi.equipName = equipment.equipName;
                                equi.quantity = (equipment.quantity - items.equipQuantity);
                                equi.equipLeadTime = equipment.equipLeadTime;
                                equi.equipCost = equipment.equipCost;
                                _context.equipments.Update(equi);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                        try
                        {
                            _context.issuences.Update(existingIssue);
                            await _context.SaveChangesAsync();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            return StatusCode(StatusCodes.Status500InternalServerError);
                        }
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


    // DELETE: api/Issuences/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIssuence(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {


                    var issuence = await _context.issuences.Where(x => x.issuenceAutoId == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();

                    if (issuence == null)
                    {
                        return NotFound();
                    }

                    _context.issuences.Remove(issuence);
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

        private bool IssuenceExists(int id)
        {
            return _context.issuences.Any(e => e.issuenceAutoId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.issuences.Any(x => x.companyId == id);
        }

        private bool IssuenceValidityExists(int id)
        {
            return _context.issuences.Any(e => e.issuenceAutoId == id);
        }
    }
}
