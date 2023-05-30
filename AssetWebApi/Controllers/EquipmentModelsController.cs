using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssetWebApi.Models;
using JwtAuthenticationManager.Models;
using JwtAuthenticationManager;

namespace AssetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentModelsController : ControllerBase
    {
        private readonly AssetDbContext _context;

        public EquipmentModelsController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: api/EquipmentModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentModel>>> GetequipmentModels()
        {
            return await _context.equipmentModels.ToListAsync();
        }

        // GET: api/EquipmentModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentModel>> GetEquipmentModel(int id)
        {
            var equipmentModel = await _context.equipmentModels.FindAsync(id);

            if (equipmentModel == null)
            {
                return NotFound();
            }

            return equipmentModel;
        }

        // PUT: api/EquipmentModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutEquipmentModel(EquipmentModel equipmentModel)
        {

            if (equipmentModel.validityCheck != 0)
            {
                var existingModel = await _context.equipmentModels.FindAsync(equipmentModel.validityCheck);
                existingModel.eName = equipmentModel.eName;


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

        // POST: api/EquipmentModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EquipmentModel>> PostEquipmentModel(EquipmentModel equipmentModel)
        {

            try
            {
                if (equipmentModel.validityCheck == 0)
                {

                    int getEquipmentAutoId = 0;
                    var compId = _context.equipmentModels.Where(d => d.companyId == equipmentModel.companyId).Select(d => d.eId).ToList();

                    var autoId = "";
                    if (compId.Count > 0)
                    {

                        autoId = compId.Max(x => int.Parse(x.Substring(2))).ToString();
                    }

                    if (autoId == "")
                    {
                        _context.ChangeTracker.Clear();
                        EquipmentModel e = new EquipmentModel();
                        string comId = "EM1";
                        e.eId = comId;
                        e.eName = equipmentModel.eName;
                        e.companyId = equipmentModel.companyId;
                        e.status = equipmentModel.status;
                        _context.equipmentModels.Add(e);
                        await _context.SaveChangesAsync();
                        getEquipmentAutoId = e.eAutoId;
                    }
                    if (autoId != "")
                    {
                        _context.ChangeTracker.Clear();
                        EquipmentModel e = new EquipmentModel();
                        string comId = "EM" + (int.Parse(autoId) + 1);
                        e.eId = comId;
                        e.eName = equipmentModel.eName;
                        e.companyId = equipmentModel.companyId;
                        e.status = equipmentModel.status;
                        _context.equipmentModels.Add(e);
                        await _context.SaveChangesAsync();
                        getEquipmentAutoId = e.eAutoId;
                    }

                    //Console.WriteLine("Id: "+ getroleautoid.ToString());
                    //_context.roles.Add(role);
                    //await _context.SaveChangesAsync();

                    List<EquipmentSubItemsList> equipmentSubItemsLists = equipmentModel.listSubItems;



                    foreach (var items in equipmentSubItemsLists)
                    {
                        EquipmentSubItem eSubItem = new EquipmentSubItem();
                        eSubItem.eAutoId = getEquipmentAutoId;
                        eSubItem.esName = items.esName;
                        eSubItem.esDescription = items.esDescription;
                        eSubItem.esPosition = items.esPosition;
                        eSubItem.esParentId = items.esParentId;
                        eSubItem.companyId = equipmentModel.companyId;
                        _context.equipmentSubItems.Add(eSubItem);
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
                else if(equipmentModel.validityCheck!=0)
                {
                   var validityCheckValue=equipmentModel.validityCheck;
                    if (EquipmentModelValidityExists(validityCheckValue))
                    {

                        List<EquipmentSubItemsList> equipmentSubItemsLists = equipmentModel.listSubItems;



                        foreach (var items in equipmentSubItemsLists)
                        {
                            EquipmentSubItem eSubItem = new EquipmentSubItem();
                            eSubItem.eAutoId = validityCheckValue;
                            eSubItem.esName = items.esName;
                            eSubItem.esDescription = items.esDescription;
                            eSubItem.esPosition = items.esPosition;
                            eSubItem.esParentId = items.esParentId;
                            eSubItem.companyId = equipmentModel.companyId;
                            _context.equipmentSubItems.Add(eSubItem);
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
            //_context.equipmentModels.Add(equipmentModel);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEquipmentModel", new { id = equipmentModel.eAutoId }, equipmentModel);
        }

        // DELETE: api/EquipmentModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipmentModel(int id)
        {
            if (id != null)
            {
                var equipmentSubItemModel = await _context.equipmentSubItems.FindAsync(id);
                if (equipmentSubItemModel == null)
                {
                    return NotFound();
                }

                _context.equipmentSubItems.Remove(equipmentSubItemModel);
                await _context.SaveChangesAsync();
            }

            return NoContent();
        }

        private bool EquipmentModelExists(int id)
        {
            return _context.equipmentModels.Any(e => e.eAutoId == id);
        }

        private bool EquipmentModelValidityExists(int id)
        {
            return _context.equipmentModels.Any(e => e.eAutoId == id);
        }
    }
}
