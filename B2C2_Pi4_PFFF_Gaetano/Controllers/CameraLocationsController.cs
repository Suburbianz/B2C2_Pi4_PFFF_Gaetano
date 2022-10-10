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
    public class CameraLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CameraLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CameraLocations
        public async Task<IActionResult> Index()
        {
              return View(await _context.CameraLocations.ToListAsync());
        }

        // GET: CameraLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CameraLocations == null)
            {
                return NotFound();
            }

            var cameraLocation = await _context.CameraLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cameraLocation == null)
            {
                return NotFound();
            }

            return View(cameraLocation);
        }

        // GET: CameraLocations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CameraLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedOn,StreetName,HouseNumber,HouseNumberAddition,ZipCode,City,Region")] CameraLocation cameraLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cameraLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cameraLocation);
        }

        // GET: CameraLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CameraLocations == null)
            {
                return NotFound();
            }

            var cameraLocation = await _context.CameraLocations.FindAsync(id);
            if (cameraLocation == null)
            {
                return NotFound();
            }
            return View(cameraLocation);
        }

        // POST: CameraLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedOn,StreetName,HouseNumber,HouseNumberAddition,ZipCode,City,Region")] CameraLocation cameraLocation)
        {
            if (id != cameraLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cameraLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CameraLocationExists(cameraLocation.Id))
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
            return View(cameraLocation);
        }

        // GET: CameraLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CameraLocations == null)
            {
                return NotFound();
            }

            var cameraLocation = await _context.CameraLocations
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cameraLocation == null)
            {
                return NotFound();
            }

            return View(cameraLocation);
        }

        // POST: CameraLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CameraLocations == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CameraLocations'  is null.");
            }
            var cameraLocation = await _context.CameraLocations.FindAsync(id);
            if (cameraLocation != null)
            {
                _context.CameraLocations.Remove(cameraLocation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CameraLocationExists(int id)
        {
          return _context.CameraLocations.Any(e => e.Id == id);
        }
    }
}
