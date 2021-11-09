using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoomReservation.Controllers;
using System;
using Xunit;

namespace RoomReservationTests.Controllers
{
	public class HomeControllerTests
	{
		[Fact]
		public void IndexReturnsViewResult()
		{
			// Arrange
			var controller = new HomeController();
			// Act
			var result = controller.Index();

			// Assert
			Assert.IsType<ViewResult>(result);
		}
	}
}
