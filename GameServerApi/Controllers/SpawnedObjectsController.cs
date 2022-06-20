using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServerApi.Data;
using GameServerApi.Models;

namespace GameServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpawnedObjectsController : ControllerBase
    {
        private readonly DataContext _context;

        public SpawnedObjectsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/SpawnedObjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SpawnedObjects>>> GetSpawnedObjects()
        {
          if (_context.SpawnedObjects == null)
          {
              return NotFound();
          }
            return await _context.SpawnedObjects.ToListAsync();
        }

        // GET: api/SpawnedObjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SpawnedObjects>> GetSpawnedObjects(int id)
        {
          if (_context.SpawnedObjects == null)
          {
              return NotFound();
          }
            var spawnedObjects = await _context.SpawnedObjects.FindAsync(id);

            if (spawnedObjects == null)
            {
                return NotFound();
            }

            return spawnedObjects;
        }

        [HttpGet]
        [Route("{id}/user")]
        public async Task<ActionResult<IEnumerable<SpawnedObjects>>> GetUserObjects(int id)
        {


            if (_context.SpawnedObjects.Where(c => c.UserId == id).ToList() == null)
            {
                return NotFound();
            }
            var spawnedObjects = await _context.SpawnedObjects.Where(c => c.UserId == id).ToListAsync();

            if (spawnedObjects == null)
            {
                return NotFound();
            }

            return spawnedObjects;
        }

        // PUT: api/SpawnedObjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpawnedObjects(int id, SpawnedObjects spawnedObjects)
        {
            if (id != spawnedObjects.Id)
            {
                return BadRequest();
            }

            _context.Entry(spawnedObjects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpawnedObjectsExists(id))
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

        // POST: api/SpawnedObjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SpawnedObjects>> PostSpawnedObjects(SpawnedObjects spawnedObjects)
        {
          if (_context.SpawnedObjects == null)
          {
              return Problem("Entity set 'DataContext.SpawnedObjects'  is null.");
          }
            _context.SpawnedObjects.Add(spawnedObjects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpawnedObjects", new { id = spawnedObjects.Id }, spawnedObjects);
        }

        // DELETE: api/SpawnedObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpawnedObjects(int id)
        {
            if (_context.SpawnedObjects == null)
            {
                return NotFound();
            }
            var spawnedObjects = await _context.SpawnedObjects.FindAsync(id);
            if (spawnedObjects == null)
            {
                return NotFound();
            }

            _context.SpawnedObjects.Remove(spawnedObjects);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpawnedObjectsExists(int id)
        {
            return (_context.SpawnedObjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
