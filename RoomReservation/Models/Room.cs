using RoomReservation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class Room
	{
		public string Id { get; set; }
		[Required]
		[ContainsDigit]
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
