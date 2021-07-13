using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomReservation.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Reservation> Reservations { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			//builder.HasDefaultSchema("Identity");

			// TODO: replace with custom user
			// users table
			builder.Entity<ApplicationUser>(e => e.ToTable("ApplicationUsers","Identity"));

			// roles table
			builder.Entity<IdentityRole>(e => e.ToTable("Roles", "Identity"));

			// userRoles table
			builder.Entity<IdentityUserRole<string>>(e => e.ToTable("UserRoles", "Identity"));

			// userClaims table
			builder.Entity<IdentityUserClaim<string>>(e => e.ToTable("UserClaims", "Identity"));

			// userLogin table
			builder.Entity<IdentityUserLogin<string>>(e => e.ToTable("UserLogins", "Identity"));

			// roleClaims table
			builder.Entity<IdentityRoleClaim<string>>(e => e.ToTable("RoleClaims", "Identity"));

			// userTokens table
			builder.Entity<IdentityUserToken<string>>(e => e.ToTable("UserTokens", "Identity"));


			// Rooms model table
			builder.Entity<Room>(e => e.ToTable("Rooms"));

			// Reservations model table
			builder.Entity<Reservation>(e => e.ToTable("Reservations"));
		}
	}
}
