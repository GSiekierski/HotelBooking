using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly GoogleMapsService _googleMapsService;
        private readonly PayUService _payUService;

        public BookingController(HotelDbContext context, GoogleMapsService googleMapsService, PayUService payUService)
        {
            _context = context;
            _googleMapsService = googleMapsService;
            _payUService = payUService;
        }
        [HttpGet("create-payment")]
        public async Task<IActionResult> CreatePayment()
        {
            var accessToken = await _payUService.GetAccessTokenAsync("300746", "2ee86a66e5d97e3fadc400c9f19b065d");
            var paymentLink = await _payUService.CreateOrderAsync(accessToken);

            return Ok(new { paymentLink });
        }

        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetAll()
        {
            var bookings = _context.Bookings.Include(b => b.Room).ToList();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public ActionResult<Booking> GetById(int id)
        {
            var booking = _context.Bookings.Include(b => b.Room).FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Booking> Create(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Booking booking)
        {
            var existingBooking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (existingBooking == null)
                return NotFound();

            existingBooking.GuestName = booking.GuestName;
            existingBooking.StartDate = booking.StartDate;
            existingBooking.EndDate = booking.EndDate;
            existingBooking.RoomId = booking.RoomId;

            _context.SaveChanges();
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("location")]
        public async Task<IActionResult> GetLocationCoordinates([FromQuery] string location)
        {
            var coordinates = await _googleMapsService.GetCoordinatesAsync(location);

            if (coordinates == null)
            {
                return NotFound("Location not found");
            }

            return Ok(coordinates);
        }
    }
}
