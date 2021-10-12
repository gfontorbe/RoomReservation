using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class Reservation
	{
		// Properties
		public string Id { get; set; }
		public DateTime StartingTime { get; set; }
		public DateTime EndingTime { get; set; }

		// Navigation Properties
		public ApplicationUser ReservingUser { get; set; }
		public Room ReservedRoom { get; set; }

		// Constructor
		public Reservation()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}
