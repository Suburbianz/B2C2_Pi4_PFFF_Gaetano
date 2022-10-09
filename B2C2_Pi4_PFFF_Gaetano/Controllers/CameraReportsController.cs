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
using B2C2_Pi4_PFFF_Gaetano.ViewModels;

namespace B2C2_Pi4_PFFF_Gaetano.Controllers
{
    public class CameraReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        AppUser? _currentAppUser;
        int scenario;





        public CameraReportsController(ApplicationDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [TempData]
        public string StatusMessage { get; set; }

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
        [Authorize]
        public async Task<IActionResult> Index()
        {
            _currentAppUser = await _userManager.GetUserAsync(User);
            var applicationDbContext = _context.CameraReports.Where(c => c.AppUser == _currentAppUser).Include(c => c.AppUser).Include(c => c.Camera).Include(c => c.Camera.CameraLocation);
            //var userReports = _currentAppUser.CameraReports
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CameraReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            _currentAppUser = await _userManager.GetUserAsync(User);
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
            if (_currentAppUser.Id != cameraReport.AppUserId)
            {
                return Forbid();
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
        public async Task<IActionResult> Create([Bind("DescriptionRemark, CameraImage")] CameraReport cameraReport)
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
                    return await FillReportAndSave(cameraReport, _context);
                }
                else if (cameraForReport == null && cameraLocationForReport != null)
                {
                    scenario = 2;
                    cameraReport.Camera = AddNewCamera();
                    cameraReport.Camera.CameraLocation = cameraLocationForReport;
                    return await FillReportAndSave(cameraReport, _context);
                }
                else if(cameraForReport != null && (cameraLocationForReport != null && cameraForReport.CameraLocation == cameraLocationForReport))
                {
                    cameraReport.Camera = cameraForReport;
                    cameraReport.Camera.CameraLocation = cameraLocationForReport;
                    return await FillReportAndSave(cameraReport, _context);
                }
                else if(cameraForReport != null && (cameraLocationForReport == null || cameraForReport.CameraLocation != cameraLocationForReport))
                {
                    ViewBag.ErrorMessage = "De camera die u probeert te rapporteren is al op een andere locatie gemeld.\nNeem voor meer informatie contact op met de websitebeheerder.";
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

        public async Task<RedirectToActionResult> FillReportAndSave(CameraReport cameraReport, DbContext dbContext)
        {
            if (cameraReport.CameraImage != null)
            {
                string folder = "cameras\\";
                folder += Guid.NewGuid().ToString() + "_" + cameraReport.CameraImage.FileName;
                string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                await cameraReport.CameraImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                cameraReport.CameraImageUrl = "/" + folder;
            }

            cameraReport.CameraId = cameraReport.Camera.Id;
            cameraReport.Camera.CameraLocationId = cameraReport.Camera.CameraLocation.Id;
            cameraReport.AppUserId = _currentAppUser.Id;
            cameraReport.AppUser = _currentAppUser;
            if(scenario == 1)
            {
                _context.Add(cameraReport.Camera);
                _context.Add(cameraReport.Camera.CameraLocation);
                _currentAppUser.TotalScore += 25;
                _context.Update(_currentAppUser);
            }
            if(scenario == 2)
            {
                _context.Add(cameraReport.Camera);
                _currentAppUser.TotalScore += 25;
                _context.Update(_currentAppUser);
            }
            _context.Add(cameraReport);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        // GET: CameraReports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            _currentAppUser = await _userManager.GetUserAsync(User);
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
            if (_currentAppUser.Id != cameraReport.AppUserId)
            {
                return Forbid();
            }
            var editCameraReportsViewModel = new EditCameraReportsViewModel();
            editCameraReportsViewModel.CameraReportId = cameraReport.Id;
            editCameraReportsViewModel.CreatedOn = cameraReport.CreatedOn;
            editCameraReportsViewModel.Camera = cameraReport.Camera;
            editCameraReportsViewModel.CameraImageUrl = cameraReport.CameraImageUrl;
            editCameraReportsViewModel.DescriptionRemark = cameraReport.DescriptionRemark;
            return View(editCameraReportsViewModel);
        }

        // POST: CameraReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DescriptionRemark,CameraReportId")] EditCameraReportsViewModel editCameraReportsViewModel)
        {
            _currentAppUser = await _userManager.GetUserAsync(User);
            var cameraReport = await _context.CameraReports.FindAsync(id);
            if (id != editCameraReportsViewModel.CameraReportId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                cameraReport.DescriptionRemark = editCameraReportsViewModel.DescriptionRemark;
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
            _currentAppUser = await _userManager.GetUserAsync(User);
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
            if (_currentAppUser.Id != cameraReport.AppUserId)
            {
                return Forbid();
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
