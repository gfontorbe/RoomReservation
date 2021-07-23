using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	public class CalendarsController : Controller
	{
		public IActionResult Index()
		{
			// return View();

			// View for tests
			return View("Test");
		}
	}
}
