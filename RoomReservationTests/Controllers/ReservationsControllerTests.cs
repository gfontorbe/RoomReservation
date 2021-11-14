using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomReservation.Controllers;
using RoomReservation.Data.Repositories;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Security.Claims;
using System.Threading;

namespace RoomReservationTests.Controllers
{
    public class ReservationsControllerTests
    {
        private List<Reservation> _reservationData;
        private List<Room> _roomData;

        private Mock<IReservationRepository> _mockReservationRepository;

        private UserManager<ApplicationUser> userManager;

        public ReservationsControllerTests()
        {
            _reservationData = GetReservationData();
            _roomData = GetRoomsData();

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            mockUserStore.Setup(x => x.FindByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(GetUsersData()[0]);

            userManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            _mockReservationRepository = new Mock<IReservationRepository>();
            _mockReservationRepository.Setup(x => x.GetRoomById(It.IsAny<string>()))
                .ReturnsAsync((string id) => _roomData.SingleOrDefault(x => x.Id == id));
            _mockReservationRepository.Setup(x => x.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => _reservationData.SingleOrDefault(x => x.Id == id));
            _mockReservationRepository.Setup(x => x.AddAsync(It.IsAny<Reservation>())).Callback((Reservation reservation) => _reservationData.Add(reservation));
            _mockReservationRepository.Setup(x => x.UpdateAsync(It.IsAny<Reservation>())).Callback((Reservation reservation) => _reservationData[_reservationData.FindIndex(r => r.Id == reservation.Id)] = reservation);
            _mockReservationRepository.Setup(x => x.DeleteAsync(It.IsAny<Reservation>())).Callback((Reservation reservation) => _reservationData.RemoveAt(_reservationData.FindIndex(r => r.Id == reservation.Id)));
        }

        private ReservationsController CreateReservationsController()
        {
            return new ReservationsController(
               userManager,
                _mockReservationRepository.Object);
        }

        [Fact]
        public async Task Create_WhenValidModel_ReturnsRedirectToRoute()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user1"),
            }, "mock"));

            var reservationsController = CreateReservationsController();
            reservationsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            CalendarViewModel calendarViewModel = new CalendarViewModel { Room = GetRoomsData()[0], ReservationToCreate = new Reservation(), ReservationToEdit = new Reservation(), JSONData = null };

            // Act
            var result = await reservationsController.Create(
                calendarViewModel);

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
        }

        [Fact]
        public async Task Create_WhenValidModel_AddModelToRepo()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "user1"),
            }, "mock"));

            var reservationsController = CreateReservationsController();
            reservationsController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user
                }
            };

            CalendarViewModel calendarViewModel = new CalendarViewModel { Room = GetRoomsData()[0], ReservationToCreate = new Reservation(), ReservationToEdit = new Reservation(), JSONData = null };

            // Act
            var result = await reservationsController.Create(
                calendarViewModel);

            // Assert
            Assert.Equal(5, _reservationData.Count);
        }

        [Fact]
        public async Task Edit_RedirectsToRoute()
        {
            // Arrange
            var reservationsController = CreateReservationsController();
            var calendarViewModel = new CalendarViewModel {ReservationToEdit = GetReservationData()[0], Room = GetRoomsData()[0] };

            // Act
            var result = await reservationsController.Edit(calendarViewModel);

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
        }

        [Fact]
        public async Task Edit_WhenTimeIsChanged_UpdatesRepo()
        {
            // Arrange
            var reservationsController = CreateReservationsController();
            var calendarViewModel = new CalendarViewModel
            {
                ReservationToEdit = new Reservation
                {
                    Id = GetReservationData().First().Id,
                    StartingTime = GetReservationData().First().StartingTime,
                    EndingTime = GetReservationData().First().EndingTime,
                    ReservedRoom = GetReservationData().First().ReservedRoom,
                    ReservingUser = GetReservationData().First().ReservingUser
                },
                Room = GetRoomsData().First()
            };

            // Act
            calendarViewModel.ReservationToEdit.StartingTime = calendarViewModel.ReservationToEdit.StartingTime.AddMinutes(1);

            var result = reservationsController.Edit(calendarViewModel);

            // Assert
            Assert.Equal(calendarViewModel.ReservationToEdit.StartingTime, _reservationData[0].StartingTime);
        }
        
        [Fact]
        public async Task Delete_ReturnsRedirectToRoute()
        {
            // Arrange
            var reservationsController = CreateReservationsController();
            string id = _reservationData[0].Id;

            // Act
            var result = await reservationsController.Delete(id);

            // Assert
            Assert.IsType<RedirectToRouteResult>(result);
        }

        [Fact]
        public async Task Delete_WhenValidId_RemovesReservationFromRepo()
        {
            // Arrange
            var reservationsController = CreateReservationsController();
            string id = _reservationData[0].Id;

            // Act
            var result = await reservationsController.Delete(id);

            // Assert
            Assert.Equal(-1, _reservationData.FindIndex(r => r.Id == id));
        }
        
        private List<Reservation> GetReservationData()
        {
            List<Reservation> reservations = new List<Reservation>();

            reservations.Add(new Reservation { Id = "1", StartingTime = DateTime.Now, EndingTime = DateTime.Now.AddHours(2), ReservedRoom = GetRoomsData()[0], ReservingUser = GetUsersData()[0] });
            reservations.Add(new Reservation { Id = "2", StartingTime = DateTime.Now.AddDays(1), EndingTime = DateTime.Now.AddDays(1).AddHours(2), ReservedRoom = GetRoomsData()[0], ReservingUser = GetUsersData()[1] });
            reservations.Add(new Reservation { Id = "3", StartingTime = DateTime.Now, EndingTime = DateTime.Now.AddHours(2), ReservedRoom = GetRoomsData()[0], ReservingUser = GetUsersData()[1] });
            reservations.Add(new Reservation { Id = "4", StartingTime = DateTime.Now.AddDays(1), EndingTime = DateTime.Now.AddDays(1).AddHours(2), ReservedRoom = GetRoomsData()[1], ReservingUser = GetUsersData()[1] });
        
            return reservations;
        }

            private List<Room> GetRoomsData()
        {
            List<Room> rooms = new List<Room>();

            rooms.Add(new Room { Id = "1", Name = "Room1", Description = "Room1", Location = "Room1", Reservations = null });

            rooms.Add(new Room { Id = "2", Name = "Room2", Description = "Room2", Location= "Room2", Reservations = null});

            return rooms;
        } 
        
        private List<ApplicationUser> GetUsersData()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();

            users.Add(new ApplicationUser { Id = "1", FirstName = "user1", LastName = "user1", UserName = "user1" }); 
            users.Add(new ApplicationUser { Id = "2", FirstName = "user2", LastName = "user2", UserName = "user2" });

            return users;
        }
    }
}
