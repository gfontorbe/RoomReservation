using Microsoft.EntityFrameworkCore.Metadata;
using RoomReservation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class Room : IAppModel
	{
		public string Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Location { get; set; }
		[Required]
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
