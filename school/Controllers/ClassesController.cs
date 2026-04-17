using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using school.Models;

namespace school.Controllers
{
    public class ClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Classes
        public async Task<IActionResult> Index(string? search, int? sectionId, Guid? teacherId, int page = 1)
        {
            const int pageSize = 15;

            var classesQuery = _context.Classes
                .Include(c => c.ReferentTeacher)
                    .ThenInclude(t => t.User)
                .Include(c => c.Section)
                .AsQueryable();

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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (classe == null)
            {
                return NotFound();
            }

            return View(classe);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
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
            return View(classe);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Level,Filiere,MaxCapacity,IsArchived,AcademicYear,SectionId,ReferentTeacherId")] Classe classe)
        {
            if (id != classe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingClasse = await _context.Classes.FindAsync(id);
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
            return View(classe);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
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
    }
}
