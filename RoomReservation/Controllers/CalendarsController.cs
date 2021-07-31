using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.ProjectModel;
using RoomReservation.Data;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	public class CalendarsController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ApplicationDbContext _context;

		public CalendarsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public IActionResult Index()
		{


			var resUser = _userManager.FindByEmailAsync("basic@company.com").Result;
			var resRoom = _context.Rooms.Find("1e090611-c2df-47e2-90b9-70197b9888e3");

			Reservation reservation = new Reservation()
			{
				Id = Guid.NewGuid().ToString(),
				StartingTime = DateTime.Now,
				Duration = 60,
				ReservingUser = resUser,
				ReservedRoom = resRoom
			};

			ReservationViewModel reservationViewModels = new ReservationViewModel()
			{
				Id = reservation.Id,
				StartingTime = reservation.StartingTime,
				EndingTime = reservation.EndingTime,
				Title = $"{reservation.ReservingUser.FirstName} {reservation.ReservingUser.LastName}",
				Editable = false,
				DurationEditable = false,
				Overlap = false
			};


			// return View();

			// View for tests
			return View("Test", reservationViewModels);
		}
	}
}
