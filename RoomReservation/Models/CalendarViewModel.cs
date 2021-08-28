using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class CalendarViewModel
	{
		public string JSONData { get; set; }
		public Reservation ReservationToCreate { get; set; }
		public Reservation ReservationToEdit { get; set; }
		public Room Room { get; set; }
	}
}
