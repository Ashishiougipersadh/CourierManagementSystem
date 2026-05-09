using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TakealotDriverManagementSystem.Data;
using TakealotDriverManagementSystem.Models;

namespace TakealotDriverManagementSystem.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobApplicationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JobApplication/Index
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Applications.Include(j => j.User).Include(j => j.Vacancy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: JobApplication/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.Applications
                .Include(j => j.User)
                .Include(j => j.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // GET: JobApplication/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description");
            return View();
        }

        // POST: JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VacancyId,UserId,LicenseNumber,ResumePath,Status,DateSubmitted")] JobApplication jobApplication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", jobApplication.UserId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancies, "Id", "Description", jobApplication.VacancyId);
            return View(jobApplication);
        }

        // GET: JobApplication/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.Applications
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (jobApplication == null) return NotFound();

            var model = new UpdateJobApplicationStatusViewModel
            {
                Id = jobApplication.Id,
                Status = jobApplication.Status
            };

            return View(model);
        }

        // POST: JobApplication/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateJobApplicationStatusViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var application = await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (application == null) return NotFound();

            var oldStatus = application.Status;
            application.Status = model.Status;

            // Create a Notification
            _context.Notifications.Add(new Notification
            {
                UserId = application.UserId,
                Message = $"Your application for '{application.Vacancy.Name}' has been {application.Status}.",
                CreatedAt = DateTime.Now
            });

            // Log admin action
            var adminId = HttpContext.Session.GetString("AdminId") ?? "UnknownAdmin";
            _context.ActivityLogs.Add(new ActivityLog
            {
                UserId = adminId,
                ActionType = "Application Processed",
                Description = $"Application ID {application.Id} – Status changed from {oldStatus} to {application.Status}"
            });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: JobApplication/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplication = await _context.Applications
                .Include(j => j.User)
                .Include(j => j.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        // POST: JobApplication/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.Applications.FindAsync(id);
            if (jobApplication != null)
            {
                _context.Applications.Remove(jobApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
