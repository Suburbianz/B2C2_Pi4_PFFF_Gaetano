using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using B2C2_Pi4_PFFF_Gaetano.Data;
using B2C2_Pi4_PFFF_Gaetano.Models;

namespace B2C2_Pi4_PFFF_Gaetano.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraLocationsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CameraLocationsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CameraLocationsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraLocation>>> GetCameraLocations()
        {
            return await _context.CameraLocations.ToListAsync();
        }

        // GET: api/CameraLocationsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraLocation>> GetCameraLocation(int id)
        {
            var cameraLocation = await _context.CameraLocations.FindAsync(id);

            if (cameraLocation == null)
            {
                return NotFound();
            }

            return cameraLocation;
        }
        /*
        // PUT: api/CameraLocationsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraLocation(int id, CameraLocation cameraLocation)
        {
            if (id != cameraLocation.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraLocationExists(id))
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

        // POST: api/CameraLocationsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CameraLocation>> PostCameraLocation(CameraLocation cameraLocation)
        {
            _context.CameraLocations.Add(cameraLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraLocation", new { id = cameraLocation.Id }, cameraLocation);
        }

        // DELETE: api/CameraLocationsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCameraLocation(int id)
        {
            var cameraLocation = await _context.CameraLocations.FindAsync(id);
            if (cameraLocation == null)
            {
                return NotFound();
            }

            _context.CameraLocations.Remove(cameraLocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CameraLocationExists(int id)
        {
            return _context.CameraLocations.Any(e => e.Id == id);
        }
        */
    }
}
