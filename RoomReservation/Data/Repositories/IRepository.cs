using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomReservation.Data.Repositories
{
	public interface IRepository<T>
	{
		Task<T> GetByIdAsync(string id);
		Task<List<T>> GetAllAsync();
		Task AddAsync(T model);
		Task UpdateAsync(T model);
		Task DeleteAsync(T model);
	}
}
