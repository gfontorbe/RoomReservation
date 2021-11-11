using RoomReservation.Models;
using System.Threading.Tasks;

namespace RoomReservation.Data.Repositories
{
    public interface IReservationRepository : IRepository<Reservation>
    {
        Task<Room> GetRoomById(string id);
    }
}