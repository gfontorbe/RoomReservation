using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomReservation.Controllers;
using RoomReservation.Data.Repositories;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoomReservationTests.Controllers
{
    public class RoomsControllerTests
    {
        private readonly Mock<IRepository<Room>> _repo;
        public RoomsControllerTests()
        {
            _repo = new Mock<IRepository<Room>>();
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(GetTestData());
            _repo.Setup<Task<Room>>(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((string id) => GetTestData().SingleOrDefault(r => r.Id == id));
        }


        #region Index
        [Fact]
        public async void Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Index(null);
            
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
        [Fact]
        public async void Index_ModelOfType_ListOfRoom()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Index(null);
            var viewResult = Assert.IsType<ViewResult>(result);
            // Assert
            var model = Assert.IsAssignableFrom<List<Room>>(viewResult.ViewData.Model);
        }

        [Fact]
        public async void Index_ModelContains_AllRoomsFromRepo()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Index(null);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Room>>(viewResult.ViewData.Model);
            
            // Assert
            Assert.Equal(3,model.Count);
        }

        [Theory]
        [InlineData("roomtest1")]
        [InlineData("RoOmTeSt1")]
        [InlineData("ROOMTEST1")]
        [InlineData("location1")]
        [InlineData("LOCATION1")]
        [InlineData("LoCaTiOn1")]
        [InlineData("description1")]
        [InlineData("DESCRIPTION1")]
        [InlineData("DeScriPtion1")]
        public async Task Index_ModelFilter_FilterByName(string searchString)
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            // Act
            var result = await controller.Index(searchString);
            var viewResult = Assert.IsType<ViewResult>(result);
            
            var model = Assert.IsAssignableFrom<List<Room>>(viewResult.ViewData.Model);

            // Assert
            Assert.Single(model);
        }

        [Fact]
        public async Task Index_ModelFilter_EmptyModel_WhenNoCorrespondingSearchString()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            string searchString = "abcdefghi";

            // Act
            var result = await controller.Index(searchString);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<List<Room>>(viewResult.ViewData.Model);

            // Assert
            Assert.Empty(model);
        }
        #endregion

        #region Details
        [Fact]
        public async void Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            
        }

        [Fact]
        public async void Details_ReturnsNotFound_WhenIdDoesNotExist()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Details("This is an invalid ID");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Details_ReturnsViewResult_WhenIsFound()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = await controller.Details("89bb2b92-7c90-441c-b293-9fc0f29fc20e");

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        #endregion
        private List<Room> GetTestData()
        {
            var data = new List<Room>();

            data.Add(new Room
            {
                Id = "89bb2b92-7c90-441c-b293-9fc0f29fc20e",
                Name = "RoomTest1",
                Location = "Location1",
                Description = "Description1"
            });

            data.Add(new Room
            {
                Id = "02a00c3e-f82b-4cf2-9c1b-f3991d1c4f6c",
                Name = "RoomTest2",
                Location = "Location2",
                Description = "Description2"
            });
            
            data.Add(new Room
            {
                Id = "54f69ac8-26c1-462a-9e53-15da1f4470d8",
                Name = "RoomTest3",
                Location = "Location3",
                Description = "Description3"
            });

            return data;
        }
    }
}
