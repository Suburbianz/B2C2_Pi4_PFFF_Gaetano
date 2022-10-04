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

namespace B2C2_Pi4_PFFF_Gaetano.Controllers
{
    public class CameraReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string currentAppUserId;
        private AppUser currentAppUser;

        

        public CameraReportsController(ApplicationDbContext context)
        {
            _context = context;
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
        //public async Task<IActionResult> Create([Bind("Id,CreatedOn,DescriptionRemark,CameraId,AppUserId")] CameraReport cameraReport)
        public async Task<IActionResult> Create([Bind("DescriptionRemark")] CameraReport cameraReport)
        {
            
            if (ModelState.IsValid)
            {
                var locations = await GetCameraLocations();
                if (locations.Count != 0)
                {
                    // Check if cameraLocation from input already exists.
                    foreach (CameraLocation cameraLocation in locations)
                    {
                        if (cameraLocation.StreetName == ViewData["StreetName"] && true == true)
                        {
                            // Check if cameraInfo from input already exists.
                            foreach (Camera locationCamera in cameraLocation.Cameras)
                            {
                                if (locationCamera.Name == ViewData["Name"])
                                {
                                    // Add report to camera and currentUser.
                                    AddReport(cameraReport, locationCamera);
                                    return RedirectToAction(nameof(Index));
                                }
                            }
                            Camera camera = AddNewCamera(cameraLocation);
                            
                            //cameraLocation.Cameras = new List<Camera>();
                           //cameraLocation.Cameras.Add(camera);

                            AddReport(cameraReport, camera);
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                CameraLocation newCameraLocation = AddNewCameraLocation();
                Camera newCamera = AddNewCamera(newCameraLocation);
                
                newCamera.CameraLocationId = newCameraLocation.Id;
                newCamera.CameraLocation = newCameraLocation;

                //-newCameraLocation.Cameras = new List<Camera>();

                //newCameraLocation.Cameras.Add(cameraReport.Camera);

                //-newCamera.CameraReports = new List<CameraReport>();

                //newCamera.CameraReports.Add(cameraReport);

                //currentAppUser.CameraReports.Add(cameraReport);

                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

                //-newCameraLocation.Cameras.Add(newCamera);
                AddReport(cameraReport, newCamera);
                return RedirectToAction(nameof(Index));
            }

            return View(cameraReport);
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

        public Camera AddNewCamera(CameraLocation cameraLocation)
        {
            Camera camera = new Camera();

            camera.Name = Request.Form["Camera.Name"];
            camera.ModelNumber = Request.Form["Camera.ModelNumber"];
            camera.SerialNumber = Request.Form["Camera.SerialNumber"];

            camera.CameraLocationId = cameraLocation.Id;
            camera.CameraLocation = cameraLocation;


            return camera;
        }

        public async void AddReport(CameraReport cameraReport, Camera locationCamera)
        {
            currentAppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            currentAppUser = _context.Users.FirstOrDefault(x => x.Id == currentAppUserId);

            cameraReport.CameraId = locationCamera.Id;
            cameraReport.Camera = locationCamera;
            cameraReport.AppUserId = currentAppUserId;
            cameraReport.AppUser = currentAppUser;
            //-locationCamera.CameraReports.Add(cameraReport);
            //if(currentAppUser.CameraReports == null)
            //{
            currentAppUser.CameraReports = new List<CameraReport>();
            //}
            //-currentAppUser.CameraReports.Add(cameraReport);

            await _context.SaveChangesAsync();
            return;
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
