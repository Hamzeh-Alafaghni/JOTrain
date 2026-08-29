using JOTrain.Data;
using JOTrain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JOTrain.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Shows the login page
        public IActionResult Login()
        {
            return View();
        }

        // 2. Processes the login credentials
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Check the database for a matching user
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Create the security claims (the data stored in the cookie)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Issue the cookie to the user's browser
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        // 3. Logs the user out
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // 1. Shows the registration form
        public IActionResult Register()
        {
            return View();
        }

        // 2. Processes the registration form
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            // Check if email already exists
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                ViewBag.Error = "An account with this email already exists.";
                return View();
            }

            // Create the new client user
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password, // Note: In production, passwords should be hashed. Keeping it plain text matches your current project setup.
                Role = UserRole.Client // Automatically forces them to be a Client, never an Admin
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Account created successfully! Please log in.";
            return RedirectToAction("Login");
        }
    }
}
