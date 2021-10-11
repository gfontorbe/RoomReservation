using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

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

		public async Task<IActionResult> Edit(CalendarViewModel calendarViewModel)
		{
			// TODO:add check that the user is the correct one or is the admin (to avoid override by http request)
			var reservationInDb = await _context.Reservations.FindAsync(calendarViewModel.ReservationToEdit.Id);

			if(reservationInDb.StartingTime != calendarViewModel.ReservationToEdit.StartingTime || reservationInDb.EndingTime != calendarViewModel.ReservationToEdit.EndingTime)
			{
				reservationInDb.StartingTime = calendarViewModel.ReservationToEdit.StartingTime;
				reservationInDb.EndingTime = calendarViewModel.ReservationToEdit.EndingTime;

				_context.Reservations.Update(reservationInDb);
				await _context.SaveChangesAsync();
			}

			return RedirectToAction(controllerName: "Calendars", actionName: "ViewCalendar", routeValues: new { id = calendarViewModel.Room.Id });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			// TODO:add check that the user is the correct one or is the admin (to avoid override by http request)

			var reservationToDelete = await _context.Reservations.Where(x => x.Id == id)
														.Include(r => r.ReservedRoom)
														.FirstAsync();

			_context.Reservations.Remove(reservationToDelete);
			await _context.SaveChangesAsync();

			//return RedirectToAction(controllerName: "Calendars", actionName: "ViewCalendar", routeValues: new { id = reservationToDelete.ReservedRoom.Id });

			return RedirectToAction(controllerName: "Calendars", actionName: "ViewCalendar", routeValues: new { id = reservationToDelete.ReservedRoom.Id });
		}
	}
}
