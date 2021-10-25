using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["SearchFilter"] = searchString;

            var rooms = await _context.Rooms.ToListAsync();

			if (!String.IsNullOrEmpty(searchString))
			{
                rooms = rooms.Where(x => x.Name.ToLower().Contains(searchString.ToLower())
                || x.Location.ToLower().Contains(searchString.ToLower())
                || x.Description.ToLower().Contains(searchString.ToLower())).ToList();
			}

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(string id)
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
			return View();
		}

		// POST: Rooms/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Location,Description")] Room room)
        {
            if (ModelState.IsValid)
            {
                room.Id = Guid.NewGuid().ToString();
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

		// GET: Rooms/Edit/5
		public async Task<IActionResult> Edit(string id)
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
			return View(room);
		}

		// POST: Rooms/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Location,Description")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
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
                return RedirectToRoute(routeName: "return", routeValues: new {controller = "Rooms", action = "Index"});
            }
            return View(room);
        }

		// GET: Rooms/Delete/5
		public async Task<IActionResult> Delete(string id)
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToRoute(routeName: "return", routeValues: new { controller = "Rooms", action = "Index" });
        }

        private bool RoomExists(string id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }
    }
}
