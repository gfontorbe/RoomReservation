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
	public class UserRolesController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
		}

		public async Task<IActionResult> Index(string userId)
		{
			var userRolesVM = new List<UserRoleViewModel>();
			var user = await _userManager.FindByIdAsync(userId);

			var roles = await _roleManager.Roles.ToListAsync();

			foreach (var role in roles)
			{
				var userRole = new UserRoleViewModel
				{
					RoleName = role.Name
				};

				if(await _userManager.IsInRoleAsync(user, role.Name))
				{
					userRole.Selected = true;
				}
				else
				{
					userRole.Selected = false;
				}


				userRolesVM.Add(userRole);
			}

			var model = new ManageUserRolesViewModel()
			{
				UserId = user.Id,
				Username = user.UserName,
				UserRoles = userRolesVM
			};

			return View(model);
		}

		public async Task<IActionResult> Update(string id, ManageUserRolesViewModel model)
		{
			var user = await _userManager.FindByIdAsync(id);
			var roles = await _userManager.GetRolesAsync(user);
			var result = await _userManager.RemoveFromRolesAsync(user, roles);
			result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(r => r.Selected).Select(r => r.RoleName));
			var currentUser = await _userManager.GetUserAsync(User);
			await _signInManager.RefreshSignInAsync(currentUser);

			return RedirectToRoute(routeName: "return", routeValues: new { controller = "UserRoles", action = "Index", userId = id });

		}
	}
}
