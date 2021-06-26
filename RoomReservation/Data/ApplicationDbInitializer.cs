using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data
{
	public static class ApplicationDbInitializer
	{
		public static void Initialize(ApplicationDbContext context) {

			if (!context.Roles.Any())
			{
				var roles = new IdentityRole[]
				{
					new IdentityRole{Name="Admin", NormalizedName="ADMIN"},
					new IdentityRole{Name="User", NormalizedName="USER"}
				};

				context.Roles.AddRange(roles);

				context.SaveChanges();
			}
		}
	}
}
