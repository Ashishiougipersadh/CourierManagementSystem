using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TakealotDriverManagementSystem.Data;
using TakealotDriverManagementSystem.Models;

namespace TakealotDriverManagementSystem.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Register
        [HttpGet]
        public IActionResult Register()
        {
            var model = new AdminRegistrationViewModel();
            return View(model);
        }

        // POST: Admin/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(AdminRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(model);
                }

                // Create a new user object -> Admin
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    ContactNumber = model.ContactNumber,
                    Address = model.Address,
                    Role = "Admin"
                };
                // Save to database
                _context.Users.Add(user);
                _context.SaveChanges();

                // Redirect to login
                return RedirectToAction("Login", "Admin");
            }
            return View(model);
        }

        // GET: Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password && u.Role == "Admin");

            if (user != null)
            {
                // Set session
                HttpContext.Session.SetString("AdminId", user.Id);
                HttpContext.Session.SetString("AdminName", user.FirstName);

                // Log activity
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserId = user.Id,
                    ActionType = "Login",
                    Description = "Admin Logged In",
                    Timestamp = DateTime.Now
                });
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // GET: Admin/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Admin/DriverManagement
        public async Task<IActionResult> DriverManagement()
        {
            var drivers = await _context.Drivers
                .Include(d => d.User)
                .Include(d => d.AssignedVehicle)
                .ToListAsync();

            var availableVehicles = await _context.Vehicles
            .Where(v => !_context.Drivers.Any(d => d.AssignedVehicleId == v.Id))
            .ToListAsync();

            ViewBag.AvailableVehicles = availableVehicles;
            return View(drivers);
        }

        // POST: Admin/AssignVehicle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignVehicleToDriver(int driverId, int vehicleId)
        {
            var driver = await _context.Drivers
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == driverId);
            if (driver == null)
                return NotFound();

            // Check if vehicle is already assigned
            var assignedVehicle = await _context.Drivers
                .FirstOrDefaultAsync(d => d.AssignedVehicleId == vehicleId);

            if (assignedVehicle != null)
            {
                TempData["ErrorMessage"] = "This vehicle is already assigned to another driver.";
                return RedirectToAction(nameof(DriverManagement));
            }

            // Assign the vehicle
            driver.AssignedVehicleId = vehicleId;
            await _context.SaveChangesAsync();

            // Log Activity
            var adminId = HttpContext.Session.GetString("AdminId");
            if (!string.IsNullOrEmpty(adminId))
            {
                var vehicle = await _context.Vehicles.FindAsync(vehicleId);
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserId = adminId,
                    ActionType = "Vehicle Assigned",
                    Description = $"Assigned vehicle {vehicle.LicensePlate} to driver {driver.User.FirstName} {driver.User.LastName}.",
                    Timestamp = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Vehicle assigned successfully!";
            return RedirectToAction(nameof(DriverManagement));
        }
    }
}
