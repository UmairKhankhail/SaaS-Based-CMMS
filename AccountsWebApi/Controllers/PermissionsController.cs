using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountsWebApi.Models;
//Libraries
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Text.RegularExpressions;
using JwtAuthenticationManager.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Collections;

namespace AccountsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly UserDbContext _context;
        private readonly ILogger<PermissionsController> _logger;
        public PermissionsController(UserDbContext context, ILogger<PermissionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Permissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Permission>>> Getpermissions()
        {
            var outputControllers=await GetAllControllerMethods();
            
            //if (perid.Count > 0)
            //{
            //    autoid = perid.Max(x => int.Parse(x.Substring(1))).ToString();
            //}

            foreach (var outputController in outputControllers)
            {
                var perId = _context.permissions.Select(x => x.permissionId).ToList();
                var autoId = "";
                if (perId.Count > 0)
                {
                    autoId = perId.Max(x => int.Parse(x.Substring(1))).ToString();
                }

                if (autoId=="")
                {
                    _context.ChangeTracker.Clear();
                    Permission per = new Permission();
                    string comid = "P1";
                    per.permissionId = comid;
                    per.permissionName = outputController;
                    per.status = "Active";

                    _context.permissions.Add(per);
                    var record = await _context.SaveChangesAsync();
                    //return Ok(per);
                }
                if (autoId != "")
                {
                    _context.ChangeTracker.Clear();
                    if (!PermissionNameExists(outputController))
                    {
                        Permission per1 = new Permission();
                        string comId = "P" + (Convert.ToInt32(autoId) + 1);
                        per1.permissionId = comId;
                        per1.permissionName = outputController;
                        per1.status = "Active";

                        _context.permissions.Add(per1);
                        var record = await _context.SaveChangesAsync();
                        //return Ok(per);
                    }
                }
            }

            return await _context.permissions.ToListAsync();
        }

        //public static List<string> GetAllControllerMethods()
        //{
        //    var methods = new List<string>();
        //    var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

        //    foreach (var controllerType in controllerTypes)
        //    {
        //        var controllerMethods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
        //            .Where(method => !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute)));

        //        foreach (var method in controllerMethods)
        //        {
        //            methods.Add($"{controllerType.Name}.{method.Name}");
        //        }
        //    }
            
        //    return methods;
        //}

        //[HttpGet("getallcontrollerstest")]
        public async static Task<List<string>> GetAllControllerMethods()
        {
            var methods = new List<string>();
            var controllerTypes = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList();

            foreach (var controllerType in controllerTypes)
            {
                var controllerMethods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(method => !method.IsSpecialName && !method.IsDefined(typeof(NonActionAttribute)));

                foreach (var method in controllerMethods)
                {
                    methods.Add($"{controllerType.Name}.{method.Name}");
                }
            }

            // Call the Preventive Maintenance API to retrieve its controller and methods
            var apiUrl = "http://localhost:5182/api/ScheduledWorkRequests/GetControllersAndMethods";
            var client = new HttpClient();
            var response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var apiContent = await response.Content.ReadAsStringAsync();
                var otherApiMethods = JsonConvert.DeserializeObject<List<string>>(apiContent);
                methods.AddRange(otherApiMethods);
            }

            //// Call the Assets API to retrieve its controller and methods
            var apiUrlAsset = "http://localhost:5231/api/LinearAssetModels/GetControllersAndMethods";
            var clientAsset = new HttpClient();
            var responseAsset = await clientAsset.GetAsync(apiUrlAsset);

            if (responseAsset.IsSuccessStatusCode)
            {
                var apiContentAsset = await responseAsset.Content.ReadAsStringAsync();
                var otherApiMethodsAsset = JsonConvert.DeserializeObject<List<string>>(apiContentAsset);
                methods.AddRange(otherApiMethodsAsset);
            }

            //// Call the Inventory API to retrieve its controller and methods
            var apiUrlEquipments = "http://localhost:5269/api/Equipments/GetControllersAndMethods";
            var clientEquipments = new HttpClient();
            var responseEquipments = await clientEquipments.GetAsync(apiUrlEquipments);

            if (responseEquipments.IsSuccessStatusCode)
            {
                var apiContentEquipments = await responseEquipments.Content.ReadAsStringAsync();
                var otherApiMethodsEquipments = JsonConvert.DeserializeObject<List<string>>(apiContentEquipments);
                methods.AddRange(otherApiMethodsEquipments);
            }


            // Call the WO API to retrieve its controller and methods
            var apiUrlWO = "http://localhost:5145/api/WorkOrders/GetControllersAndMethods";
            var clientWO = new HttpClient();
            var responseWO = await clientWO.GetAsync(apiUrlWO);

            if (responseWO.IsSuccessStatusCode)
            {
                var apiContentWO = await responseWO.Content.ReadAsStringAsync();
                var otherApiMethodsWO = JsonConvert.DeserializeObject<List<string>>(apiContentWO);
                methods.AddRange(otherApiMethodsWO);
            }


            return methods;
        }



        // GET: api/Permissions/5
        [HttpGet("{uId, uAutoId, cId}")]
        public async Task<IActionResult> GetPermission(string uId, string uAutoId, string cId)
        {

            //var permissionslist = await _context.roles
            //.Join(_context.roleandpermissions, r => r.roleautoid, rp => rp.roleautoid, (r, rp) => new { r, rp })
            //.Where(x => x.r.companyid == cid && x.r.roleautoid == rid && roleIds.Contains(x.r.roleautoid))
            //.SelectMany(x => _context.permissions.Where(p => p.permissionid == x.rp.permissionid), (x, p) => p.permissionname)
            //.ToListAsync();

            //return Ok(permissionslist);
            List<string> permissionsList = new List<string>();
            var resultOuput = "";
            var getRoleId = await _context.users
                .Join(_context.userAndRoles, d => d.userAutoId, sd => sd.userAutoId, (d, sd) => new { d, sd })
                .Where(x => x.sd.companyId == cId && x.d.companyId == cId && x.d.userId == uId)
                .Select(result => new
                {
                    result.sd.roleAutoId
                }).ToListAsync();
            foreach (var x in getRoleId)
            {
                var rid = x.roleAutoId;
                //   var getrole = await _context.roles
                //.Join(_context.roleandpermissions, d => d.roleautoid, sd => sd.roleautoid, (d, sd) => new { d, sd })
                //.Where(x => x.sd.companyid == cid && x.d.companyid == cid && x.d.roleautoid == rid)
                //.Select(result => new
                //{
                //    result.d.roleautoid,
                //    result.d.rolename,
                //    result.sd.permissionid
                //}).ToListAsync();

                var dept = await _context.roles
                    .Join(_context.roleAndPermissions, d => d.roleAutoId, sd => sd.roleAutoId, (d, sd) => new { d, sd })
                    .Where(x => x.sd.companyId == cId && x.d.companyId == cId && x.d.roleAutoId == rid)
                    .Select(result => new
                    {
                        //result.d.roleautoid,
                        //result.d.rolename,
                        result.sd.permissionId
                    }).ToListAsync();
                //int count = 0;

                foreach (var y in dept)
                {
                    //var perid = y.permissionid;
                    var pername = _context.permissions.Where(p => p.permissionId == y.permissionId).Select(p => p.permissionName).FirstOrDefault();
                    //Console.WriteLine(pername);
                    permissionsList.Add(pername);
                }
                //if (count > 0) { resultouput = "A"; }
                //else { resultouput = "N"; }


            }
            return Ok(permissionsList.ToList());
            //var actionname = base.ControllerContext.ActionDescriptor.ActionName;
            //var controller = RouteData.Values["controller"].ToString();
            //var result = controller + "Controller" + "." + actionname;
            //var dept = await _context.roles
            //.Join(_context.roleandpermissions, d => d.roleautoid, sd => sd.roleautoid, (d, sd) => new { d, sd })
            //.Where(x => x.sd.companyid == "C1" && x.d.companyid == "C1" && x.d.roleautoid == 1)
            //.Select(result => new
            //{
            //    result.d.roleautoid,
            //    result.d.rolename,
            //    result.sd.permissionid
            //}).ToListAsync();
            //int count = 0;
            //foreach (var x in dept)
            //{
            //    var perid = x.permissionid;
            //    var pername = _context.permissions.Where(p => p.permissionid == perid).Select(p => p.permissionname).FirstOrDefault();
            //    Console.WriteLine(pername);
            //    if (pername == result) { count += 1; }
            //}
            //if (count > 0) { Console.WriteLine("Allowed"); }
            //else { Console.WriteLine("Not Allowed"); }
            //return Ok();

            //var permission = await _context.permissions.FindAsync(id);

            //if (permission == null)
            //{
            //    return NotFound();
            //}

            //return permission;
        }

        // PUT: api/Permissions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPermission(string id, Permission permission)
        {
            try
            {
                if (id != permission.permissionId)
                {
                    return BadRequest();
                }

                _context.Entry(permission).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // POST: api/Permissions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Permission>> PostPermission(Permission permission)
        {
            try
            {
                _context.permissions.Add(permission);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (PermissionExists(permission.permissionId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtAction("GetPermission", new { id = permission.permissionId }, permission);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // DELETE: api/Permissions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePermission(string id)
        {
            try
            {
                var permission = await _context.permissions.FindAsync(id);
                if (permission == null)
                {
                    return NotFound();
                }

                _context.permissions.Remove(permission);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool PermissionExists(string id)
        {
            return _context.permissions.Any(e => e.permissionId == id);
        }
        private bool PermissionNameExists(string name)
        {
            return _context.permissions.Any(e => e.permissionName == name);
        }
    }
}
