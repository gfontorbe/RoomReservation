using RoomReservation.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class Reservation : IAppModel
	{
		// Properties
		public string Id { get; set; }
		[Required (ErrorMessage = "Starting Time Required")]
		[DisplayName("Starting Time")]
		public DateTime StartingTime { get; set; }
		[Required (ErrorMessage = "Ending Time Required")]
		//[IsDateAfter (nameof(StartingTime), false)]
		[TestDate]
		[DisplayName("Ending Time")]
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
