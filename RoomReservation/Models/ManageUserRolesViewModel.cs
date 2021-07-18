using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Models
{
	public class ManageUserRolesViewModel
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public IList<UserRoleViewModel> UserRoles { get; set; }
	}
}
