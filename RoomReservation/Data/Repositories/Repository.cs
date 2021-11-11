using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RoomReservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data.Repositories
{
	public class Repository<T> : IRepository<T> where T : class, IAppModel
	{
		private readonly ApplicationDbContext _dbContext;

		public Repository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}
		public Task AddAsync(T model)
		{
			_dbContext.Set<T>().AddAsync(model);
			return _dbContext.SaveChangesAsync();
		}

		public Task DeleteAsync(T model)
		{
			_dbContext.Entry(model).State = EntityState.Deleted;
			return _dbContext.SaveChangesAsync();
		}

		public Task<List<T>> GetAllAsync()
		{
			return _dbContext.Set<T>().ToListAsync();
		}

		public virtual Task<T> GetByIdAsync(string id)
		{
			return _dbContext.Set<T>().FirstOrDefaultAsync(m => m.Id == id);
		}

		public Task UpdateAsync(T model)
		{
			_dbContext.Entry(model).State = EntityState.Modified;
			return _dbContext.SaveChangesAsync();
		}
	}
}
