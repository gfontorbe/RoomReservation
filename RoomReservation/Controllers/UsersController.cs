using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
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

		public async Task<IActionResult> Index(string? searchString)
		{
			ViewData["SearchFilter"] = searchString;

			var loggedUser = await _userManager.GetUserAsync(HttpContext.User);
			var users = await _userManager.Users.Where(u => u.Id != loggedUser.Id).OrderBy(u => u.UserName).ToListAsync();

			if (!String.IsNullOrEmpty(searchString))
			{
				users = users.Where(
					x => x.FirstName.ToLower().Contains(searchString.ToLower())
				|| x.LastName.ToLower().Contains(searchString.ToLower())
				|| x.Email.ToLower().Contains(searchString.ToLower())
				|| x.UserName.ToLower().Contains(searchString.ToLower()))
					.ToList();

			}

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

		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			var userVM = new UserViewModel
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Email = user.Email
			};

			return View(userVM);
		}

		public async Task<IActionResult> Update(string id, UserViewModel model)
		{
			var user = await _userManager.FindByIdAsync(id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.UserName = model.UserName;
			user.Email = model.Email;

			await _userManager.UpdateAsync(user);

			return RedirectToAction("Index");
		}
	}
}
