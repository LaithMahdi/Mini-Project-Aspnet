using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school.Models;
using school.ViewModels;

namespace school.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            PopulateSelectLists();
            return View(new UserCreateViewModel
            {
                IsActive = true,
                EnrollmentDate = DateTime.UtcNow
            });
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    FullName = model.FullName,
                    Role = model.Role,
                    Gender = model.Gender,
                    IsActive = model.IsActive,
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);

                if (model.Role == Role.Teacher)
                {
                    var teacher = new Teacher
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Gender = model.Gender,
                        Specialization = model.Specialization,
                        HireDate = model.HireDate,
                        Salary = model.Salary,
                        IsActive = model.IsActive
                    };

                    _context.Teachers.Add(teacher);
                }
                else if (model.Role == Role.Student)
                {
                    var student = new Student
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Gender = model.Gender,
                        DateOfBirth = model.DateOfBirth,
                        CinNumber = model.CinNumber,
                        PhoneNumber = model.PhoneNumber,
                        SecondPhoneNumber = model.SecondPhoneNumber,
                        Address = model.Address,
                        IsActive = model.IsActive,
                        EnrollmentDate = model.EnrollmentDate ?? DateTime.UtcNow
                    };

                    _context.Students.Add(student);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists();
            return View(model);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            PopulateSelectLists();
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FullName,Role,Gender,IsActive,UserName,Email,PhoneNumber")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FullName = user.FullName;
                    existingUser.Role = user.Role;
                    existingUser.Gender = user.Gender;
                    existingUser.IsActive = user.IsActive;
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;
                    existingUser.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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

            PopulateSelectLists();
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private void PopulateSelectLists()
        {
            ViewBag.Roles = new SelectList(Enum.GetValues<Role>());
            ViewBag.Genders = new SelectList(Enum.GetValues<Gender>());
        }
    }
}
