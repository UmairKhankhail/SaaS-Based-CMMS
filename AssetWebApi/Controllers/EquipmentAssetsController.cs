using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentAssetsController : ControllerBase
    {
        private readonly AssetDbContext _context;

        public EquipmentAssetsController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentAssets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentAsset>>> GetequipmentAssets()
        {
            return await _context.equipmentAssets.ToListAsync();
        }

        // GET: api/EquipmentAssets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentAsset>> GetEquipmentAsset(int id)
        {
            var equipmentAsset = await _context.equipmentAssets.FindAsync(id);

            if (equipmentAsset == null)
            {
                return NotFound();
            }

            return equipmentAsset;
        }

        // PUT: api/EquipmentAssets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipmentAsset(int id, EquipmentAsset equipmentAsset)
        {
            if (id != equipmentAsset.eAssetAuotoId)
            {
                return BadRequest();
            }

            _context.Entry(equipmentAsset).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipmentAssetExists(id))
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

        // POST: api/EquipmentAssets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EquipmentAsset>> PostEquipmentAsset(EquipmentAsset equipmentAsset)
        {
            _context.equipmentAssets.Add(equipmentAsset);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquipmentAsset", new { id = equipmentAsset.eAssetAuotoId }, equipmentAsset);
        }

        // DELETE: api/EquipmentAssets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentAsset(int id)
        {
            var equipmentAsset = await _context.equipmentAssets.FindAsync(id);
            if (equipmentAsset == null)
            {
                return NotFound();
            }

            _context.equipmentAssets.Remove(equipmentAsset);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentAssetExists(int id)
        {
            return _context.equipmentAssets.Any(e => e.eAssetAuotoId == id);
        }
    }
}
