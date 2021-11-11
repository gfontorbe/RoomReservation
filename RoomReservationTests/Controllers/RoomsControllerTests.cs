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
        private readonly List<Room> _data;
        private readonly Mock<IRepository<Room>> _repo;
        public RoomsControllerTests()
        {
            _data = GetTestData();

            _repo = new Mock<IRepository<Room>>();
            _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(_data);
            _repo.Setup<Task<Room>>(r => r.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((string id) => _data.SingleOrDefault(r => r.Id == id));
            _repo.Setup(r => r.AddAsync(It.IsAny<Room>())).Callback((Room room) => _data.Add(room));
            _repo.Setup(r => r.UpdateAsync(It.IsAny<Room>())).Callback((Room room) => _data[_data.FindIndex(r => r.Id == room.Id)] = room);
            _repo.Setup(r => r.DeleteAsync(It.IsAny<Room>())).Callback((Room room) => _data.RemoveAt(_data.FindIndex(r => r.Id == room.Id)));
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
            var result = (ViewResult)await controller.Index(null);
            //var viewResult = Assert.IsType<ViewResult>(result);
            // Assert
            var model = Assert.IsAssignableFrom<List<Room>>(result.ViewData.Model);
        }

        [Fact]
        public async void Index_ModelContains_AllRoomsFromRepo()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = (ViewResult)await controller.Index(null);
            var model = (List<Room>)result.ViewData.Model;
            
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
            var result = (ViewResult)await controller.Index(searchString);
            var model = (List<Room>)result.ViewData.Model;

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
            var result = (ViewResult)await controller.Index(searchString);
            var model = (List<Room>)result.ViewData.Model;

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
        [Fact]
        public async void Details_ReturnsModelOfTypeRoom()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);

            // Act
            var result = (ViewResult)await controller.Details("89bb2b92-7c90-441c-b293-9fc0f29fc20e");
            var model = result.ViewData.Model;
            // Assert
            Assert.IsType<Room>(model);
        }

        [Fact]
        public async Task Details_ReturnsModelWithId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            // Act
            var result = (ViewResult)await controller.Details("89bb2b92-7c90-441c-b293-9fc0f29fc20e");
            var model = (Room)result.ViewData.Model;
            // Assert
            Assert.Equal(GetTestData()[0].Id, model.Id);
            Assert.Equal(GetTestData()[0].Description, model.Description);
            Assert.Equal(GetTestData()[0].Location, model.Location);
            Assert.Equal(GetTestData()[0].Name, model.Name);
            Assert.Equal(GetTestData()[0].Reservations, model.Reservations);
        }
        #endregion

        #region Create

        [Fact]
        public void Create_ReturnsView()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            // Act
            var result = controller.Create();
            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_AddModelToRepo()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var model = new Room
            {
                Id = Guid.NewGuid().ToString(),
                Name = "NewRoom",
                Description = "New Description",
                Location = "New Location",
                Reservations = null
            };
            // Act
            await controller.Create(model);
            // Assert
            Assert.Equal(4, _data.Count);
        }

        [Fact]
        public async Task Create_DoesNotAddInvalidModelToRepo()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var model = new Room();
            controller.ModelState.AddModelError("invalidModel", "The model is invalid");
            // Act
            await controller.Create(model);
            // Assert
            Assert.Equal(3,_data.Count);
        }

        #endregion

        #region Edit
        [Theory]
        [InlineData(null)]
        [InlineData("This is an invalid ID")]
        public async Task Edit_ReturnNotFoundWhenWrongId(string id)
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            // Act
            var result = await controller.Edit(id);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnViewResultWhenValidId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = "89bb2b92-7c90-441c-b293-9fc0f29fc20e";

            // Act
            var result = await controller.Edit(id);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Edit_ReplaceModelWithId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = _data[0].Id;
            var model = new Room
            {
                Id = id,
                Name = "Edited Name",
                Description = "Edited Description", Location = "Edited Location", Reservations = _data[0].Reservations
            };
            // Act
            var result = controller.Edit(id, model);

            // Assert
            Assert.Equal(_data[0].Id, model.Id);
            Assert.Equal(_data[0].Name, model.Name);
            Assert.Equal(_data[0].Description, model.Description);
            Assert.Equal(_data[0].Location, model.Location);
            Assert.Equal(_data[0].Reservations, model.Reservations);
        }

        [Fact]
        public async Task Edit_ReturnNotFoundWhenIdDifferentFromModelId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = _data[0].Id;
            var model = new Room { Id = Guid.NewGuid().ToString(), Name = null, Description = null, Location = null ,Reservations = null };
            // Act
            var result = await controller.Edit(id, model);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion

        #region Delete
        [Theory]
        [InlineData(null)]
        [InlineData("This is an invalid ID")]
        public async Task Delete_ReturnsNotFound_WhenWrongId(string id)
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            // Act
            var result = await controller.Delete(id);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WhenValidId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = "89bb2b92-7c90-441c-b293-9fc0f29fc20e";
            // Act
            var result = await controller.Delete(id);
            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesFromRepo_WhenValidId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = "89bb2b92-7c90-441c-b293-9fc0f29fc20e";
            // Act
            var result = await controller.DeleteConfirmed(id);
            // Assert
            Assert.Equal(2, _data.Count);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToRoute_WhenValidId()
        {
            // Arrange
            var controller = new RoomsController(_repo.Object);
            var id = "89bb2b92-7c90-441c-b293-9fc0f29fc20e";
            // Act
            var result = await controller.DeleteConfirmed(id);
            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
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
