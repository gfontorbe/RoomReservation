using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoomReservation.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.HasDefaultSchema("Identity");

			// TODO: replace with custom user
			// users table
			builder.Entity<IdentityUser>(e => e.ToTable("ApplicationUser"));

			// roles table
			builder.Entity<IdentityRole>(e => e.ToTable("Roles"));

			// userRoles table
			builder.Entity<IdentityUserRole<string>>(e => e.ToTable("UserRoles"));

			// userClaims table
			builder.Entity<IdentityUserClaim<string>>(e => e.ToTable("UserClaims"));

			// userLogin table
			builder.Entity<IdentityUserLogin<string>>(e => e.ToTable("UserLogins"));

			// roleClaims table
			builder.Entity<IdentityRoleClaim<string>>(e => e.ToTable("RoleClaims"));

			// userTokens table
			builder.Entity<IdentityUserToken<string>>(e => e.ToTable("UserTokens"));
		}
	}
}
