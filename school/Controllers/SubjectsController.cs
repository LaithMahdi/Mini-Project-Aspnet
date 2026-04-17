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
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(string? search, SubjectType? type, bool? isActive)
        {
            var subjectsQuery = _context.Subjects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                subjectsQuery = subjectsQuery.Where(s =>
                    s.Name.ToLower().Contains(term) ||
                    s.Code.ToLower().Contains(term));
            }

            if (type.HasValue)
            {
                subjectsQuery = subjectsQuery.Where(s => s.Type == type.Value);
            }

            if (isActive.HasValue)
            {
                subjectsQuery = subjectsQuery.Where(s => s.IsActive == isActive.Value);
            }

            ViewBag.Search = search;
            ViewBag.Type = type;
            ViewBag.IsActive = isActive;
            PopulateSubjectTypeOptions(type);
            PopulateActiveStatusOptions(isActive);

            var subjects = await subjectsQuery
                .OrderBy(s => s.Name)
                .ToListAsync();

            return View(subjects);
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            PopulateSubjectTypeOptions();
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Code,Coefficient,HoursPerWeek,Type,IsActive")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateSubjectTypeOptions(subject.Type);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            PopulateSubjectTypeOptions(subject.Type);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Code,Coefficient,HoursPerWeek,Type,IsActive")] Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSubject = await _context.Subjects.FindAsync(id);
                    if (existingSubject == null)
                    {
                        return NotFound();
                    }

                    existingSubject.Name = subject.Name;
                    existingSubject.Code = subject.Code;
                    existingSubject.Coefficient = subject.Coefficient;
                    existingSubject.HoursPerWeek = subject.HoursPerWeek;
                    existingSubject.Type = subject.Type;
                    existingSubject.IsActive = subject.IsActive;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            PopulateSubjectTypeOptions(subject.Type);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }

        private void PopulateSubjectTypeOptions(SubjectType? selectedType = null)
        {
            var options = Enum.GetValues<SubjectType>()
                .Select(t => new
                {
                    Value = ((int)t).ToString(),
                    Text = t.ToString()
                })
                .ToList();

            var selectedValue = selectedType.HasValue ? ((int)selectedType.Value).ToString() : null;
            ViewBag.SubjectTypeOptions = new SelectList(options, "Value", "Text", selectedValue);
        }

        private void PopulateActiveStatusOptions(bool? selectedStatus = null)
        {
            var options = new[]
            {
                new { Value = "true", Text = "Active" },
                new { Value = "false", Text = "Inactive" }
            };

            var selectedValue = selectedStatus.HasValue ? selectedStatus.Value.ToString().ToLower() : null;
            ViewBag.ActiveStatusOptions = new SelectList(options, "Value", "Text", selectedValue);
        }
    }
}
