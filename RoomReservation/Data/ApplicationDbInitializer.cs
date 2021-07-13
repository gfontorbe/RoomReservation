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
			await SeedRolesAsync(roleManager);
			await SeedAdminAsync(userManager);
			await SeedBasicUserAsync(userManager);
			await SeedRolesAsync(context);
		}

		private static async Task SeedRolesAsync(ApplicationDbContext context)
		{
			if (!context.Rooms.Any())
			{
				var rooms = new Room[]
				{
					new Room{ Name = "Big Conference Room", Location="Building A Room 022", Description="Large conference room in the main building"},
					new Room{ Name = "Small Conference Room", Location="Building A Room 135", Description="Small conference room in the main building"},
					new Room{ Name = "East Conference Room", Location="Building C Room 007", Description="Conference room in the East building"}
				};

				await context.Rooms.AddRangeAsync(rooms);
				await context.SaveChangesAsync();
			}
		}

		private static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager)
		{
			var basicUser = new ApplicationUser
			{
				UserName = "basicUser",
				Email = "basic@company.com",
				EmailConfirmed = true,
				FirstName = "Basic",
				LastName = "User"
			};

			if (userManager.Users.All(u => u.Id != basicUser.Id))
			{
				var user = await userManager.FindByEmailAsync(basicUser.Email);
				if (user == null)
				{
					await userManager.CreateAsync(basicUser, "Pass123!");
					await userManager.AddToRoleAsync(basicUser, Roles.Basic.ToString());
				}
			}
		}

		private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
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
				var user = await userManager.FindByEmailAsync(adminUser.Email);
				if (user == null)
				{
					await userManager.CreateAsync(adminUser, "Pass123!");
					await userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString());
					await userManager.AddToRoleAsync(adminUser, Roles.Basic.ToString());
				}
			}
		}

		private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
				await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
			}
		}
	}
}
