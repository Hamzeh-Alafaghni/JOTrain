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

        //Shows form
        public async Task<IActionResult> Book(int id)
        {
            var trip = await _context.Trips
                .Include(t => t.DepartureStation)
                .Include(t => t.ArrivalStation)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trip == null) return NotFound();

            return View(trip);
        }

        //seat limits
        //submission
        [HttpPost]
        public async Task<IActionResult> BookTicket(int tripId, SeatClass seatClass, PaymentMethod paymentVia)
        {
            
            int bookedSeats = await _context.Tickets
                .CountAsync(t => t.TripId == tripId && t.ClassType == seatClass);

         
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

        
        public async Task<IActionResult> MyTickets()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var tickets = await _context.Tickets
                .Include(t => t.Trip)
                .ThenInclude(tr => tr.DepartureStation)
                .Include(t => t.Trip)
                .ThenInclude(tr => tr.ArrivalStation)  
                .Where(t => t.UserId == userId)        
                .OrderByDescending(t => t.BookingDate)
                .ToListAsync();

            return View(tickets);
        }
    }
}