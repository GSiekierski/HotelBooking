using HotelBookingAPI.Models;

namespace HotelBookingAPI.Services
{
    public class RoomService
    {
        private readonly List<Room> _rooms = new();

        public RoomService()
        {
            // dane przykładowe
            _rooms.Add(new Room { Id = 1, Name = "Pokój jednoosobowy", Capacity = 1, PricePerNight = 150 });
            _rooms.Add(new Room { Id = 2, Name = "Pokój dwuosobowy", Capacity = 2, PricePerNight = 250 });
        }

        public IEnumerable<Room> GetAll() => _rooms;
        public Room? GetById(int id) => _rooms.FirstOrDefault(r => r.Id == id);
    }
}
