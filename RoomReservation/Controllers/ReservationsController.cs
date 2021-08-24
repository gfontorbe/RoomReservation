using Microsoft.AspNetCore.Mvc;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	public class ReservationsController : Controller
	{
		public IActionResult Create(CalendarViewModel calendarViewModel)
		{

			return View();
		}
	}
}
