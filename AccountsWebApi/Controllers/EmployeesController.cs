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

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(UserDbContext context, ILogger<EmployeesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> Getemployees(string id)
        {
            try
            {
                return await _context.employees.Where(x => x.companyid == id).ToListAsync();
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
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id, string eid)
        {
            try
            {
                var employee = await _context.employees.Where(x => x.companyid == id && x.employeeid == eid).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return NotFound();
                }

                return employee;
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
        public async Task<IActionResult> PutEmployee(string id, Employee employee)
        {
            try
            {
                if (EmployeeExists(employee.employeeautoid) == true)
                {
                    if (id != employee.employeeid)
                    {
                        return BadRequest();
                    }

                    _context.Entry(employee).State = EntityState.Modified;

                    
                    await _context.SaveChangesAsync();
                    
                }
                return NoContent();
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
        public async Task<ActionResult<IEnumerable<Employee>>> PostEmployee(Employee employee)
        {
            try
            {

                var compid = _context.employees.Where(e => e.companyid == employee.companyid).Select(e => e.employeeid.ToString()).ToList();
                var autoid = "";
                if (compid.Count > 0)
                {
                    autoid = compid.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoid == "")
                {
                    _context.ChangeTracker.Clear();
                    Employee c = new Employee();
                    string comid = "E1";
                    c.employeeid = comid;
                    c.employeename = employee.employeename;
                    c.employeeemail = employee.employeeemail;
                    c.employeecontactno = employee.employeecontactno;
                    c.employeedesignation = employee.employeedesignation;
                    c.employeefathername = employee.employeefathername;
                    c.deptautoid = employee.deptautoid;
                    c.companyid = employee.companyid;
                    c.status = employee.status;
                    _context.employees.Add(c);
                    await _context.SaveChangesAsync();
                }
                if (autoid != "")
                {
                    _context.ChangeTracker.Clear();
                    Employee c = new Employee();
                    string comid = "E" + (int.Parse(autoid) + 1);
                    c.employeeid = comid;
                    c.employeename = employee.employeename;
                    c.employeeemail = employee.employeeemail;
                    c.employeecontactno = employee.employeecontactno;
                    c.employeedesignation = employee.employeedesignation;
                    c.employeefathername = employee.employeefathername;
                    c.deptautoid = employee.deptautoid;
                    c.companyid = employee.companyid;
                    c.status = employee.status;
                    _context.employees.Add(c);
                    await _context.SaveChangesAsync();
                }

                return Ok();


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
        public async Task<IActionResult> DeleteEmployee(string id, string eid)
        {
            try
            {
                var employee = _context.employees.Where(x => x.companyid == id && x.employeeid == eid).FirstOrDefault();
                if (employee == null)
                {
                    return NotFound();
                }

                _context.employees.Remove(employee);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.employees.Any(e => e.employeeautoid == id);
        }
    }
}
