using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	[Authorize]
	public class CalendarsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public CalendarsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var rooms = await _context.Rooms.ToListAsync();
			rooms.OrderBy(x => x.Name);

			return View(rooms);
		}

		public async Task<IActionResult> ViewCalendar(string id)
		{
			var connectedUser = await _userManager.FindByNameAsync(User.Identity.Name);
			var room = await _context.Rooms.FindAsync(id);

			var reservations = await _context.Reservations.Where(x => x.ReservedRoom.Id == id).Include(r => r.ReservingUser).ToListAsync();

			var reservationsVM = new List<ReservationViewModel>();

			foreach(var r in reservations)
			{
				ReservationViewModel viewModel = new ReservationViewModel
				{
					Id = r.Id,
					StartingTime = r.StartingTime,
					EndingTime = r.EndingTime,
					Title = $"{r.ReservingUser.FirstName} {r.ReservingUser.LastName}",
					Editable = r.ReservingUser.Id == connectedUser.Id || User.IsInRole("Admin") ? true : false,
					DurationEditable = r.ReservingUser.Id == connectedUser.Id || User.IsInRole("Admin") ? true : false,
					Overlap = false
				};

				reservationsVM.Add(viewModel);
			}

			var calendarVM = new CalendarViewModel();

			calendarVM.Room = room;
			calendarVM.JSONData = JsonSerializer.Serialize(reservationsVM, new JsonSerializerOptions { WriteIndented = true});

			return View(calendarVM);
		}
	}
}
