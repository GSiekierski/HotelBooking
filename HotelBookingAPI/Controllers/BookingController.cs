using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Models;
using HotelBookingAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Booking>> GetAll()
        {
            return Ok(_bookingService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Booking> GetById(int id)
        {
            var booking = _bookingService.GetById(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [Authorize]
        [HttpPost]
        public ActionResult<Booking> Create(Booking booking)
        {
            var created = _bookingService.Add(booking);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Booking booking)
        {
            if (!_bookingService.Update(id, booking))
                return NotFound();

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_bookingService.Delete(id))
                return NotFound();

            return NoContent();
        }
    }
}
