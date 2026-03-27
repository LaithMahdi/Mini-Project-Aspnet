using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mini_project_aspnet.Models;
using System.Security.Cryptography;
using System.Text;

namespace mini_project_aspnet.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.students.ToListAsync();
            return View(students);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.students
                .FirstOrDefaultAsync(m => m.id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fullName,email,gender,dateOfBirth,cinNumber,phoneNumber,secondPhoneNumber,address,isActive")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.id = Guid.NewGuid();
                student.role = UserRole.Student;

                // Use dateOfBirth as password (format: yyyy-MM-dd)
                string passwordToHash = student.dateOfBirth.ToString("yyyy-MM-dd");
                student.password = HashPassword(passwordToHash);

                student.createdBy = "Admin";
                // isActive is already set from form (checked/unchecked)
                student.enrollmentDate = DateTime.UtcNow;

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("id,fullName,email,gender,dateOfBirth,cinNumber,phoneNumber,secondPhoneNumber,address,isActive,enrollmentDate")] Student student)
        {
            if (id != student.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Keep existing password and other audit fields
                    var existingStudent = await _context.students.FindAsync(id);
                    if (existingStudent != null)
                    {
                        existingStudent.fullName = student.fullName;
                        existingStudent.email = student.email;
                        existingStudent.gender = student.gender;
                        existingStudent.dateOfBirth = student.dateOfBirth;
                        existingStudent.cinNumber = student.cinNumber;
                        existingStudent.phoneNumber = student.phoneNumber;
                        existingStudent.secondPhoneNumber = student.secondPhoneNumber;
                        existingStudent.address = student.address;
                        existingStudent.isActive = student.isActive;

                        _context.Update(existingStudent);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.id))
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
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var student = await _context.students.FindAsync(id);
            if (student != null)
            {
                _context.students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id)
        {
            return _context.students.Any(e => e.id == id);
        }

        // Helper method to hash passwords
        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
