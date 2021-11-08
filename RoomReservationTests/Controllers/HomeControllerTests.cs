using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoomReservation.Controllers;
using System;
using Xunit;

namespace RoomReservationTests
{
	public class HomeControllerTests
	{
		private readonly HomeController _controller;

		public HomeControllerTests()
		{
			_controller = new HomeController();
		}

		[Fact]
		public void IndexReturnsViewResult()
		{
			var result = _controller.Index();

			Assert.IsType<ViewResult>(result);
		}
	}
}
