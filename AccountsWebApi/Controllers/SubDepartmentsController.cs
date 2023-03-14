﻿using System;
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

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubDepartmentsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly HttpClient _httpClient; 
        private readonly ILogger<SubDepartmentsController> _logger;
        public SubDepartmentsController(UserDbContext context, HttpClient httpClient, ILogger<SubDepartmentsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/SubDepartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubDepartment>>> Getsub_departments(string id)
        {
            try
            {
                return await _context.subdepartments.Where(x => x.companyid == id).ToListAsync();
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
        [HttpGet("{id}")]
        public async Task<ActionResult<SubDepartment>> GetSubDepartment(string id, string cid)
        {
            try
            {

                var subDepartment = await _context.subdepartments.Where(x => x.companyid == cid && x.subdeptid == id).FirstOrDefaultAsync();

                if (subDepartment == null)
                {
                    return NotFound();
                }

                return subDepartment;
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
        public async Task<IActionResult> PutSubDepartment(string id, SubDepartment subDepartment)
        {
            try
            {
                if (id != subDepartment.subdeptid)
                {
                    return BadRequest();
                }

                _context.Entry(subDepartment).State = EntityState.Modified;

                await _context.SaveChangesAsync();


                return NoContent();
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
        public async Task<ActionResult<IEnumerable<SubDepartment>>> PostSubDepartment(SubDepartment subDepartment)
        {
            try
            {
                var subdept = _context.subdepartments.Where(d => d.companyid == subDepartment.companyid).Select(d => d.subdeptid).ToList();
                List<string> subdeptlist = new List<string>();
                List<int> subdeptnolist = new List<int>();
                foreach (var z in subdept)
                {
                    if (z.Contains(subDepartment.deptsingleid + "S"))
                    {
                        subdeptlist.Add(z);
                    }
                }
                if (subdeptlist.Count > 0)
                {
                    foreach (var x in subdeptlist)
                    {
                        subdeptnolist.Add(int.Parse(x.Split("S").Last().ToString()));
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



                if (subdeptnolist.Count == 0)
                {
                    _context.ChangeTracker.Clear();
                    SubDepartment c = new SubDepartment();
                    c.subdeptid = subDepartment.deptsingleid + "S1";
                    c.subdeptname = subDepartment.subdeptname;
                    c.companyid = subDepartment.companyid;
                    c.status = "Active";
                    c.deptautoid = subDepartment.deptautoid;
                    _context.subdepartments.Add(c);
                    await _context.SaveChangesAsync();
                }
                if (subdeptnolist.Count > 0)
                {
                    _context.ChangeTracker.Clear();
                    SubDepartment c = new SubDepartment();
                    string comid = subDepartment.deptsingleid + "S" + (subdeptnolist.Max() + 1);
                    c.subdeptid = comid;
                    c.subdeptname = subDepartment.subdeptname;
                    c.companyid = subDepartment.companyid;
                    c.status = "Active";
                    c.deptautoid = subDepartment.deptautoid;
                    _context.subdepartments.Add(c);
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
        public async Task<IActionResult> DeleteSubDepartment(string id, string sid)
        {
            try
            {
                var subDepartment = _context.subdepartments.Where(x => x.companyid == id && x.subdeptid == sid).FirstOrDefault();
                if (subDepartment == null)
                {
                    return NotFound();
                }

                _context.subdepartments.Remove(subDepartment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool SubDepartmentExists(string id)
        {
            return _context.subdepartments.Any(e => e.subdeptid == id);
        }
    }
}
