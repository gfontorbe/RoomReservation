using Microsoft.AspNetCore.Identity;
using RoomReservation.Enums;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data
{
	public static class ApplicationDbInitializer
	{
		public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
			await SeedRolesAsync(context, userManager,roleManager);
			await SeedAdminAsync(context, userManager,roleManager);
			await SeedBasicUserAsync(context, userManager,roleManager);
		}

		private static Task SeedBasicUserAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			throw new NotImplementedException();
		}

		private static async Task SeedAdminAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			var adminUser = new ApplicationUser
			{
				UserName = "admin",
				Email = "admin@company.com",
				EmailConfirmed = true,
				FirstName = "Admin",
				LastName = "User"
			};

			if (userManager.Users.All(u => u.Id != adminUser.Id))
			{
				var user = userManager.FindByEmailAsync(adminUser.Email);
				if (user == null)
				{
					await userManager.CreateAsync(adminUser, "Pass123!");
					await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
					await userManager.AddToRoleAsync(adminUser, Roles.Basic.ToString());
				}
			}
		}

		private static async Task SeedRolesAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (!context.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
				await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
			}
		}
	}
}
