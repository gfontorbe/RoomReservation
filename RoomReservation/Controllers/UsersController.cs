using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
			var users = await _userManager.Users.Where(u => u.Id != loggedUser.Id).ToListAsync();

			var usersViewModels = new List<UserViewModel>();

			foreach (var user in users)
			{
				var newUser = new UserViewModel
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					UserName = user.UserName,
					Email = user.Email,
					Roles = await _userManager.GetRolesAsync(user)
				};

				usersViewModels.Add(newUser);
			}

			return View(usersViewModels);
		}
	}
}
