namespace HotelBookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //TODO: status płatności
    }
}
