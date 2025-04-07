using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Models;
using HotelBookingAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class RoomController : ControllerBase
{
    private readonly RoomService _roomService;

    public RoomController(RoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Room>> GetAll()
    {
        return Ok(_roomService.GetAll());
    }

    [HttpGet("{id}")]
    public ActionResult<Room> GetById(int id)
    {
        var room = _roomService.GetById(id);
        if (room == null)
            return NotFound();

        return Ok(room);
    }
}
