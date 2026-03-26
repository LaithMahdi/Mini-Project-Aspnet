using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_project_aspnet.Models;
using System.Security.Cryptography;
using System.Text;

namespace mini_project_aspnet.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required.");
                return View();
            }

            var user = await _context.users.Where(u => u.email == email).FirstOrDefaultAsync();

            if (user == null || !VerifyPassword(password, user.password))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View();
            }

            // Store user session
            HttpContext.Session.SetString("UserId", user.id.ToString());
            HttpContext.Session.SetString("UserEmail", user.email);
            HttpContext.Session.SetString("UserRole", user.role.ToString());
            HttpContext.Session.SetString("UserFullName", user.fullName);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashOfInput = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
                var hashString = Convert.ToBase64String(hashOfInput);
                return hashString == storedHash;
            }
        }
    }
}
