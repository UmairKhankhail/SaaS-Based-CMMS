using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using InventoryWebApi.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EquipmentsController> _logger;


        public PurchasesController(InventoryDbContext context, HttpClient httpClient, ILogger<EquipmentsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> Getpurchases(string companyId)
        {
            try
            {
                return await _context.purchases.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Purchases/5
        [HttpGet("id")]
        public async Task<ActionResult<Purchase>> GetPurchase(string companyId,int id)
        {
            try
            {

                var getPurchaseRequests = await _context.purchases
                    .Join(_context.purchaseandEquipment, i => i.purchaseAutoId, ie => ie.purchaseAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == companyId && x.i.companyId == companyId && x.i.purchaseAutoId==id)
                    .Select(result => new
                    {
                        result.i.purchaseAutoId,
                        result.i.purchaseId,
                        result.i.status,
                        result.i.companyId,
                        result.ie.equipAutoId,
                        result.i.purchasesDescp,
                    }
                    ).ToListAsync();
                return Ok(getPurchaseRequests);



            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutPurchase(Purchase purchase)
        {
            if (purchase.validityCheck != 0)
            {
                var existingPurchase = await _context.purchases.FindAsync(purchase.validityCheck);
                existingPurchase.purchasesDescp = purchase.purchasesDescp;
                existingPurchase.status = purchase.status;

                List<PurchaseList> purchaseLists = purchase.equipList;
                foreach (var items in purchaseLists)
                {

                    var equipment = _context.equipments.Where(x => x.equipAutoId == items.equipAutoId && purchase.status == "completed").FirstOrDefault();
                    //foreach( var item in equipment)
                    //{
                    //    item.quantity = item.quantity - items.equipQuanity;
                    //    _context.issuenceandEquipment.Add(issuenceandEquipment);
                    //    await _context.SaveChangesAsync();
                    //}
                    if (equipment.equipAutoId != null)

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
                        equi.quantity = (equipment.quantity + items.equipQuantity);
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
                    _context.purchases.Update(existingPurchase);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex.Message);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

             
            }

            return NotFound();
            //if (IssuenceExists(id) && CompanyExists(companyId))
            //{
            //    Purchase pur = new Purchase();
            //    pur.purchaseAutoId = purchase.purchaseAutoId;
            //    pur.purchaseId = purchase.purchaseId;
            //    pur.companyId = purchase.companyId;
            //    pur.status = purchase.status;

            //    pur.purchasesDescp=purchase.purchasesDescp;
            //    pur.userAutoId = userAutoId;

            //    var listEquipments = purchase.equipList;
            //    var listDbEquipments = new List<string>();
            //    var getEquipments = _context.purchaseandEquipment.Where(x => x.purchaseAutoId == purchase.purchaseAutoId && x.companyId == purchase.companyId).Select(x => x.equipAutoId);

            //    foreach (var item in getEquipments)
            //    {
            //        listDbEquipments.Add(item.ToString());
            //        Console.WriteLine("DB Equipments: " + item);
            //    }

            //    foreach (var equip in listEquipments)
            //    {
            //        Console.WriteLine("New Equipments: " + equip);

            //    }

            //    var resultListLeft = listDbEquipments.Except(listEquipments).ToList();
            //    var resultListRight = listEquipments.Except(listDbEquipments).ToList();

            //    var rll = new List<string>();
            //    var rlr = new List<string>();
            //    if (resultListLeft != null)
            //    {
            //        foreach (var x in resultListLeft)
            //        {
            //            rll.Add(x);
            //        }
            //    }
            //    if (resultListRight != null)
            //    {
            //        foreach (var x in resultListRight)
            //        {
            //            rlr.Add(x);
            //        }
            //    }

            //    if (rll != null)
            //    {
            //        foreach (var item in rll)
            //        {
            //            var delUser = _context.purchaseandEquipment.Where(x => x.companyId == companyId && x.purchaseAutoId == purchase.purchaseAutoId && x.equipAutoId == int.Parse(item)).FirstOrDefault();
            //            if (delUser == null)
            //            {
            //                return NotFound();
            //            }
            //            _context.purchaseandEquipment.Remove(delUser);
            //        }
            //    }

            //    if (rlr != null)
            //    {
            //        foreach (var item in rlr)
            //        {
            //            PurchaseandEquipment purchaseEquipment = new PurchaseandEquipment();
            //            purchaseEquipment.purchaseAutoId = purchase.purchaseAutoId;
            //            purchaseEquipment.equipAutoId = int.Parse(item);
            //            purchaseEquipment.companyId = purchase.companyId;
            //            _context.purchaseandEquipment.Add(purchaseEquipment);
            //        }
            //    }


            //    _context.Entry(pur).State = EntityState.Modified;

            //    await _context.SaveChangesAsync();
            //    return Ok();
            //}

            
        }

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {

            try
            {

                int getPurchaseAutoId = 0;

                if (purchase.validityCheck == 0)
                {

                    var compId = _context.purchases.Where(d => d.companyId == purchase.companyId).Select(d => d.purchaseId).ToList();

                    var autoId = "";
                    if (compId.Count > 0)
                    {

                        autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        Purchase pur = new Purchase();
                        string comId = "PR1";
                        pur.purchaseAutoId = purchase.purchaseAutoId;
                        pur.purchaseId = comId;
                        pur.companyId = purchase.companyId;
                        pur.status = purchase.status;

                        pur.purchasesDescp = purchase.purchasesDescp;
                        pur.userAutoId = purchase.userAutoId;
                        _context.purchases.Add(pur);
                        await _context.SaveChangesAsync();
                        getPurchaseAutoId = pur.purchaseAutoId;
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        Purchase pur = new Purchase();
                        string comId = "PR" + (int.Parse(autoId) + 1);
                        pur.purchaseAutoId = purchase.purchaseAutoId;
                        pur.purchaseId = comId;
                        pur.companyId = purchase.companyId;
                        pur.status = purchase.status;

                        pur.purchasesDescp = purchase.purchasesDescp;
                        pur.userAutoId = purchase.userAutoId;
                        _context.purchases.Add(pur);
                        await _context.SaveChangesAsync();
                        getPurchaseAutoId = pur.purchaseAutoId;
                    }

                    //Console.WriteLine("Id: "+ getroleautoid.ToString());
                    //_context.roles.Add(role);
                    //await _context.SaveChangesAsync();

                    List<PurchaseList> purchaseLists = purchase.equipList;



                    foreach (var items in purchaseLists)
                    {
                        PurchaseandEquipment purchaseandEquipment = new PurchaseandEquipment();
                        purchaseandEquipment.equipName = items.equipName;
                        purchaseandEquipment.quantity = items.equipQuantity;
                        purchaseandEquipment.equipAutoId = items.equipAutoId;
                        purchaseandEquipment.purchaseAutoId = getPurchaseAutoId;
                        purchaseandEquipment.companyId = purchase.companyId;
                        _context.purchaseandEquipment.Add(purchaseandEquipment);
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

                else if (purchase.validityCheck != 0 && purchase.status=="pending")
                {
                    var validityCheckValue = purchase.validityCheck;
                    if (PurchaseValidityExists(validityCheckValue))
                    {


                        List<PurchaseList> purchaseLists = purchase.equipList;


                        foreach (var items in purchaseLists)
                        {
                            PurchaseandEquipment purchaseandEquipment = new PurchaseandEquipment();
                            purchaseandEquipment.equipName = items.equipName;
                            purchaseandEquipment.quantity = items.equipQuantity;                            
                            purchaseandEquipment.purchaseAutoId = purchase.purchaseAutoId;
                            purchaseandEquipment.equipAutoId = items.equipAutoId;
                            purchaseandEquipment.companyId = purchase.companyId;
                            _context.purchaseandEquipment.Add(purchaseandEquipment);
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

            //int getPurchaseId = 0;
            //var compId = _context.purchases.Where(i => i.companyId == purchase.companyId).Select(d => d.purchaseId).ToList();
            //var autoId = "";
            //if (compId.Count > 0)
            //{

            //    autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
            //}

            //if (autoId == "")
            //{
            //    _context.ChangeTracker.Clear();
            //    Purchase pur = new Purchase();
            //    string comId = "PR1";
            //    pur.purchaseAutoId = purchase.purchaseAutoId;
            //    pur.purchaseId = comId;
            //    pur.companyId = purchase.companyId;
            //    pur.status = purchase.status;

            //    pur.purchasesDescp=purchase.purchasesDescp;
            //    pur.userAutoId = userAutoId;
            //    _context.purchases.Add(pur);
            //    await _context.SaveChangesAsync();
            //    getPurchaseId = pur.purchaseAutoId;
            //}
            //if (autoId != "")
            //{
            //    _context.ChangeTracker.Clear();
            //    Purchase pur = new Purchase();
            //    string comId = "PR" + (int.Parse(autoId) + 1);
            //    pur.purchaseAutoId = purchase.purchaseAutoId;
            //    pur.purchaseId = comId;
            //    pur.companyId = purchase.companyId;
            //    pur.status = purchase.status;

            //    pur.purchasesDescp=purchase.purchasesDescp;
            //    pur.userAutoId = userAutoId;
            //    _context.purchases.Add(pur);
            //    await _context.SaveChangesAsync();
            //    getPurchaseId = pur.purchaseAutoId;
            //}


            //var listEquipments = purchase.equipList;
            //foreach (var item in listEquipments)
            //{
            //    PurchaseandEquipment purchaseandEquipment = new PurchaseandEquipment();
            //    purchaseandEquipment.equipAutoId = Convert.ToInt32(item);
            //    purchaseandEquipment.purchaseAutoId = getPurchaseId;
            //    purchaseandEquipment.companyId = purchase.companyId;
            //    _context.purchaseandEquipment.Add(purchaseandEquipment);
            //    await _context.SaveChangesAsync();
            //}

            //return Ok();

        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(string id, string companyId)
        {
            var purchase = await _context.purchases.Where(x => x.purchaseId == id && x.companyId == companyId).FirstOrDefaultAsync();

            if (purchase == null)
            {
                return NotFound();
            }

            _context.purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IssuenceExists(int id)
        {
            return _context.purchases.Any(e => e.purchaseAutoId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.purchases.Any(x => x.companyId == id);
        }

        private bool PurchaseValidityExists(int id)
        {
            return _context.purchases.Any(e => e.purchaseAutoId == id);
        }
    }
}
