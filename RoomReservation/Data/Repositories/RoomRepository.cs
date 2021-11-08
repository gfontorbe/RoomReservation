using Microsoft.EntityFrameworkCore;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data.Repositories
{
	public class RoomRepository : IRepository<Room>
	{
		private readonly ApplicationDbContext _dbContext;

		public RoomRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public Task<Room> GetByIdAsync(string id)
		{
			return _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == id);
		}

		public Task<List<Room>> GetAllAsync()
		{
			return _dbContext.Rooms.ToListAsync();
		}

		public Task AddAsync(Room room)
		{
			_dbContext.Rooms.Add(room);
			return _dbContext.SaveChangesAsync();
		}

		public Task UpdateAsync(Room room)
		{
			_dbContext.Entry(room).State = EntityState.Modified;
			return _dbContext.SaveChangesAsync();
		}

		public Task DeleteAsync(Room room)
		{
			_dbContext.Entry(room).State = EntityState.Deleted;
			return _dbContext.SaveChangesAsync();
		}
	}
}
