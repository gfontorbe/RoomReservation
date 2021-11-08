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
using RoomReservation.Data.Repositories;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoomsController : Controller
    {
		private readonly IRepository<Room> _repository;

		public RoomsController(IRepository<Room> repository)
        {
			_repository = repository;
		}

        // GET: Rooms
        public async Task<IActionResult> Index(string? searchString)
        {
            ViewData["SearchFilter"] = searchString;

            var rooms = await _repository.GetAllAsync();

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

            var room = await _repository.GetByIdAsync(id);

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

                await _repository.AddAsync(room);

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

            var room = await _repository.GetByIdAsync(id);

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
                    await _repository.UpdateAsync(room);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoomExists(room.Id))
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

            var room = await _repository.GetByIdAsync(id);

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
            var room = await _repository.GetByIdAsync(id);

            await _repository.DeleteAsync(room);

            return RedirectToRoute(routeName: "return", routeValues: new { controller = "Rooms", action = "Index" });
        }

        private async Task<bool> RoomExists(string id)
        {
            var room = await _repository.GetByIdAsync(id);
            return room != null;
        }
    }
}
