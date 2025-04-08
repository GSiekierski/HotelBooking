using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Range(1, 100, ErrorMessage = "wrong id")]
        public int RoomId { get; set; }

        [Required(ErrorMessage ="name must be set")]
        public string GuestName { get; set; } = string.Empty;
        
        [Required(ErrorMessage ="start date is required")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "start date is required")]
        public DateTime EndDate { get; set; }

        public Room? Room { get; set; }
    }
}
