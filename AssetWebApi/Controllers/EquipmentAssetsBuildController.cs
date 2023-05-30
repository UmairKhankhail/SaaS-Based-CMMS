using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using NuGet.ContentModel;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentAssetsBuildController : ControllerBase
    {
        private readonly AssetDbContext _context;

        public EquipmentAssetsBuildController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentAssetsBuild
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentAsset>>> GetequipmentAssets()
        {
            return await _context.equipmentAssets.ToListAsync();
        }

        // GET: api/EquipmentAssetsBuild/5
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

        // PUT: api/EquipmentAssetsBuild/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEquipmentAsset(EquipmentAsset equipmentAsset)
        {
            var existingModel = await _context.equipmentAssets.FindAsync(equipmentAsset.eAssetAuotoId);

            
            existingModel.eAssetId = equipmentAsset.eAssetId;
            existingModel.eAutoId = equipmentAsset.eAutoId;
            existingModel.eAssetName = equipmentAsset.eAssetName;
            existingModel.brandNmae = equipmentAsset.brandNmae;
            existingModel.code = equipmentAsset.code;
            existingModel.description = equipmentAsset.description;
            existingModel.deptId = equipmentAsset.deptId;
            existingModel.subDeptId = equipmentAsset.subDeptId;
            existingModel.flId = equipmentAsset.flId;
            existingModel.companyId = equipmentAsset.companyId;

            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

            return NoContent();
        }

        // POST: api/EquipmentAssetsBuild
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EquipmentAsset>> PostEquipmentAsset(EquipmentAsset equipmentAsset)
        {
           
            int getEquipmentAutoId = 0;
            var compId = _context.equipmentAssets.Where(d => d.companyId == equipmentAsset.companyId).Select(d => d.eAssetId).ToList();

            var autoId = "";
            if (compId.Count > 0)
            {
                autoId = compId.Max(x => {
                int startIndex = x.IndexOf("AAE") + 3;
                string numbers = x.Substring(startIndex).TrimStart('0');
                return int.Parse(numbers);
            }).ToString();

            }

            if (autoId == "")
            {
                //B1F1L1D1S1
                _context.ChangeTracker.Clear();
                EquipmentAsset e = new EquipmentAsset();
                string comId = equipmentAsset.flId + "AAE1";
                e.eAssetId = comId;
                e.eAutoId = equipmentAsset.eAutoId;
                e.eAssetName = equipmentAsset.eAssetName;
                e.brandNmae = equipmentAsset.brandNmae;
                e.code = equipmentAsset.code;
                e.description = equipmentAsset.description;
                e.deptId = equipmentAsset.deptId;
                e.subDeptId = equipmentAsset.subDeptId;
                e.flId = equipmentAsset.flId;
                e.companyId = equipmentAsset.companyId;
                _context.equipmentAssets.Add(e);
                await _context.SaveChangesAsync();
                getEquipmentAutoId = e.eAssetAuotoId;
            }
            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                EquipmentAsset e = new EquipmentAsset();
                string comId = equipmentAsset.flId + "AAE"+ (int.Parse(autoId) + 1);
                e.eAssetId = comId;
                e.eAutoId = equipmentAsset.eAutoId;
                e.eAssetName = equipmentAsset.eAssetName;
                e.brandNmae = equipmentAsset.brandNmae;
                e.code = equipmentAsset.code;
                e.description = equipmentAsset.description;
                e.deptId = equipmentAsset.deptId;
                e.subDeptId = equipmentAsset.subDeptId;
                e.flId = equipmentAsset.flId;
                e.companyId = equipmentAsset.companyId;
                _context.equipmentAssets.Add(e);
                await _context.SaveChangesAsync();
                getEquipmentAutoId = e.eAutoId;
            }
            return Ok();
            //_context.equipmentAssets.Add(equipmentAsset);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEquipmentAsset", new { id = equipmentAsset.eAssetAuotoId }, equipmentAsset);
        }

        // DELETE: api/EquipmentAssetsBuild/5
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
