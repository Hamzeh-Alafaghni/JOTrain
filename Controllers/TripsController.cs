using JOTrain.Data;
using JOTrain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace JOTrain.Controllers
{
    public class TripsController : Controller
    {
        private readonly AppDbContext _context;

        public TripsController(AppDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int? fromStation, int? toStation)
        {
            // Start with all trips
            var tripsQuery = _context.Trips
                .Include(t => t.DepartureStation)
                .Include(t => t.ArrivalStation)
                .AsQueryable();

            // Filter by departure station if the user selected one
            if (fromStation.HasValue)
            {
                tripsQuery = tripsQuery.Where(t => t.DepartureStationId == fromStation);
            }

            // Filter by arrival station if the user selected one
            if (toStation.HasValue)
            {
                tripsQuery = tripsQuery.Where(t => t.ArrivalStationId == toStation);
            }

            // Pass the list of stations to the view for the dropdown menus
            ViewBag.Stations = await _context.Stations.ToListAsync();

            // Execute the query and return the filtered list
            return View(await tripsQuery.ToListAsync());
        }

        // 1. Shows the form to create a new trip
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // We need to pass the list of stations to the view so the admin can select them from a dropdown
            ViewBag.Stations = _context.Stations.ToList();
            return View();
        }

        // 2. Saves the new trip to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Trip trip)
        {
            if (ModelState.IsValid)
            {
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New trip scheduled successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdowns and show the form again
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }

        // 1. Shows the confirmation page before deleting
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.DepartureStation)
                .Include(t => t.ArrivalStation)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            return View(trip);
        }

        // 2. Processes the actual deletion when confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip != null)
            {
                _context.Trips.Remove(trip);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Trip canceled and deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // 1. Shows the edit form filled with the trip's current details
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            // Pass the stations list to the view so the dropdowns can be populated
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }

        // 2. Saves the updated changes back to the database
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Trip trip)
        {
            if (id != trip.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(trip);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Trip schedule updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, reload the dropdowns and show the form again
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }
    }
}