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
            //all trips
            var tripsQuery = _context.Trips
                .Include(t => t.DepartureStation)
                .Include(t => t.ArrivalStation)
                .AsQueryable();

            // Filter
            if (fromStation.HasValue)
            {
                tripsQuery = tripsQuery.Where(t => t.DepartureStationId == fromStation);
            }

            // Filter
            if (toStation.HasValue)
            {
                tripsQuery = tripsQuery.Where(t => t.ArrivalStationId == toStation);
            }

            // dropdown menus // pass list
            ViewBag.Stations = await _context.Stations.ToListAsync();

            // Execute query
            return View(await tripsQuery.ToListAsync());
        }

        // Shows the form to create a new trip
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            
            ViewBag.Stations = _context.Stations.ToList();
            return View();
        }

        // Saves the new trip to the database
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

           
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }

        // Shows the confirmation page before deleting
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

        // Processes the actual deletion
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

        // Shows the edit form filled with the trip's current details
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var trip = await _context.Trips.FindAsync(id);
            if (trip == null) return NotFound();

            
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }

        // Saves the updated database
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

           
            ViewBag.Stations = _context.Stations.ToList();
            return View(trip);
        }
    }
}