using JOTrain.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JOTrain.Controllers
{
    public class StationsController : Controller
    {
        private readonly AppDbContext _context;

        public StationsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           
            var stations = await _context.Stations.ToListAsync();
            return View(stations);
        }
    }
}