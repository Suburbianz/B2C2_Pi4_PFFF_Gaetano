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
    public class AppUserAchievementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppUserAchievementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppUserAchievements
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AppUserAchievement.Include(a => a.Achievement).Include(a => a.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AppUserAchievements/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.AppUserAchievement == null)
            {
                return NotFound();
            }

            var appUserAchievement = await _context.AppUserAchievement
                .Include(a => a.Achievement)
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.AppUserId == id);
            if (appUserAchievement == null)
            {
                return NotFound();
            }

            return View(appUserAchievement);
        }

        // GET: AppUserAchievements/Create
        public IActionResult Create()
        {
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Content");
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: AppUserAchievements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,AchievementId")] AppUserAchievement appUserAchievement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUserAchievement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Content", appUserAchievement.AchievementId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", appUserAchievement.AppUserId);
            return View(appUserAchievement);
        }

        // GET: AppUserAchievements/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AppUserAchievement == null)
            {
                return NotFound();
            }

            var appUserAchievement = await _context.AppUserAchievement.FindAsync(id);
            if (appUserAchievement == null)
            {
                return NotFound();
            }
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Content", appUserAchievement.AchievementId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", appUserAchievement.AppUserId);
            return View(appUserAchievement);
        }

        // POST: AppUserAchievements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AppUserId,AchievementId")] AppUserAchievement appUserAchievement)
        {
            if (id != appUserAchievement.AppUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUserAchievement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserAchievementExists(appUserAchievement.AppUserId))
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
            ViewData["AchievementId"] = new SelectList(_context.Achievements, "Id", "Content", appUserAchievement.AchievementId);
            ViewData["AppUserId"] = new SelectList(_context.AppUsers, "Id", "Id", appUserAchievement.AppUserId);
            return View(appUserAchievement);
        }

        // GET: AppUserAchievements/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AppUserAchievement == null)
            {
                return NotFound();
            }

            var appUserAchievement = await _context.AppUserAchievement
                .Include(a => a.Achievement)
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.AppUserId == id);
            if (appUserAchievement == null)
            {
                return NotFound();
            }

            return View(appUserAchievement);
        }

        // POST: AppUserAchievements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AppUserAchievement == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AppUserAchievement'  is null.");
            }
            var appUserAchievement = await _context.AppUserAchievement.FindAsync(id);
            if (appUserAchievement != null)
            {
                _context.AppUserAchievement.Remove(appUserAchievement);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserAchievementExists(string id)
        {
          return _context.AppUserAchievement.Any(e => e.AppUserId == id);
        }
    }
}
