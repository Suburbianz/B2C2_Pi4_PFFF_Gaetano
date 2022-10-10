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
    public class CameraReportsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CameraReportsAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CameraReportsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CameraReport>>> GetCameraReports()
        {
            return await _context.CameraReports
                .ToListAsync();
        }

        // GET: api/CameraReportsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CameraReport>> GetCameraReport(int id)
        {
            var cameraReport = await _context.CameraReports.FindAsync(id);

            if (cameraReport == null)
            {
                return NotFound();
            }

            return cameraReport;
        }


        // PUT: api/CameraReportsAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCameraReport(int id, CameraReport cameraReport)
        {
            if (id != cameraReport.Id)
            {
                return BadRequest();
            }

            _context.Entry(cameraReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CameraReportExists(id))
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


        /*
        // POST: api/CameraReportsAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CameraReport>> PostCameraReport(CameraReport cameraReport)
        {
            _context.CameraReports.Add(cameraReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCameraReport", new { id = cameraReport.Id }, cameraReport);
        }
        */

        // DELETE: api/CameraReportsAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCameraReport(int id)
        {
            var cameraReport = await _context.CameraReports.FindAsync(id);
            if (cameraReport == null)
            {
                return NotFound();
            }

            _context.CameraReports.Remove(cameraReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CameraReportExists(int id)
        {
            return _context.CameraReports.Any(e => e.Id == id);
        }
    }
}
