using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	public class ReservationsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public ReservationsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public async Task<IActionResult> Create(CalendarViewModel calendarViewModel)
		{
			if (ModelState.IsValid)
			{
				Reservation reservation = new Reservation();

				reservation.StartingTime = calendarViewModel.ReservationToCreate.StartingTime;
				reservation.EndingTime = calendarViewModel.ReservationToCreate.EndingTime;
				reservation.ReservedRoom = await _context.Rooms.FindAsync(calendarViewModel.Room.Id);
				reservation.ReservingUser = await _userManager.FindByNameAsync(User.Identity.Name);

				_context.Add(reservation);
				await _context.SaveChangesAsync();

				return RedirectToAction(controllerName: "Calendars", actionName: "ViewCalendar", routeValues: new { id = reservation.ReservedRoom.Id });
			}

			return RedirectToAction(controllerName: "Calendars", actionName: "ViewCalendar", routeValues: new { id = calendarViewModel.Room.Id });
		}
	}
}
