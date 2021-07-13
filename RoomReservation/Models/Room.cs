using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class Room
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public string Description { get; set; }

		// Navigation properties
		public ICollection<Reservation> Reservations { get; set; }

		// Constructor
		public Room()
		{
			Id = Guid.NewGuid().ToString();
		}
	}
}
