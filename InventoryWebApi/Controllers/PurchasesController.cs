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
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        private readonly JwtTokenHandler _JwtTokenHandler;
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
        public async Task<ActionResult<IEnumerable<Purchase>>> Getpurchases()
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    return await _context.purchases.Where(x => x.companyId ==claimresponse.companyId && x.status == "Active").ToListAsync();
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        // GET: api/Purchases/5
        [HttpGet("id")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var getPurchaseRequests = await _context.purchases
                    .Join(_context.purchaseandEquipment, i => i.purchaseAutoId, ie => ie.purchaseAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == claimresponse.companyId && x.i.companyId == claimresponse.companyId && x.i.purchaseAutoId == id)
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
                return Unauthorized();
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
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
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
                    
                            if (equipment.equipAutoId != null)

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

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    int getPurchaseAutoId = 0;

                    if (purchase.validityCheck == 0)
                    {

                        var compId = _context.purchases.Where(d => d.companyId == claimresponse.companyId).Select(d => d.purchaseId).ToList();

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
                            pur.companyId = claimresponse.companyId;
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
                            pur.companyId = claimresponse.companyId;
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
                            purchaseandEquipment.companyId = claimresponse.companyId;
                            _context.purchaseandEquipment.Add(purchaseandEquipment);
                            await _context.SaveChangesAsync();
                        }
                  
                    }

                    else if (purchase.validityCheck != 0 && purchase.status == "pending")
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
                                purchaseandEquipment.companyId = claimresponse.companyId;
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

                   
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(string id)
        {
            try
            {
                var accessToken = Request.Headers.Authorization.FirstOrDefault()?.Replace("Bearer ", "");
                var claimresponse = _JwtTokenHandler.GetCustomClaims(new ClaimRequest { token = accessToken, controllerActionName = RouteData.Values["controller"] + "Controller." + base.ControllerContext.ActionDescriptor.ActionName });
                if (claimresponse.isAuth == true)
                {

                    var purchase = await _context.purchases.Where(x => x.purchaseId == id && x.companyId == claimresponse.companyId).FirstOrDefaultAsync();

                    if (purchase == null)
                    {
                        return NotFound();
                    }

                    _context.purchases.Remove(purchase);
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
