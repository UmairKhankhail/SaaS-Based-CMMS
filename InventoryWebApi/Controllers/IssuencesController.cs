using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using AccountsWebApi.Models;
using StackExchange.Redis;
using System.ComponentModel.Design;
using InventoryWebApi.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuencesController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EquipmentsController> _logger;

        public IssuencesController(InventoryDbContext context, HttpClient httpClient, ILogger<EquipmentsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Issuences
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issuence>>> Getissuences(string companyId)
        {
            try
            {
               var result= await _context.issuences.Where(x => x.companyId == companyId ).ToListAsync();
               if (result==null) {
                return Unauthorized();
                }

                return result;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            

       
        }

        // GET: api/Issuences/5
        [HttpGet("id")]
        public async Task<ActionResult<Issuence>> GetIssuence(string companyId, int issueId)
        {
            try
            {

                var getIssuenceRequests = await _context.issuences
                    .Join(_context.issuenceandEquipment, i => i.issuenceAutoId, ie => ie.issuenceAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == companyId && x.i.companyId == companyId && x.i.issuenceAutoId == issueId)
                    .Select(result => new
                    {
                        result.i.issuenceAutoId,
                        result.i.issuenceId,
                        result.i.companyId,
                        result.ie.equipAutoId,
                        result.i.issuenceDescp,
                    }
                    ).ToListAsync();
                return Ok(getIssuenceRequests);



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

                int getIssueAutoId = 0;

                if (issuence.validityCheck == 0)
                {

                    var compId = _context.issuences.Where(d => d.companyId == issuence.companyId).Select(d => d.issuenceId).ToList();

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
                        issue.companyId = issuence.companyId;
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
                        issue.companyId = issuence.companyId;
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
                        issuenceandEquipment.quantity = items.equipQuanity;
                        issuenceandEquipment.equipAutoId = items.equipAutoId;
                        issuenceandEquipment.issuenceAutoId = getIssueAutoId;
                        issuenceandEquipment.companyId = issuence.companyId;
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
                            issuenceandEquipment.quantity = items.equipQuanity;
                            

                            issuenceandEquipment.issuenceAutoId = issuence.issuenceAutoId;
                            issuenceandEquipment.equipAutoId = items.equipAutoId;
                            issuenceandEquipment.companyId = issuence.companyId;
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

                //    }
                //    return Unauthorized();

            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //// PUT: api/Issuences
        [HttpPut]
        public async Task<IActionResult> PutIssuence(Issuence issuence)
        {
            if (issuence.validityCheck != 0)
            {
                var existingIssue = await _context.issuences.FindAsync(issuence.validityCheck);
                existingIssue.issuenceDescp=issuence.issuenceDescp;
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
                    if (equipment.equipAutoId != null && equipment.quantity >= items.equipQuanity)

                    {
                        _context.ChangeTracker.Clear();
                        Equipment equi = new Equipment();
                        equi.equipAutoId = equipment.equipAutoId;
                        equi.equipId = equipment.equipId;
                        equi.status = equipment.status;
                        equi.catAutoId = equipment.catAutoId;
                        equi.groupAutoId = equipment.groupAutoId;
                        equi.companyId = equipment.companyId;
                        equi.equipName = equipment.equipName;
                        equi.quantity = (equipment.quantity - items.equipQuanity);
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
                    //_logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Unauthorized();
        

    }


    // DELETE: api/Issuences/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssuence(int id,string companyId)
        {
            var issuence = await _context.issuences.Where(x => x.issuenceAutoId == id && x.companyId == companyId).FirstOrDefaultAsync();

            if (issuence == null)
            {
                return NotFound();
            }

            _context.issuences.Remove(issuence);
            await _context.SaveChangesAsync();

            return NoContent();
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
