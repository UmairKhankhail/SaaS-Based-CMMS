using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MaintenanceWebApi.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MaintenanceWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrdersController : ControllerBase
    {
        private readonly MaintenanceDbContext _context;

        public WorkOrdersController(MaintenanceDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkOrders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkOrder>>> GetworkOrders(string companyId)
        {
            return await _context.workOrders.Where(x=>x.companyId==companyId).ToListAsync();
        }

        // GET: api/WorkOrders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkOrder>> GetWorkOrder(int id,string companyId)
        {
            var workOrder = await _context.workOrders.Where(x=>x.woAutoId==id && x.companyId==companyId).ToListAsync();

            if (workOrder == null)
            {
                return NotFound();
            }

            return Ok(workOrder);
        }

        // PUT: api/WorkOrders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutWorkOrder(int id,string companyId, WorkOrder workOrder)
        {
            if (workOrder.woAutoId == id && workOrder.companyId==companyId)
            {
                _context.Entry(workOrder).State = EntityState.Modified;
            }

           

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkOrderExists(id))
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

        // POST: api/WorkOrders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkOrder>> PostWorkOrder(WorkOrder workOrder)
        {
            var comId = _context.workOrders.Where(wo => wo.companyId == workOrder.companyId).Select(wo => wo.woId).ToList();

            var autoId = "";
            if (comId.Count > 0)
            {
                autoId = comId.Max(x => int.Parse(x.Substring(2))).ToString();
            }

            if (autoId == "")
            {
                _context.ChangeTracker.Clear();
                WorkOrder wo = new WorkOrder();
                string comid = "WO1";
                wo.woId = comid;
                wo.woType = workOrder.woType;
                wo.topName = workOrder.topName;
                wo.requestId = workOrder.requestId;
                wo.assetDetials = workOrder.assetDetials;
                wo.companyId = workOrder.companyId;
                _context.workOrders.Add(wo);
                await _context.SaveChangesAsync();
            }

            if (autoId != "")
            {
                _context.ChangeTracker.Clear();
                WorkOrder wo = new WorkOrder();
                string comid = "WO" + (int.Parse(autoId) + 1);
                wo.woId = comid;
                wo.woType = workOrder.woType;
                wo.topName = workOrder.topName;
                wo.requestId = workOrder.requestId;
                wo.assetDetials = workOrder.assetDetials;
                wo.companyId = workOrder.companyId;
                _context.workOrders.Add(wo);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        // DELETE: api/WorkOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOrder(int id)
        {
            var workOrder = await _context.workOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }

            _context.workOrders.Remove(workOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkOrderExists(int id)
        {
            return _context.workOrders.Any(e => e.woAutoId == id);
        }
    }
}
