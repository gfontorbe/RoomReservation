using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class UserViewModel
	{
		public string Id { get; set; }
		[DisplayName("First Name")]
		public string FirstName { get; set; }
		[DisplayName("Last Name")]
		public string LastName { get; set; }
		[DisplayName("Username")]
		public string UserName { get; set; }
		[DisplayName("Email address")]
		public string Email { get; set; }
		public IEnumerable<string> Roles { get; set; }
	}
}
