﻿using System;
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
    public class LinearAssetModelsController : ControllerBase
    {
        private readonly AssetDbContext _context;

        public LinearAssetModelsController(AssetDbContext context)
        {
            _context = context;
        }

        // GET: api/LinearAssetModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LinearAssetModel>>> GetlinearAssetModels()
        {
            return await _context.linearAssetModels.ToListAsync();
        }

        // GET: api/LinearAssetModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LinearAssetModel>> GetLinearAssetModel(int id)
        {
            var linearAssetModel = await _context.linearAssetModels.FindAsync(id);

            if (linearAssetModel == null)
            {
                return NotFound();
            }

            return linearAssetModel;
        }

        // PUT: api/LinearAssetModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLinearAssetModel(int id, LinearAssetModel linearAssetModel)
        {
            if (id != linearAssetModel.laAutoID)
            {
                return BadRequest();
            }

            _context.Entry(linearAssetModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinearAssetModelExists(id))
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

        // POST: api/LinearAssetModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LinearAssetModel>> PostLinearAssetModel(LinearAssetModel linearAssetModel)
        {
            _context.linearAssetModels.Add(linearAssetModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLinearAssetModel", new { id = linearAssetModel.laAutoID }, linearAssetModel);
        }

        // DELETE: api/LinearAssetModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinearAssetModel(int id)
        {
            var linearAssetModel = await _context.linearAssetModels.FindAsync(id);
            if (linearAssetModel == null)
            {
                return NotFound();
            }

            _context.linearAssetModels.Remove(linearAssetModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinearAssetModelExists(int id)
        {
            return _context.linearAssetModels.Any(e => e.laAutoID == id);
        }
    }
}
