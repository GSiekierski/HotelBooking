import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const BookingForm = ({ token, onBookingCreated }) => {
  const [rooms, setRooms] = useState([]);
  const [guestName, setGuestName] = useState('');
  const [roomId, setRoomId] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get('https://localhost:7036/api/rooms', {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        setRooms(response.data);
      })
      .catch((error) => {
        console.error('Error fetching rooms:', error);
      });
  }, [token]);

  const handleSubmit = (event) => {
    event.preventDefault();

    const newBooking = {
      guestName,
      roomId,
      startDate,
      endDate,
    };

    axios
      .post('https://localhost:7036/api/booking', newBooking, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then(() => {
        if (onBookingCreated) onBookingCreated();
        navigate('/');
      })
      .catch((error) => {
        console.error('Error creating booking:', error);
      });
  };

  return (
    <div>
      <h2>Create Booking</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="guestName">Guest Name</label>
          <input
            type="text"
            id="guestName"
            className="form-control"
            value={guestName}
            onChange={(e) => setGuestName(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label htmlFor="room">Room</label>
          <select
            id="room"
            className="form-control"
            value={roomId}
            onChange={(e) => setRoomId(e.target.value)}
          >
            <option value="">Select a room</option>
            {rooms.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name} - {room.description}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="startDate">Start Date</label>
          <input
            type="date"
            id="startDate"
            className="form-control"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label htmlFor="endDate">End Date</label>
          <input
            type="date"
            id="endDate"
            className="form-control"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
          />
        </div>

        <button type="submit" className="btn btn-primary">Create Booking</button>
      </form>
    </div>
  );
};

export default BookingForm;
