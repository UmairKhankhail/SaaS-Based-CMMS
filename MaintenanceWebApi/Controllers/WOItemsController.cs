using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WOItemsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public WOItemsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/WOItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WOItems>>> GetwOItems(string companyId)
        {
            return await _context.wOItems.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/WOItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WOItems>> GetWOItems(int id,string companyId)
        {
            var wOItems = await _context.wOItems.Where(x=>x.woItemsAutoId==id && x.companyId==companyId).ToListAsync();

            if (wOItems == null)
            {
                return NotFound();
            }

            return Ok(wOItems);
        }

        // PUT: api/WOItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWOItems(int id,string companyId, WOItems wOItems)
        {
            if (wOItems.woItemsAutoId==id && wOItems.companyId==companyId)
            {
                _context.Entry(wOItems).State = EntityState.Modified;

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WOItemsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WOItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WOItems>> PostWOItems(WOItems wOItems)
        {
            int getItemsId = 0;
            var compId = _context.wOItems.Where(i => i.companyId == wOItems.companyId).Select(d => d.woItemsId).ToList();
            var autoId = "";
            if (compId.Count > 0)
            {

                autoId = compId.Max(x => int.Parse(x.Substring(1))).ToString();
            }

            if (autoId == "")
            {

                _context.ChangeTracker.Clear();
                WOItems items = new WOItems();
                string comId = "I1";
                items.woItemsAutoId = wOItems.woItemsAutoId;
                items.woItemsId = comId;
                items.woAutoId = wOItems.woAutoId;
                items.quantity = wOItems.quantity;
                items.stock = wOItems.stock;
                items.cost= wOItems.cost;
                items.requestStatus = wOItems.requestStatus;
                items.companyId = wOItems.companyId;
                _context.wOItems.Add(items);
                await _context.SaveChangesAsync();
                getItemsId = items.woItemsAutoId;
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                WOItems items = new WOItems();
                string comId = "I" + (int.Parse(autoId) + 1);
                items.woItemsAutoId = wOItems.woItemsAutoId;
                items.woItemsId = comId;
                items.woAutoId = wOItems.woAutoId;
                items.quantity = wOItems.quantity;
                items.stock = wOItems.stock;
                items.cost = wOItems.cost;
                items.requestStatus = wOItems.requestStatus;
                items.companyId = wOItems.companyId;
                _context.wOItems.Add(items);
                await _context.SaveChangesAsync();
                getItemsId = items.woItemsAutoId;
            }

            return Ok();

        }

        // DELETE: api/WOItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWOItems(int id)
        {
            var wOItems = await _context.wOItems.FindAsync(id);
            if (wOItems == null)
            {
                return NotFound();
            }

            _context.wOItems.Remove(wOItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WOItemsExists(int id)
        {
            return _context.wOItems.Any(e => e.woItemsAutoId == id);
        }
    }
}
