using JOTrain.Data;
using JOTrain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace JOTrain.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Shows the booking form
        public async Task<IActionResult> Book(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.DepartureStation)
                .Include(t => t.ArrivalStation)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            return View(trip);
        }

        // 2. Processes the form submission and enforces seat limits
        [HttpPost]
        public async Task<IActionResult> BookTicket(int tripId, SeatClass seatClass, PaymentMethod paymentVia)
        {
            // Count existing tickets for this specific trip and class
            int bookedSeats = await _context.Tickets
                .CountAsync(t => t.TripId == tripId && t.ClassType == seatClass);

            // Enforce capacity rules
            bool isFull = seatClass switch
            {
                SeatClass.Economy => bookedSeats >= 50,
                SeatClass.EconomyPlus => bookedSeats >= 30,
                SeatClass.VIP => bookedSeats >= 20,
                _ => true
            };

            if (isFull)
            {
                TempData["Error"] = $"Sorry, {seatClass} is fully booked for this trip!";
                return RedirectToAction("Book", new { id = tripId });
            }

            // Create and save the new ticket
            var ticket = new Ticket
            {
                TripId = tripId,
                UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                ClassType = seatClass,
                PaymentVia = paymentVia,
                BookingDate = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Ticket booked successfully!";
            return RedirectToAction("Index", "Trips");
        }

        // 3. Displays the tickets for the current user
        public async Task<IActionResult> MyTickets()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tickets = await _context.Tickets
                .Include(t => t.Trip)
                .ThenInclude(tr => tr.DepartureStation) // Joins the Departure Station name
                .Include(t => t.Trip)
                .ThenInclude(tr => tr.ArrivalStation)   // Joins the Arrival Station name
                .Where(t => t.UserId == userId)         // Filters dynamically for whoever is logged in
                .OrderByDescending(t => t.BookingDate)
                .ToListAsync();

            return View(tickets);
        }
    }
}