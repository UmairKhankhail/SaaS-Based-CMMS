using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using NuGet.Protocol;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using System.Security.Cryptography;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubDepartmentsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly HttpClient _httpClient; 
        private readonly ILogger<SubDepartmentsController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public SubDepartmentsController(UserDbContext context, HttpClient httpClient, ILogger<SubDepartmentsController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/SubDepartments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubDepartment>>> Getsub_departments()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.subDepartments.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //return await _context.sub_departments.ToListAsync();
        }


        //var getsubdept = _context.sub_departments.Select(x => x.subdeptid);
        //foreach(var x in getsubdept)
        //{
        //    int index = x.IndexOf("D1"); // get the starting index of the search string
        //    string value = x.Substring(index, "D1".Length); // extract the substring
        //    Console.WriteLine("Found value: " + value);
        //}
        //var getsubdeptsingle = getsubdept.Max(x => x.Split("S").Last());
        //Console.WriteLine(getsubdeptsingle);
    

            
        

        // GET: api/SubDepartments/5
        [HttpGet("getSubDepartment")]
        [Authorize]
        public async Task<ActionResult<SubDepartment>> GetSubDepartment(string id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var subDepartment = await _context.subDepartments.Where(x => x.companyId == claimresponse.companyId && x.subDeptId == id).FirstOrDefaultAsync();

                    if (subDepartment == null)
                    {
                        return NotFound();
                    }

                    return subDepartment;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/SubDepartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutSubDepartment(SubDepartment subDepartment)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (SubDepartmentExists(subDepartment.subDeptAutoId))
                    {
                        subDepartment.companyId = claimresponse.companyId;
                        _context.Entry(subDepartment).State = EntityState.Modified;
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

        // POST: api/SubDepartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<SubDepartment>>> PostSubDepartment(SubDepartment subDepartment)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var subDept = _context.subDepartments.Where(d => d.companyId == claimresponse.companyId).Select(d => d.subDeptId).ToList();
                    List<string> subDeptList = new List<string>();
                    List<int> subDeptNoList = new List<int>();
                    foreach (var z in subDept)
                    {
                        if (z.Contains(subDepartment.deptSingleId + "S"))
                        {
                            subDeptList.Add(z);
                        }
                    }
                    if (subDeptList.Count > 0)
                    {
                        foreach (var x in subDeptList)
                        {
                            subDeptNoList.Add(int.Parse(x.Split("S").Last().ToString()));
                        }
                    }
                    //foreach (var x in subdept)
                    //{
                    //    if (x.Contains(subDepartment.deptsingleid + "S"))
                    //    {
                    //        subdeptlist.Add(x);
                    //    }
                    //}
                    //Console.WriteLine(subdeptlist.Count);



                    if (subDeptNoList.Count == 0)
                    {
                        _context.ChangeTracker.Clear();
                        SubDepartment c = new SubDepartment();
                        c.subDeptId = subDepartment.deptSingleId + "S1";
                        c.subDeptName = subDepartment.subDeptName;
                        c.companyId = claimresponse.companyId;
                        c.status = "Active";
                        c.deptAutoId = subDepartment.deptAutoId;
                        _context.subDepartments.Add(c);
                        await _context.SaveChangesAsync();
                    }
                    if (subDeptNoList.Count > 0)
                    {
                        _context.ChangeTracker.Clear();
                        SubDepartment c = new SubDepartment();
                        string comid = subDepartment.deptSingleId + "S" + (subDeptNoList.Max() + 1);
                        c.subDeptId = comid;
                        c.subDeptName = subDepartment.subDeptName;
                        c.companyId = claimresponse.companyId;
                        c.status = "Active";
                        c.deptAutoId = subDepartment.deptAutoId;
                        _context.subDepartments.Add(c);
                        await _context.SaveChangesAsync();
                    }




                    //var compid = _context.sub_departments.Select(x => x.subdeptid).ToList();
                    //var autoid = "";
                    //if (compid.Count > 0)
                    //{
                    //    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                    //}

                    //if (autoid == "")
                    //{
                    //    _context.ChangeTracker.Clear();
                    //    SubDepartment c = new SubDepartment();
                    //    string comid = subDepartment.deptid+"S1";
                    //    c.subdeptid = comid;
                    //    c.subdeptname = subDepartment.subdeptname;
                    //    c.companyid = subDepartment.companyid;
                    //    c.deptid = subDepartment.deptid;
                    //    c.status = subDepartment.status;
                    //    _context.sub_departments.Add(c);
                    //    await _context.SaveChangesAsync();
                    //}
                    //if (autoid != "")
                    //{
                    //    _context.ChangeTracker.Clear();
                    //    SubDepartment c = new SubDepartment();
                    //    string comid = "D" + (int.Parse(autoid) + 1);
                    //    c.subdeptid = comid;
                    //    c.subdeptname = subDepartment.subdeptname;
                    //    c.companyid = subDepartment.companyid;
                    //    c.deptid = subDepartment.deptid;
                    //    c.status = subDepartment.status;
                    //    _context.sub_departments.Add(c);
                    //    await _context.SaveChangesAsync();
                    //}

                    return Ok();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //_context.sub_departments.Add(subDepartment);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (SubDepartmentExists(subDepartment.subdeptid))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetSubDepartment", new { id = subDepartment.subdeptid }, subDepartment);
        }

        // DELETE: api/SubDepartments/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSubDepartment(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (SubDepartmentExists(id))
                    {
                        var subDepartment = await _context.subDepartments.FindAsync(id);
                        if (subDepartment == null)
                        {
                            return NotFound();
                        }

                        _context.subDepartments.Remove(subDepartment);
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

        private bool SubDepartmentExists(int id)
        {
            return _context.subDepartments.Any(e => e.subDeptAutoId == id);
        }
    }
}
