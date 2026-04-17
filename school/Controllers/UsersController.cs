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
        public async Task<IActionResult> Index(string? search, Role? role, bool? isActive, int page = 1)
        {
            const int pageSize = 15;

            var usersQuery = _context.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                usersQuery = usersQuery.Where(u =>
                    u.FullName.ToLower().Contains(term) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(term)) ||
                    (u.Email != null && u.Email.ToLower().Contains(term)) ||
                    (u.PhoneNumber != null && u.PhoneNumber.ToLower().Contains(term)));
            }

            if (role.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.Role == role.Value);
            }

            if (isActive.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.IsActive == isActive.Value);
            }

            var filteredTotal = await usersQuery.CountAsync();
            var activeCount = await usersQuery.CountAsync(u => u.IsActive);
            var studentsCount = await usersQuery.CountAsync(u => u.Role == Role.Student);
            var teachersCount = await usersQuery.CountAsync(u => u.Role == Role.Teacher);

            var totalPages = filteredTotal == 0
                ? 1
                : (int)Math.Ceiling(filteredTotal / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            var users = await usersQuery
                .OrderBy(u => u.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.RoleFilter = role;
            ViewBag.IsActiveFilter = isActive;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;
            ViewBag.ActiveCount = activeCount;
            ViewBag.StudentsCount = studentsCount;
            ViewBag.TeachersCount = teachersCount;

            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
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
                Role = Role.Student,
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
                    Password = model.Password,
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

            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var model = new UserEditViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = user.Role,
                Gender = user.Gender,
                IsActive = user.IsActive,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Specialization = user.Teacher?.Specialization,
                HireDate = user.Teacher?.HireDate,
                Salary = user.Teacher?.Salary,
                DateOfBirth = user.Student?.DateOfBirth,
                CinNumber = user.Student?.CinNumber,
                SecondPhoneNumber = user.Student?.SecondPhoneNumber,
                Address = user.Student?.Address,
                EnrollmentDate = user.Student?.EnrollmentDate
            };

            PopulateSelectLists();
            return View(model);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Users
                        .Include(u => u.Teacher)
                        .Include(u => u.Student)
                        .FirstOrDefaultAsync(u => u.Id == id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FullName = model.FullName;
                    existingUser.Role = model.Role;
                    existingUser.Gender = model.Gender;
                    existingUser.IsActive = model.IsActive;
                    existingUser.UserName = model.UserName;
                    existingUser.Email = model.Email;
                    existingUser.PhoneNumber = model.PhoneNumber;
                    existingUser.UpdatedAt = DateTime.UtcNow;

                    if (model.Role == Role.Teacher)
                    {
                        if (existingUser.Student != null)
                        {
                            _context.Students.Remove(existingUser.Student);
                        }

                        if (existingUser.Teacher == null)
                        {
                            existingUser.Teacher = new Teacher
                            {
                                Id = Guid.NewGuid(),
                                UserId = existingUser.Id
                            };
                            _context.Teachers.Add(existingUser.Teacher);
                        }

                        existingUser.Teacher.Gender = model.Gender;
                        existingUser.Teacher.Specialization = model.Specialization;
                        existingUser.Teacher.HireDate = model.HireDate;
                        existingUser.Teacher.Salary = model.Salary;
                        existingUser.Teacher.IsActive = model.IsActive;
                    }
                    else if (model.Role == Role.Student)
                    {
                        if (existingUser.Teacher != null)
                        {
                            _context.Teachers.Remove(existingUser.Teacher);
                        }

                        if (existingUser.Student == null)
                        {
                            existingUser.Student = new Student
                            {
                                Id = Guid.NewGuid(),
                                UserId = existingUser.Id
                            };
                            _context.Students.Add(existingUser.Student);
                        }

                        existingUser.Student.Gender = model.Gender;
                        existingUser.Student.DateOfBirth = model.DateOfBirth;
                        existingUser.Student.CinNumber = model.CinNumber;
                        existingUser.Student.PhoneNumber = model.PhoneNumber;
                        existingUser.Student.SecondPhoneNumber = model.SecondPhoneNumber;
                        existingUser.Student.Address = model.Address;
                        existingUser.Student.IsActive = model.IsActive;
                        existingUser.Student.EnrollmentDate = model.EnrollmentDate ?? existingUser.Student.EnrollmentDate;
                        if (existingUser.Student.EnrollmentDate == default)
                        {
                            existingUser.Student.EnrollmentDate = DateTime.UtcNow;
                        }
                    }
                    else
                    {
                        if (existingUser.Teacher != null)
                        {
                            _context.Teachers.Remove(existingUser.Teacher);
                        }

                        if (existingUser.Student != null)
                        {
                            _context.Students.Remove(existingUser.Student);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(model.Id))
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
            return View(model);
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
            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user != null)
            {
                if (user.Teacher != null)
                {
                    _context.Teachers.Remove(user.Teacher);
                }

                if (user.Student != null)
                {
                    _context.Students.Remove(user.Student);
                }

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
            // Only show Teacher and Student roles (exclude Admin)
            var roles = new List<Role> { Role.Teacher, Role.Student };
            ViewBag.Roles = new SelectList(roles);
            ViewBag.Genders = new SelectList(Enum.GetValues<Gender>());
        }

        /// <summary>
        /// Generates a student ID with format: 2 letters from name + YYYY (birth year) + DD (birth day)
        /// Example: "JO199915" for John born 1999-09-15
        /// </summary>
        public static string GenerateStudentId(string fullName, DateTime? dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(fullName) || !dateOfBirth.HasValue)
                return string.Empty;

            var namePart = new string(fullName
                .Where(char.IsLetter)
                .Take(2)
                .Select(char.ToUpper)
                .ToArray());

            if (namePart.Length < 2)
                namePart = namePart.PadRight(2, 'X');

            var yearPart = dateOfBirth.Value.Year.ToString();
            var dayPart = dateOfBirth.Value.Day.ToString().PadLeft(2, '0');

            return $"{namePart}{yearPart}{dayPart}";
        }
    }
}
