using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using System.Text.RegularExpressions;
using NuGet.Versioning;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using System.Net.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(UserDbContext context, JwtTokenHandler jwtTokenHandler, HttpClient httpClient, ILogger<DepartmentsController> logger)
        {
            _context = context;
            _JwtTokenHandler = jwtTokenHandler;
            _httpClient = httpClient;
            _logger = logger;
        }

        //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
        //var controller = RouteData.Values["controller"].ToString();
        //var result = $"{controller}Controller.{actionname}";

        // GET: api/Departments
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Department>>> Getdepartments()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.departments.Where(x => x.companyId == claimresponse.companyId).ToListAsync();
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //if (claimresponse is null)
        //{
        //    return Unauthorized();
        //}
        //else
        //{
        //    //return Ok(claimresponse);
        //    if (claimresponse.approle == "admin")
        //    {
        //        count += 1;
        //    }
        //    else if (claimresponse.approle == "user")
        //    {

        //        foreach (var item in claimresponse.role)
        //        {
        //            if(item==controller_action)
        //            {
        //                count += 1;

        //            }
        //        }
        //    }
        //    else { return Unauthorized(); }
        //}

        //if (count > 0)
        //{

        //    return await _context.departments.Where(x => x.companyid == claimresponse.companyid).ToListAsync();
        //}
        //return Unauthorized();

        // GET: api/Departments/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Department>> GetDepartment(string id, string cid)
        {
            try 
            {
                var department = _context.departments.Where(x => x.deptId == id && x.companyId == cid).FirstOrDefault();

                if (department == null)
                {
                    return NotFound();
                }

                return department;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //int count = 0;
            //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            //var controller = RouteData.Values["controller"].ToString();
            //var result = $"{controller}Controller.{actionname}";
            //if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            //{
            //    return Unauthorized();
            //}
            //var accessToken = authHeaderValues.FirstOrDefault()?.Replace("Bearer ", "");
            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    return Unauthorized();
            //}
            //var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
            //if (claimresponse is null)
            //{
            //    return Unauthorized();
            //}
            //else
            //{
            //    if (claimresponse.role == "admin")
            //    { count += 1; }
            //    else if (claimresponse.role == "user")
            //    {
            //        var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
            //        var response = await _httpClient.GetAsync(url);
            //        response.EnsureSuccessStatusCode();
            //        var responseContent = await response.Content.ReadAsStringAsync();
            //        if (responseContent == "A")
            //        { count += 1; }
            //        else { return Unauthorized(); }
            //    }
            //    else { return Unauthorized(); }
            //}

            //if (count > 0)
            //{
            //    var department = _context.departments.Where(x=>x.deptid==id && x.companyid==claimresponse.companyid).FirstOrDefault();

            //    if (department == null)
            //    {
            //        return NotFound();
            //    }

            //    return department;
            //}
            return Ok();
            
            
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutDepartment(string id, Department department)
        {
            try
            {
                if (id != department.deptId)
                {
                    return BadRequest();
                }

                Department d = new Department();
                d.deptAutoId = department.deptAutoId;
                d.deptId = department.deptId;
                d.deptName = department.deptName;
                d.status = department.status;
                d.companyId = department.companyId;
                _context.Entry(d).State = EntityState.Modified;

                
                await _context.SaveChangesAsync();
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //int count = 0;
            //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            //var controller = RouteData.Values["controller"].ToString();
            //var result = $"{controller}Controller.{actionname}";
            //if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            //{
            //    return Unauthorized();
            //}
            //var accessToken = authHeaderValues.FirstOrDefault()?.Replace("Bearer ", "");
            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    return Unauthorized();
            //}
            //var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
            //if (claimresponse is null)
            //{
            //    return Unauthorized();
            //}
            //else
            //{
            //    if (claimresponse.role == "admin")
            //    { count += 1; }
            //    else if (claimresponse.role == "user")
            //    {
            //        var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
            //        var response = await _httpClient.GetAsync(url);
            //        response.EnsureSuccessStatusCode();
            //        var responseContent = await response.Content.ReadAsStringAsync();
            //        if (responseContent == "A")
            //        { count += 1; }
            //        else { return Unauthorized(); }
            //    }
            //    else { return Unauthorized(); }
            //}

            //if (count > 0)
            //{
            //    if (id != department.deptid)
            //    {
            //        return BadRequest();
            //    }

            //    Department d = new Department();
            //    d.deptautoid = department.deptautoid;
            //    d.deptid = department.deptid;
            //    d.deptname = department.deptname;
            //    d.status = department.status;
            //    d.companyid = claimresponse.companyid;
            //    _context.Entry(d).State = EntityState.Modified;

            //    try
            //    {
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        throw;
            //    }
            //}
            return Ok();
            
        }

        // POST: api/Departments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Department>>> PostDepartment(Department department)
        {
            try
            {
                var compId = _context.departments.Where(d => d.companyId == department.companyId).Select(d => d.deptId).ToList();

                var autoId = "";
                if (compId.Count > 0)
                {

                    autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoId == "")
                {
                    _context.ChangeTracker.Clear();
                    Department c = new Department();
                    string comid = "D1";
                    c.deptId = comid;
                    c.deptName = department.deptName;
                    c.companyId = department.companyId;
                    c.status = department.status;
                    _context.departments.Add(c);
                    await _context.SaveChangesAsync();
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    Department c = new Department();
                    string comid = "D" + (int.Parse(autoId) + 1);
                    c.deptId = comid;
                    c.deptName = department.deptName;
                    c.companyId = department.companyId;
                    c.status = department.status;
                    _context.departments.Add(c);
                    await _context.SaveChangesAsync();
                }

                return await _context.departments.Where(x => x.companyId == department.companyId).ToListAsync();

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //int count = 0;
            //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            //var controller = RouteData.Values["controller"].ToString();
            //var result = $"{controller}Controller.{actionname}";
            //if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            //{
            //    return Unauthorized();
            //}
            //var accessToken = authHeaderValues.FirstOrDefault()?.Replace("Bearer ", "");
            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    return Unauthorized();
            //}
            //var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
            //if (claimresponse is null)
            //{
            //    return Unauthorized();
            //}
            //else
            //{
            //    if (claimresponse.role == "admin")
            //    { count += 1; }
            //    else if (claimresponse.role == "user")
            //    {
            //        var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
            //        var response = await _httpClient.GetAsync(url);
            //        response.EnsureSuccessStatusCode();
            //        var responseContent = await response.Content.ReadAsStringAsync();
            //        if (responseContent == "A")
            //        { count += 1; }
            //        else { return Unauthorized(); }
            //    }
            //    else { return Unauthorized(); }
            //}


            //if (count > 0)
            //{
            //    var compid = _context.departments.Where(d => d.companyid == department.companyid).Select(d => d.deptid).ToList();

            //    var autoid = "";
            //    if (compid.Count > 0)
            //    {

            //        autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
            //    }

            //    if (autoid == "")
            //    {
            //        _context.ChangeTracker.Clear();
            //        Department c = new Department();
            //        string comid = "D1";
            //        c.deptid = comid;
            //        c.deptname = department.deptname;
            //        c.companyid = claimresponse.companyid;
            //        c.status = department.status;
            //        _context.departments.Add(c);
            //        await _context.SaveChangesAsync();
            //    }
            //    if (autoid != "")
            //    {
            //        _context.ChangeTracker.Clear();
            //        Department c = new Department();
            //        string comid = "D" + (int.Parse(autoid) + 1);
            //        c.deptid = comid;
            //        c.deptname = department.deptname;
            //        c.companyid = claimresponse.companyid;
            //        c.status = department.status;
            //        _context.departments.Add(c);
            //        await _context.SaveChangesAsync();
            //    }

            //    return await _context.departments.Where(x=>x.companyid==claimresponse.companyid).ToListAsync();
            //}


            //_context.departments.Add(department);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (DepartmentExists(department.deptid))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetDepartment", new { id = department.deptid }, department);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(string id, string cid)
        {
            try
            {
                var department = _context.departments.Where(x => x.deptId == id && x.companyId == cid).FirstOrDefault();
                if (department == null)
                {
                    return NotFound();
                }

                _context.departments.Remove(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
            //int count = 0;
            //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            //var controller = RouteData.Values["controller"].ToString();
            //var result = $"{controller}Controller.{actionname}";
            //if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
            //{
            //    return Unauthorized();
            //}
            //var accessToken = authHeaderValues.FirstOrDefault()?.Replace("Bearer ", "");
            //if (string.IsNullOrEmpty(accessToken))
            //{
            //    return Unauthorized();
            //}
            //var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken });
            //if (claimresponse is null)
            //{
            //    return Unauthorized();
            //}
            //else
            //{
            //    if (claimresponse.role == "admin")
            //    { count += 1; }
            //    else if (claimresponse.role == "user")
            //    {
            //        var url = $"http://localhost:5088/api/Permissions/GetPermission?uid={claimresponse.uid}&uautoid={claimresponse.uautoid}&cid={claimresponse.companyid}&caresult={result}";
            //        var response = await _httpClient.GetAsync(url);
            //        response.EnsureSuccessStatusCode();
            //        var responseContent = await response.Content.ReadAsStringAsync();
            //        if (responseContent == "A")
            //        { count += 1; }
            //        else { return Unauthorized(); }
            //    }
            //    else { return Unauthorized(); }
            //}

            //if (count > 0)
            //{
                
            //        var department = _context.departments.Where(x => x.deptid == id && x.companyid == claimresponse.companyid).FirstOrDefault();
            //        if (department == null)
            //        {
            //            return NotFound();
            //        }

            //        _context.departments.Remove(department);
            //        await _context.SaveChangesAsync();
                
                
            //}

            
        }

        private bool DepartmentExists(string id, string cid)
        {
            return _context.departments.Any(x => x.deptId == id && x.companyId == cid);
        }
    }
}
