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
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string? search, RoomType? type, bool? isAvailable, int page = 1)
        {
            const int pageSize = 15;

            var roomsQuery = _context.Rooms.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                roomsQuery = roomsQuery.Where(r =>
                    r.Name.ToLower().Contains(term) ||
                    (r.Building != null && r.Building.ToLower().Contains(term)) ||
                    (r.Equipment != null && r.Equipment.ToLower().Contains(term)));
            }

            if (type.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.Type == type.Value);
            }

            if (isAvailable.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.IsAvailable == isAvailable.Value);
            }

            var filteredTotal = await roomsQuery.CountAsync();
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

            var rooms = await roomsQuery
                .OrderBy(r => r.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Type = type;
            ViewBag.IsAvailable = isAvailable;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilteredTotal = filteredTotal;
            PopulateRoomTypeOptions(type);
            PopulateAvailabilityOptions(isAvailable);

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            PopulateRoomTypeOptions();
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Type,Capacity,Building,Floor,HasProjector,HasComputers,IsAvailable,Equipment")] Room room)
        {
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateRoomTypeOptions(room.Type);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            PopulateRoomTypeOptions(room.Type);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Capacity,Building,Floor,HasProjector,HasComputers,IsAvailable,Equipment")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingRoom = await _context.Rooms.FindAsync(id);
                    if (existingRoom == null)
                    {
                        return NotFound();
                    }

                    existingRoom.Name = room.Name;
                    existingRoom.Type = room.Type;
                    existingRoom.Capacity = room.Capacity;
                    existingRoom.Building = room.Building;
                    existingRoom.Floor = room.Floor;
                    existingRoom.HasProjector = room.HasProjector;
                    existingRoom.HasComputers = room.HasComputers;
                    existingRoom.IsAvailable = room.IsAvailable;
                    existingRoom.Equipment = room.Equipment;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
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
            PopulateRoomTypeOptions(room.Type);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

        private void PopulateRoomTypeOptions(RoomType? selectedType = null)
        {
            var options = Enum.GetValues<RoomType>()
                .Select(t => new
                {
                    Value = ((int)t).ToString(),
                    Text = t.ToString()
                })
                .ToList();

            var selectedValue = selectedType.HasValue ? ((int)selectedType.Value).ToString() : null;
            ViewBag.RoomTypeOptions = new SelectList(options, "Value", "Text", selectedValue);
        }

        private void PopulateAvailabilityOptions(bool? selectedStatus = null)
        {
            var options = new[]
            {
                new { Value = "true", Text = "Available" },
                new { Value = "false", Text = "Unavailable" }
            };

            var selectedValue = selectedStatus.HasValue ? selectedStatus.Value.ToString().ToLower() : null;
            ViewBag.RoomAvailabilityOptions = new SelectList(options, "Value", "Text", selectedValue);
        }
    }
}
