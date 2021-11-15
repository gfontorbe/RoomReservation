using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Data;
using RoomReservation.Data.Repositories;
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
		private readonly IReservationRepository _repository;

		public ReservationsController(UserManager<ApplicationUser> userManager, IReservationRepository repository)
		{
			_userManager = userManager;
			_repository = repository;
		}

		public async Task<IActionResult> Create(CalendarViewModel calendarViewModel)
		{
			if (ModelState.IsValid)
			{
				Reservation reservation = new Reservation();

				reservation.StartingTime = calendarViewModel.ReservationToCreate.StartingTime;
				reservation.EndingTime = calendarViewModel.ReservationToCreate.EndingTime;
				reservation.ReservedRoom = await _repository.GetRoomById(calendarViewModel.Room.Id);
				reservation.ReservingUser = await _userManager.FindByNameAsync(User.Identity.Name);

                await _repository.AddAsync(reservation);
				
				return RedirectToRoute("return",routeValues: new {controller= "Calendars", action="ViewCalendar", id = reservation.ReservedRoom.Id });
			}
			return RedirectToRoute("return", routeValues: new { controller = "Calendars", action = "ViewCalendar", id = calendarViewModel.Room.Id });
		}

		public async Task<IActionResult> Edit(CalendarViewModel calendarViewModel)
		{
			// TODO:add check that the user is the correct one or is the admin (to avoid override by http request)
			var reservationInDb = await _repository.GetByIdAsync(calendarViewModel.ReservationToEdit.Id);

			if(reservationInDb.StartingTime != calendarViewModel.ReservationToEdit.StartingTime || reservationInDb.EndingTime != calendarViewModel.ReservationToEdit.EndingTime)
			{
				reservationInDb.StartingTime = calendarViewModel.ReservationToEdit.StartingTime;
				reservationInDb.EndingTime = calendarViewModel.ReservationToEdit.EndingTime;

				await _repository.UpdateAsync(reservationInDb);
			}

			return RedirectToRoute("return", routeValues: new { controller = "Calendars", action = "ViewCalendar", id = calendarViewModel.Room.Id });
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string id)
		{
			// TODO:add check that the user is the correct one or is the admin (to avoid override by http request)

			var reservationToDelete = await _repository.GetByIdAsync(id);

			await _repository.DeleteAsync(reservationToDelete);

			return RedirectToRoute("return", routeValues: new { controller = "Calendars", action = "ViewCalendar", id = reservationToDelete.ReservedRoom.Id });
		}
	}
}
