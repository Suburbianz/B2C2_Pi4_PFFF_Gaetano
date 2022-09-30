using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using B2C2_Pi4_PFFF_Gaetano.Data;
using B2C2_Pi4_PFFF_Gaetano.Models;

namespace B2C2_Pi4_PFFF_Gaetano.Controllers
{
    public class CameraReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CameraReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CameraReports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CameraReports.Include(c => c.AppUser).Include(c => c.Camera);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CameraReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CameraReports == null)
            {
                return NotFound();
            }

            var cameraReport = await _context.CameraReports
                .Include(c => c.AppUser)
                .Include(c => c.Camera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cameraReport == null)
            {
                return NotFound();
            }

            return View(cameraReport);
        }

        // GET: CameraReports/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "ModelNumber");
            return View();
        }

        // POST: CameraReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedOn,DescriptionRemark,CameraId,AppUserId")] CameraReport cameraReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cameraReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", cameraReport.AppUserId);
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "ModelNumber", cameraReport.CameraId);
            return View(cameraReport);
        }

        // GET: CameraReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CameraReports == null)
            {
                return NotFound();
            }

            var cameraReport = await _context.CameraReports.FindAsync(id);
            if (cameraReport == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", cameraReport.AppUserId);
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "ModelNumber", cameraReport.CameraId);
            return View(cameraReport);
        }

        // POST: CameraReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedOn,DescriptionRemark,CameraId,AppUserId")] CameraReport cameraReport)
        {
            if (id != cameraReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cameraReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraReportExists(cameraReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", cameraReport.AppUserId);
            ViewData["CameraId"] = new SelectList(_context.Cameras, "Id", "ModelNumber", cameraReport.CameraId);
            return View(cameraReport);
        }

        // GET: CameraReports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CameraReports == null)
            {
                return NotFound();
            }

            var cameraReport = await _context.CameraReports
                .Include(c => c.AppUser)
                .Include(c => c.Camera)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cameraReport == null)
            {
                return NotFound();
            }

            return View(cameraReport);
        }

        // POST: CameraReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CameraReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CameraReports'  is null.");
            }
            var cameraReport = await _context.CameraReports.FindAsync(id);
            if (cameraReport != null)
            {
                _context.CameraReports.Remove(cameraReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CameraReportExists(int id)
        {
          return _context.CameraReports.Any(e => e.Id == id);
        }
    }
}
