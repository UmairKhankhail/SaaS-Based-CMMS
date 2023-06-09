using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using System.Reflection;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinearAssetModelsController : ControllerBase
    {
        private readonly AssetDbContext _context;

        public LinearAssetModelsController(AssetDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetControllersAndMethods")]
        public async Task<List<string>> GetAllControllerMethods()
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

            return methods;
        }

        // GET: api/LinearAssetModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinearAssetModel>>> GetlinearAssetModels(string companyId)
        {

            var result=await _context.linearAssetModels.Join(
                _context.linearSubItems, la => la.laAutoID, lsm => lsm.laAutoId, (la, lsm) => new { la, lsm }).
                Where(x => x.la.laAutoID == x.lsm.laAutoId && x.la.companyId == companyId).
                Select(result => new
                {
                    result.la.laID,
                    result.la.laName,
                    result.la.status,
                    result.lsm.lsName,
                    result.lsm.location,
                    result.lsm.description

                }).ToListAsync();
            return Ok(result);
         }
        // GET: api/LinearAssetModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LinearAssetModel>> GetLinearAssetModel(int id, string companyId)
        {
            
                var linearAsssetModel = await _context.linearAssetModels.Join(
                _context.linearSubItems, la => la.laAutoID, lsm => lsm.laAutoId, (la, lsm) => new { la, lsm }).
                Where(x => x.la.companyId == companyId && x.la.laAutoID==id && x.la.laAutoID == x.lsm.laAutoId).
                Select(result => new
                {   result.la.status,
                    result.lsm.lsName,
                    result.lsm.location,
                    result.lsm.description

                }).ToListAsync();
                return Ok(linearAsssetModel);
    
        }

        // PUT: api/LinearAssetModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutLinearAssetModel(LinearAssetModel linearAssetModel)
        {
            if (linearAssetModel.validityCheck != 0)
            {
                var existingModel = await _context.linearAssetModels.FindAsync(linearAssetModel.validityCheck);

                existingModel.laName = linearAssetModel.laName;
                try
                {
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

        // POST: api/LinearAssetModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinearAssetModel>> PostLinearAssetModel( LinearAssetModel linearAssetModel)
        {
            var getLamAutoId = 0;
            var compId = _context.linearAssetModels.Where(i => i.companyId == linearAssetModel.companyId).Select(d => d.laID).ToList();
            var autoId = "";
            if (compId.Count > 0)
            {

                autoId = compId.Max(x => int.Parse(x.Substring(3))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                LinearAssetModel laModel = new LinearAssetModel();
                string comId = "LAM1";
                laModel.laAutoID= linearAssetModel.laAutoID;
                laModel.laID = comId;
                laModel.laName= linearAssetModel.laName;
                laModel.status= linearAssetModel.status;
                laModel.companyId = linearAssetModel.companyId;
                _context.linearAssetModels.Add(laModel);
                await _context.SaveChangesAsync();
                getLamAutoId = laModel.laAutoID;
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                LinearAssetModel laModel = new LinearAssetModel();
                string comId = "LAM" + (int.Parse(autoId) + 1);
                laModel.laAutoID = linearAssetModel.laAutoID;
                laModel.laID = comId;
                laModel.laName = linearAssetModel.laName;
                laModel.status = linearAssetModel.status;
                laModel.companyId = linearAssetModel.companyId;
                _context.linearAssetModels.Add(laModel);
                await _context.SaveChangesAsync();
                getLamAutoId = laModel.laAutoID;
            }

            List<LinearSubItemList> linearSubItemLists = linearAssetModel.listSubItems;

            foreach (var items in linearSubItemLists)
            {
                LinearSubItem laSubItem = new LinearSubItem();
                laSubItem.laAutoId = getLamAutoId;
                laSubItem.lsName = items.lsName;
                laSubItem.description = items.description;
                laSubItem.location = items.location;
                laSubItem.companyId=linearAssetModel.companyId;
                _context.linearSubItems.Add(laSubItem);
                await _context.SaveChangesAsync();
            }

       

            return Ok();
        }

        // DELETE: api/LinearAssetModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinearAssetModel(int id)
        {
            var linearSubItems = await _context.linearSubItems.FindAsync(id);
            if (linearSubItems == null)
            {
                return NotFound();
            }

            _context.linearSubItems.Remove(linearSubItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinearAssetModelExists(int id)
        {
            return _context.linearAssetModels.Any(e => e.laAutoID == id);
        }
    }
}
