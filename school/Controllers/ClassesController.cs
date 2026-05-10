using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school.Models;
using school.Services;

namespace school.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationService _notificationService;

        public ClassesController(ApplicationDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: Classes
        public async Task<IActionResult> Index(string? search, int? sectionId, Guid? teacherId, int page = 1)
        {
            const int pageSize = 15;
            var isTeacher = User.IsInRole(Role.Teacher.ToString());
            var currentTeacherId = await GetCurrentTeacherIdAsync();

            var classesQuery = _context.Classes
                .Include(c => c.ReferentTeacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Section)
                .AsQueryable();

            if (isTeacher)
            {
                if (!currentTeacherId.HasValue)
                {
                    return Forbid();
                }

                classesQuery = classesQuery.Where(c =>
                    c.ReferentTeacherId == currentTeacherId.Value ||
                    c.ClassSubjects.Any(cs => cs.TeacherId == currentTeacherId.Value));
                teacherId = currentTeacherId.Value;
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                classesQuery = classesQuery.Where(c =>
                    c.Name.ToLower().Contains(term) ||
                    c.Level.ToLower().Contains(term) ||
                    c.AcademicYear.ToLower().Contains(term) ||
                    (c.Filiere != null && c.Filiere.ToLower().Contains(term)) ||
                    (c.Section != null && c.Section.Name.ToLower().Contains(term)) ||
                    (c.ReferentTeacher != null && c.ReferentTeacher.User != null && c.ReferentTeacher.User.FullName.ToLower().Contains(term)));
            }

            if (sectionId.HasValue)
            {
                classesQuery = classesQuery.Where(c => c.SectionId == sectionId.Value);
            }

            if (teacherId.HasValue)
            {
                classesQuery = classesQuery.Where(c => c.ReferentTeacherId == teacherId.Value);
            }

            var filteredTotal = await classesQuery.CountAsync();
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

            var classes = await classesQuery
                .OrderBy(c => c.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.SectionId = sectionId;
            ViewBag.TeacherId = teacherId;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;
            ViewBag.IsTeacher = isTeacher;

            var sectionOptions = await _context.Sections
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, Display = s.Name + " (" + s.Code + ")" })
                .ToListAsync();

            var teacherOptions = await _context.Teachers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.Id,
                    Display = t.User.FullName + (string.IsNullOrWhiteSpace(t.Specialization) ? string.Empty : " - " + t.Specialization)
                })
                .ToListAsync();

            ViewBag.SectionOptions = new SelectList(sectionOptions, "Id", "Display", sectionId);
            ViewBag.TeacherOptions = new SelectList(teacherOptions, "Id", "Display", teacherId);

            return View(classes);
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classe = await _context.Classes
                .Include(c => c.ReferentTeacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Section)
                .Include(c => c.Students)
                    .ThenInclude(s => s.User)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Teacher)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classe == null)
            {
                return NotFound();
            }

            if (User.IsInRole(Role.Teacher.ToString()))
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();
                if (!currentTeacherId.HasValue ||
                    (classe.ReferentTeacherId != currentTeacherId.Value &&
                     !classe.ClassSubjects.Any(cs => cs.TeacherId == currentTeacherId.Value)))
                {
                    return Forbid();
                }

                ViewBag.IsTeacher = true;
            }

            return View(classe);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            PopulateSelectLists();
            return View(new Classe
            {
                AcademicYear = DateTime.UtcNow.Year + "-" + (DateTime.UtcNow.Year + 1)
            });
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Level,Filiere,MaxCapacity,IsArchived,AcademicYear,SectionId,ReferentTeacherId")] Classe classe)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                classe.Id = Guid.NewGuid();
                _context.Add(classe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateSelectLists(classe.SectionId, classe.ReferentTeacherId);
            return View(classe);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var classe = await _context.Classes.FindAsync(id);
            if (classe == null)
            {
                return NotFound();
            }
            PopulateSelectLists(classe.SectionId, classe.ReferentTeacherId);
            await PopulateAssignmentSelectLists();
            await PopulateExistingAssignments(classe.Id);
            await PopulateStudentAssignmentSelectList(classe.Id);
            return View(classe);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Level,Filiere,MaxCapacity,IsArchived,AcademicYear,SectionId,ReferentTeacherId")] Classe classe)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id != classe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldClasse = await _context.Classes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                    var oldReferentTeacherId = oldClasse?.ReferentTeacherId;

                    var existingClasse = await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
                    if (existingClasse == null)
                    {
                        return NotFound();
                    }

                    existingClasse.Name = classe.Name;
                    existingClasse.Level = classe.Level;
                    existingClasse.Filiere = classe.Filiere;
                    existingClasse.MaxCapacity = classe.MaxCapacity;
                    existingClasse.IsArchived = classe.IsArchived;
                    existingClasse.AcademicYear = classe.AcademicYear;
                    existingClasse.SectionId = classe.SectionId;
                    existingClasse.ReferentTeacherId = classe.ReferentTeacherId;

                    await _context.SaveChangesAsync();

                    // Notification: Referent Teacher assigned
                    if (classe.ReferentTeacherId != oldReferentTeacherId && classe.ReferentTeacherId.HasValue)
                    {
                        var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == classe.ReferentTeacherId.Value);
                        if (teacher != null)
                        {
                            await _notificationService.SendNotificationAsync(
                                teacher.UserId,
                                "Referent Teacher Assignment",
                                $"You have been assigned as the referent teacher for class {classe.Name}.",
                                $"/Classes/Details/{classe.Id}"
                            );
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClasseExists(classe.Id))
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
            PopulateSelectLists(classe.SectionId, classe.ReferentTeacherId);
            await PopulateAssignmentSelectLists();
            await PopulateExistingAssignments(classe.Id);
            await PopulateStudentAssignmentSelectList(classe.Id);
            return View(classe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStudentAssignments(Guid id, List<Guid>? studentIds)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            var classeExists = await _context.Classes.AnyAsync(c => c.Id == id);
            if (!classeExists)
            {
                return NotFound();
            }

            var selectedSet = (studentIds ?? new List<Guid>()).ToHashSet();

            var studentsInClass = await _context.Students
                .Where(s => s.ClassId == id)
                .ToListAsync();

            foreach (var student in studentsInClass.Where(s => !selectedSet.Contains(s.Id)))
            {
                student.ClassId = null;
            }

            var assignableSelectedStudents = await _context.Students
                .Where(s => selectedSet.Contains(s.Id) && s.IsActive && (s.ClassId == null || s.ClassId == id))
                .ToListAsync();

            foreach (var student in assignableSelectedStudents)
            {
                student.ClassId = id;
            }

            var blockedCount = await _context.Students
                .CountAsync(s => selectedSet.Contains(s.Id) && s.ClassId != null && s.ClassId != id);

            if (blockedCount > 0)
            {
                TempData["ClassEditError"] = $"{blockedCount} selected student(s) are already assigned to another class. Remove them from their current class first.";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSubjectAssignment(Guid id, int subjectId, Guid? teacherId)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (teacherId == null)
            {
                TempData["ClassEditError"] = "Please select a teacher for the subject assignment.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            var classeExists = await _context.Classes.AnyAsync(c => c.Id == id);
            if (!classeExists)
            {
                return NotFound();
            }

            var subjectExists = await _context.Subjects.AnyAsync(s => s.Id == subjectId && s.IsActive);
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId && t.IsActive);

            if (!subjectExists || !teacherExists)
            {
                TempData["ClassEditError"] = "Invalid subject or teacher selection.";
                return RedirectToAction(nameof(Edit), new { id });
            }

            var existing = await _context.ClassSubjects
                .FirstOrDefaultAsync(cs => cs.ClassId == id && cs.SubjectId == subjectId);

            if (existing == null)
            {
                _context.ClassSubjects.Add(new ClassSubject
                {
                    ClassId = id,
                    SubjectId = subjectId,
                    TeacherId = teacherId
                });
            }
            else
            {
                existing.TeacherId = teacherId;
            }

            await _context.SaveChangesAsync();

            // Notification: Teacher assigned to subject
            if (teacherId.HasValue)
            {
                var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == teacherId.Value);
                var subject = await _context.Subjects.FindAsync(subjectId);
                var classe = await _context.Classes.FindAsync(id);
                if (teacher != null && subject != null && classe != null)
                {
                    await _notificationService.SendNotificationAsync(
                        teacher.UserId,
                        "Subject Assignment",
                        $"You have been assigned to teach {subject.Name} in class {classe.Name}.",
                        $"/Classes/Details/{id}"
                    );
                }
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSubjectAssignment(Guid id, int classSubjectId)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            var assignment = await _context.ClassSubjects
                .FirstOrDefaultAsync(cs => cs.Id == classSubjectId && cs.ClassId == id);

            if (assignment != null)
            {
                _context.ClassSubjects.Remove(assignment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Edit), new { id });
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var classe = await _context.Classes
                .Include(c => c.ReferentTeacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Section)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classe == null)
            {
                return NotFound();
            }

            return View(classe);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (User.IsInRole(Role.Teacher.ToString()))
            {
                return Forbid();
            }

            var classe = await _context.Classes.FindAsync(id);
            if (classe != null)
            {
                _context.Classes.Remove(classe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClasseExists(Guid id)
        {
            return _context.Classes.Any(e => e.Id == id);
        }

        private void PopulateSelectLists(int? sectionId = null, Guid? referentTeacherId = null)
        {
            var sections = _context.Sections
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, Display = s.Name + " (" + s.Code + ")" })
                .ToList();

            var teachers = _context.Teachers
                .Include(t => t.User)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.Id,
                    Display = t.User.FullName + (string.IsNullOrWhiteSpace(t.Specialization) ? string.Empty : " - " + t.Specialization)
                })
                .ToList();

            ViewData["SectionId"] = new SelectList(sections, "Id", "Display", sectionId);
            ViewData["ReferentTeacherId"] = new SelectList(teachers, "Id", "Display", referentTeacherId);
        }

        private async Task PopulateAssignmentSelectLists(int? subjectId = null, Guid? teacherId = null)
        {
            var subjects = await _context.Subjects
                .Where(s => s.IsActive)
                .OrderBy(s => s.Name)
                .Select(s => new { s.Id, Display = s.Name + " (" + s.Code + ")" })
                .ToListAsync();

            var teachers = await _context.Teachers
                .Include(t => t.User)
                .Where(t => t.IsActive)
                .OrderBy(t => t.User.FullName)
                .Select(t => new
                {
                    t.Id,
                    Display = t.User.FullName + (string.IsNullOrWhiteSpace(t.Specialization) ? string.Empty : " - " + t.Specialization)
                })
                .ToListAsync();

            ViewData["SubjectOptions"] = new SelectList(subjects, "Id", "Display", subjectId);
            ViewData["TeacherOptions"] = new SelectList(teachers, "Id", "Display", teacherId);
        }

        private async Task PopulateExistingAssignments(Guid classId)
        {
            var assignments = await _context.ClassSubjects
                .Include(cs => cs.Subject)
                .Include(cs => cs.Teacher)
                    .ThenInclude(t => t.User)
                .Where(cs => cs.ClassId == classId)
                .OrderBy(cs => cs.Subject.Name)
                .ToListAsync();

            ViewData["ClassSubjectAssignments"] = assignments;
        }

        private async Task PopulateStudentAssignmentSelectList(Guid classId)
        {
            var students = await _context.Students
                .Include(s => s.User)
                .Where(s => s.IsActive && (s.ClassId == null || s.ClassId == classId))
                .OrderBy(s => s.User.FullName)
                .Select(s => new
                {
                    s.Id,
                    Display = s.User.FullName + (s.ClassId == classId ? " (Assigned)" : "")
                })
                .ToListAsync();

            var selectedIds = await _context.Students
                .Where(s => s.ClassId == classId)
                .Select(s => s.Id)
                .ToListAsync();

            ViewData["ClassStudentOptions"] = new MultiSelectList(students, "Id", "Display", selectedIds);
        }

        private async Task<Guid?> GetCurrentTeacherIdAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return null;
            }

            return await _context.Teachers
                .Where(t => t.UserId == userId && t.IsActive)
                .Select(t => (Guid?)t.Id)
                .FirstOrDefaultAsync();
        }
    }
}
