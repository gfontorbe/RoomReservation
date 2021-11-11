using RoomReservation.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RoomReservation.Data.Repositories
{
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReservationRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Room> GetRoomById(string id)
        {
            return await _dbContext.Set<Room>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public override async Task<Reservation> GetByIdAsync(string id)
        {
            return await _dbContext.Reservations.Where(r => r.Id == id).Include(r => r.ReservedRoom).FirstAsync();
        }
    }
}
