using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore.Metadata;

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
        public async Task<ActionResult<Purchase>> GetPurchase(string companyId, string purchaseId)
        {
            try
            {

                var getPurchaseRequests = await _context.purchases
                    .Join(_context.purchaseandEquipment, i => i.purchaseAutoId, ie => ie.purchaseAutoId, (i, ie) => new { i, ie })
                    .Where(x => x.ie.companyId == companyId && x.i.companyId == companyId && x.i.purchaseId == purchaseId)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(string id, string companyId, Purchase purchase, int userAutoId)
        {
            if (IssuenceExists(id) && CompanyExists(companyId))
            {
                Purchase pur = new Purchase();
                pur.purchaseAutoId = purchase.purchaseAutoId;
                pur.purchaseId = purchase.purchaseId;
                pur.companyId = purchase.companyId;
                pur.status = purchase.status;
                pur.qty = purchase.qty;
                pur.purchasesDescp=purchase.purchasesDescp;
                pur.userAutoId = userAutoId;

                var listEquipments = purchase.equipList;
                var listDbEquipments = new List<string>();
                var getEquipments = _context.purchaseandEquipment.Where(x => x.purchaseAutoId == purchase.purchaseAutoId && x.companyId == purchase.companyId).Select(x => x.equipAutoId);

                foreach (var item in getEquipments)
                {
                    listDbEquipments.Add(item.ToString());
                    Console.WriteLine("DB Equipments: " + item);
                }

                foreach (var equip in listEquipments)
                {
                    Console.WriteLine("New Equipments: " + equip);

                }

                var resultListLeft = listDbEquipments.Except(listEquipments).ToList();
                var resultListRight = listEquipments.Except(listDbEquipments).ToList();

                var rll = new List<string>();
                var rlr = new List<string>();
                if (resultListLeft != null)
                {
                    foreach (var x in resultListLeft)
                    {
                        rll.Add(x);
                    }
                }
                if (resultListRight != null)
                {
                    foreach (var x in resultListRight)
                    {
                        rlr.Add(x);
                    }
                }

                if (rll != null)
                {
                    foreach (var item in rll)
                    {
                        var delUser = _context.purchaseandEquipment.Where(x => x.companyId == companyId && x.purchaseAutoId == purchase.purchaseAutoId && x.equipAutoId == int.Parse(item)).FirstOrDefault();
                        if (delUser == null)
                        {
                            return NotFound();
                        }
                        _context.purchaseandEquipment.Remove(delUser);
                    }
                }

                if (rlr != null)
                {
                    foreach (var item in rlr)
                    {
                        PurchaseandEquipment purchaseEquipment = new PurchaseandEquipment();
                        purchaseEquipment.purchaseAutoId = purchase.purchaseAutoId;
                        purchaseEquipment.equipAutoId = int.Parse(item);
                        purchaseEquipment.companyId = purchase.companyId;
                        _context.purchaseandEquipment.Add(purchaseEquipment);
                    }
                }


                _context.Entry(pur).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();

        }

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(int userAutoId, string companyId, Purchase purchase)
        {
            int getPurchaseId = 0;
            var compId = _context.purchases.Where(i => i.companyId == purchase.companyId).Select(d => d.purchaseId).ToList();
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
                pur.qty = purchase.qty;
                pur.purchasesDescp=purchase.purchasesDescp;
                pur.userAutoId = userAutoId;
                _context.purchases.Add(pur);
                await _context.SaveChangesAsync();
                getPurchaseId = pur.purchaseAutoId;
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
                pur.qty = purchase.qty;
                pur.purchasesDescp=purchase.purchasesDescp;
                pur.userAutoId = userAutoId;
                _context.purchases.Add(pur);
                await _context.SaveChangesAsync();
                getPurchaseId = pur.purchaseAutoId;
            }


            var listEquipments = purchase.equipList;
            foreach (var item in listEquipments)
            {
                PurchaseandEquipment purchaseandEquipment = new PurchaseandEquipment();
                purchaseandEquipment.equipAutoId = Convert.ToInt32(item);
                purchaseandEquipment.purchaseAutoId = getPurchaseId;
                purchaseandEquipment.companyId = purchase.companyId;
                _context.purchaseandEquipment.Add(purchaseandEquipment);
                await _context.SaveChangesAsync();
            }

            return Ok();

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

        private bool IssuenceExists(string id)
        {
            return _context.purchases.Any(e => e.purchaseId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.purchases.Any(x => x.companyId == id);
        }
    }
}
