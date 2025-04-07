using HotelBookingAPI.Models;

namespace HotelBookingAPI.Services
{
    public class BookingService
    {
        private readonly List<Booking> _bookings = new();
        private int _nextId = 1;

        public IEnumerable<Booking> GetAll() => _bookings;

        public Booking? GetById(int id) => _bookings.FirstOrDefault(b => b.Id == id);

        public Booking Add(Booking booking)
        {
            booking.Id = _nextId++;
            _bookings.Add(booking);
            return booking;
        }

        public bool Update(int id, Booking updated)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                return false;
            }

            existing.RoomId = updated.RoomId;
            existing.GuestName = updated.GuestName;
            existing.StartDate = updated.StartDate;
            existing.EndDate = updated.EndDate;
            return true;

        }

        public bool Delete(int id) { 
            var booking = GetById(id);
            if (booking == null) return false;

            _bookings.Remove(booking);
            return true;
        }
    }
}
