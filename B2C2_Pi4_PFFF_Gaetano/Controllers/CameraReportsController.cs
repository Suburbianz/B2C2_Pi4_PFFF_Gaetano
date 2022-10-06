using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using B2C2_Pi4_PFFF_Gaetano.Data;
using B2C2_Pi4_PFFF_Gaetano.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.AspNetCore.Components.Forms;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;

namespace B2C2_Pi4_PFFF_Gaetano.Controllers
{
    public class CameraReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        AppUser? _currentAppUser;
        int scenario;





        public CameraReportsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CameraReports
        public async Task<List<CameraReport>> GetCameraReports()
        {
            var cameraReports = await _context.CameraReports.Include(c => c.AppUser).Include(c => c.Camera).ToListAsync();
            return cameraReports;
        }

        // GET: CameraLocations
        public async Task<List<CameraLocation>> GetCameraLocations()
        {
            var cameraLocations = await _context.CameraLocations.Include(c => c.Cameras).ToListAsync();
            return cameraLocations;
        }

        // GET: Cameras
        public async Task<List<Camera>> GetCameras()
        {
            var cameras = await _context.Cameras.Include(c => c.CameraLocation).Include(c => c.CameraReports).ToListAsync();
            return cameras;
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
                .Include(c => c.Camera.CameraLocation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cameraReport == null)
            {
                return NotFound();
            }

            return View(cameraReport);
        }

        // GET: CameraReports/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: CameraReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DescriptionRemark")] CameraReport cameraReport)
        {
            _currentAppUser = await _userManager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                var locations = await GetCameraLocations();
                var cameraForReport = await SearchCameraForReport();
                var cameraLocationForReport = SearchCameraLocationForReport(locations);
                if (locations.Count == 0 || (cameraForReport == null && cameraLocationForReport == null))
                {
                    scenario = 1;
                    cameraReport.Camera = AddNewCamera();
                    cameraReport.Camera.CameraLocation = AddNewCameraLocation();
                    return FillReportAndSave(cameraReport, _context);
                }
                else if (cameraForReport == null && cameraLocationForReport != null)
                {
                    scenario = 2;
                    cameraReport.Camera = AddNewCamera();
                    cameraReport.Camera.CameraLocation = cameraLocationForReport;
                    return FillReportAndSave(cameraReport, _context);
                }
                else if(cameraForReport != null && cameraLocationForReport != null)
                {
                    cameraReport.Camera = cameraForReport;
                    cameraReport.Camera.CameraLocation = cameraLocationForReport;
                    return FillReportAndSave(cameraReport, _context);
                }
                else if(cameraForReport != null && cameraLocationForReport == null)
                {
                    // Error
                }
            }
            return View(cameraReport);
        }

        public async Task<Camera?> SearchCameraForReport()
        {
            var cameras = await GetCameras();
            var cameraForReport = cameras.FirstOrDefault(c =>
                c.Name == Request.Form["Camera.Name"] &&
                c.ModelNumber == Request.Form["Camera.ModelNumber"] &&
                c.SerialNumber == Request.Form["Camera.SerialNumber"]);
            return cameraForReport;
        }

        public CameraLocation? SearchCameraLocationForReport(List<CameraLocation> locations)
        {
            var cameraLocationForReport = locations.FirstOrDefault(l =>
                l.StreetName == Request.Form["Camera.CameraLocation.StreetName"] &&
                l.HouseNumber == Int32.Parse(Request.Form["Camera.CameraLocation.HouseNumber"]) && (
                l.HouseNumberAddition == Request.Form["Camera.CameraLocation.HouseNumberAddition"] || l.HouseNumberAddition == null) &&
                l.ZipCode == Request.Form["Camera.CameraLocation.ZipCode"] &&
                l.City == Request.Form["Camera.CameraLocation.City"] &&
                l.Region == Request.Form["Camera.CameraLocation.Region"]);
            return cameraLocationForReport;
        }

        public CameraLocation AddNewCameraLocation()
        {
            CameraLocation cameraLocation = new CameraLocation();

            cameraLocation.StreetName = Request.Form["Camera.CameraLocation.StreetName"];
            cameraLocation.HouseNumber = Int32.Parse(Request.Form["Camera.CameraLocation.HouseNumber"]);
            if(Request.Form["Camera.CameraLocation.HouseNumberAddition"] != "")
            {
                cameraLocation.HouseNumberAddition = Request.Form["Camera.CameraLocation.HouseNumberAddition"];
            }
            cameraLocation.ZipCode = Request.Form["Camera.CameraLocation.ZipCode"];
            cameraLocation.City = Request.Form["Camera.CameraLocation.City"];
            cameraLocation.Region = Request.Form["Camera.CameraLocation.Region"];

            return cameraLocation;
        }

        public Camera AddNewCamera()
        {
            Camera camera = new Camera();

            camera.Name = Request.Form["Camera.Name"];
            camera.ModelNumber = Request.Form["Camera.ModelNumber"];
            camera.SerialNumber = Request.Form["Camera.SerialNumber"];

            return camera;
        }

        public RedirectToActionResult FillReportAndSave(CameraReport cameraReport, DbContext dbContext)
        {
            cameraReport.CameraId = cameraReport.Camera.Id;
            cameraReport.Camera.CameraLocationId = cameraReport.Camera.CameraLocation.Id;
            cameraReport.AppUserId = _currentAppUser.Id;
            cameraReport.AppUser = _currentAppUser;
            if(scenario == 1)
            {
                _context.Add(cameraReport.Camera);
                _context.Add(cameraReport.Camera.CameraLocation);
            }
            if(scenario == 2)
            {
                _context.Add(cameraReport.Camera);
            }
            _context.Add(cameraReport);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
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
