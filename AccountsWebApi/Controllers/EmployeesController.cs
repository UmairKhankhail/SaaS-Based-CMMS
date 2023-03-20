using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Security.Cryptography;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.AspNetCore.Authorization;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<EmployeesController> _logger;
        private readonly JwtTokenHandler _JwtTokenHandler;
        public EmployeesController(UserDbContext context, ILogger<EmployeesController> logger, JwtTokenHandler jwtTokenHandler)
        {
            _context = context;
            _logger = logger;
            _JwtTokenHandler = jwtTokenHandler;
        }

        // GET: api/Employees
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employee>>> Getemployees()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    return await _context.employees.Where(x => x.companyId == claimresponse.companyId && x.status == "Active").ToListAsync();
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //[HttpGet]
        //public IActionResult MyAction(string name, int age)
        //{
        //    // Do something with the data
        //    // ...
        //    Console.WriteLine(name);
        //    Console.WriteLine(age);
        //    return Ok();
        //}

        // GET: api/Employees/5
        [HttpGet("getEmployee")]
        [Authorize]
        public async Task<ActionResult<Employee>> GetEmployee(string eId)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    var employee = await _context.employees.Where(x => x.companyId == claimresponse.companyId && x.employeeId == eId).FirstOrDefaultAsync();

                    if (employee == null)
                    {
                        return NotFound();
                    }

                    return employee;
                }
                return Unauthorized();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEmployee(Employee employee)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (EmployeeExists(employee.employeeAutoId))
                    {
                        employee.companyId = claimresponse.companyId;
                        _context.Entry(employee).State = EntityState.Modified;

                        await _context.SaveChangesAsync();
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

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Employee>>> PostEmployee(Employee employee)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var compId = _context.employees.Where(e => e.companyId == claimresponse.companyId).Select(e => e.employeeId.ToString()).ToList();
                    var autoId = "";
                    if (compId.Count > 0)
                    {
                        autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Employee c = new Employee();
                        string comId = "E1";
                        c.employeeId = comId;
                        c.employeeName = employee.employeeName;
                        c.employeeeMail = employee.employeeeMail;
                        c.employeeContactNo = employee.employeeContactNo;
                        c.employeeDesignation = employee.employeeDesignation;
                        c.employeeFatherName = employee.employeeFatherName;
                        c.deptAutoId = employee.deptAutoId;
                        c.companyId = claimresponse.companyId;
                        c.status = employee.status;
                        _context.employees.Add(c);
                        await _context.SaveChangesAsync();
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Employee c = new Employee();
                        string comId = "E" + (int.Parse(autoId) + 1);
                        c.employeeId = comId;
                        c.employeeName = employee.employeeName;
                        c.employeeeMail = employee.employeeeMail;
                        c.employeeContactNo = employee.employeeContactNo;
                        c.employeeDesignation = employee.employeeDesignation;
                        c.employeeFatherName = employee.employeeFatherName;
                        c.deptAutoId = employee.deptAutoId;
                        c.companyId = claimresponse.companyId;
                        c.status = employee.status;
                        _context.employees.Add(c);
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
            //_context.employees.Add(employee);
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException)
            //{
            //    if (EmployeeExists(employee.employeeid))
            //    {
            //        return Conflict();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return CreatedAtAction("GetEmployee", new { id = employee.employeeid }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {
                    if (EmployeeExists(id))
                    {
                        var employee = await _context.employees.FindAsync(id);
                        if (employee == null)
                        {
                            return NotFound();
                        }

                        _context.employees.Remove(employee);
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

        private bool EmployeeExists(int id)
        {
            return _context.employees.Any(e => e.employeeAutoId == id);
        }
    }
}
