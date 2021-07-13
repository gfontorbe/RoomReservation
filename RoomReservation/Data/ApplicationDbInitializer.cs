using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data
{
	public static class ApplicationDbInitializer
	{
		public static async Task Initialize(ApplicationDbContext context) {
			await SeedRolesAsync(context);
			await SeedAdminAsync(context);
			await SeedBasicUserAsync(context);
		}

		private static Task SeedBasicUserAsync(ApplicationDbContext context)
		{
			throw new NotImplementedException();
		}

		private static Task SeedAdminAsync(ApplicationDbContext context)
		{
			throw new NotImplementedException();
		}

		private static Task SeedRolesAsync(ApplicationDbContext context)
		{
			throw new NotImplementedException();
		}
	}
}
