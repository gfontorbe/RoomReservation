using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class ReservationViewModel
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }
		[JsonPropertyName("start")]
		public DateTime StartingTime { get; set;}
		[JsonPropertyName("end")]
		public DateTime EndingTime { get; set; }
		[JsonPropertyName("title")]
		public string Title { get; set; }
		[JsonPropertyName("editable")]
		public bool Editable { get; set; }
		[JsonPropertyName("durationEditable")]
		public bool DurationEditable { get; set; }
		[JsonPropertyName("overlap")]
		public bool Overlap { get; set; }
		[JsonIgnore]
		public string jsonData { get { return ToJson(this); } }


		public string ToJson(ReservationViewModel viewModel)
		{
			var options = new JsonSerializerOptions { WriteIndented = true };
			string jsonString = JsonSerializer.Serialize(viewModel, options);

			return jsonString;
		}
	}
}
