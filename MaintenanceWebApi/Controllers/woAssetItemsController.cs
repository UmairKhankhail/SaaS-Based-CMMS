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
    public class woAssetItemsController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public woAssetItemsController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/woAssetItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<woAssetItem>>> GetwoAssetItems(string companyId)
        {
            return await _context.woAssetItems.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/woAssetItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<woAssetItem>> GetwoAssetItem(int id,string companyId)
        {
            var woAssetItem = await _context.woAssetItems.Where(x=>x.woAssetItemAutoId==id && x.companyId==companyId).ToListAsync();

            if (woAssetItem == null)
            {
                return NotFound();
            }

            return Ok(woAssetItem);
        }

        // PUT: api/woAssetItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutwoAssetItem(int id,string companyId, woAssetItem woAssetItem)
        {
            if (woAssetItem.woAssetItemAutoId==id && woAssetItem.companyId==companyId)
            {
                _context.Entry(woAssetItem).State = EntityState.Modified;

            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!woAssetItemExists(id))
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

        // POST: api/woAssetItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<woAssetItem>> PostwoAssetItem(woAssetItem woAssetItem)
        {
            _context.woAssetItems.Add(woAssetItem);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/woAssetItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletewoAssetItem(int id)
        {
            var woAssetItem = await _context.woAssetItems.FindAsync(id);
            if (woAssetItem == null)
            {
                return NotFound();
            }

            _context.woAssetItems.Remove(woAssetItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool woAssetItemExists(int id)
        {
            return _context.woAssetItems.Any(e => e.woAssetItemAutoId == id);
        }
    }
}
