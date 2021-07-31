using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class ReservationViewModel
	{
		public string Id { get; set; }
		public DateTime StartingTime { get; set;}
		public DateTime EndingTime { get; set; }
		public string Title { get; set; }
		public bool Editable { get; set; }
		public bool DurationEditable { get; set; }
		public bool Overlap { get; set; }

	}
}
