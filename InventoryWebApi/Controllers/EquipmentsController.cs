using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryAPI.Models;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentsController : ControllerBase
    {
        private readonly InventoryDbContext _context;
        //private readonly JwtTokenHandler _JwtTokenHandler;
        private readonly HttpClient _httpClient;
        private readonly ILogger<EquipmentsController> _logger;


        public EquipmentsController(InventoryDbContext context, HttpClient httpClient, ILogger<EquipmentsController> logger)
        {
            _context = context;
            _httpClient = httpClient;
            _logger = logger;
        }

        // GET: api/Equipments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> Getequipments(string companyId)
        {
            try
            {

                return await _context.equipments.Where(x => x.companyId == companyId && x.status == "Active").ToListAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        // GET: api/Equipments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(string id,string companyId)
        {
            var equipment = await _context.equipments.Where(x => x.equipId == id && x.companyId == companyId && x.status == "Active").FirstOrDefaultAsync();


            if (equipment == null)
            {
                return NotFound();
            }

            return equipment;
        }

        // PUT: api/Equipments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(string id, string companyId, Equipment equipment)
        {
            if (EquipmentExists(id) && CompanyExists(companyId))
            {
                Equipment equip = new Equipment();
                equip.equipAutoId = equipment.equipAutoId;
                equip.equipId = equipment.equipId;
                equip.equipName = equipment.equipName;
                equip.equipCost = equipment.equipCost;
                equip.equipLeadTime = equipment.equipLeadTime;
                equip.status = equipment.status;
                equip.catAutoId= equipment.catAutoId;
                equip.groupAutoId = equipment.groupAutoId;
                equip.companyId = equipment.companyId;

                _context.Entry(equip).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }

            return NotFound();

        }

        // POST: api/Equipments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(Equipment equipment,string companyId)
        {
            var comId = _context.equipments.Where(equip => equip.companyId == companyId).Select(equip => equip.equipId).ToList();

            Console.WriteLine(comId);
            var autoId = "";
            if (comId.Count > 0)
            {
                autoId = comId.Max(x => int.Parse(x.Substring(1))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                Equipment equip = new Equipment();
                equip.equipAutoId = equipment.equipAutoId;
                string comid = "E1";
                equip.equipId = comid;
                equip.equipName = equipment.equipName;
                equip.equipCost = equipment.equipCost;
                equip.equipLeadTime = equipment.equipLeadTime;
                equip.status = equipment.status;
                equip.catAutoId = equipment.catAutoId;
                equip.groupAutoId = equipment.groupAutoId;
                equip.companyId = equipment.companyId;

                _context.equipments.Add(equip);
                await _context.SaveChangesAsync();
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                Equipment equip = new Equipment();
                equip.equipAutoId = equipment.equipAutoId;
                string comid = "E1" + (int.Parse(autoId) + 1);
                equip.equipId = comid;
                equip.equipName = equipment.equipName;
                equip.equipCost = equipment.equipCost;
                equip.equipLeadTime = equipment.equipLeadTime;
                equip.status = equipment.status;
                equip.catAutoId = equipment.catAutoId;
                equip.groupAutoId = equipment.groupAutoId;
                equip.companyId = equipment.companyId;

                _context.equipments.Add(equip);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // DELETE: api/Equipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(string id,string companyId)
        {
            var equipment = await _context.equipments.Where(x => x.equipId == id && x.companyId == companyId).FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            _context.equipments.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EquipmentExists(string id)
        {
            return _context.equipments.Any(e => e.equipId == id);
        }

        private bool CompanyExists(string id)
        {
            return _context.brands.Any(x => x.companyId == id);
        }
    }
}
