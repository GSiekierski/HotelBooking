import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';

const BookingUpdate = ({ token }) => {
  const [booking, setBooking] = useState({});
  const [rooms, setRooms] = useState([]);
  const [guestName, setGuestName] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get(`https://localhost:7036/api/booking/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        const bookingData = response.data;
        setBooking(bookingData);
        setGuestName(bookingData.guestName);
        setStartDate(bookingData.startDate);
        setEndDate(bookingData.endDate);
      })
      .catch((error) => {
        console.error('Error fetching booking:', error);
      });

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
  }, [id, token]);

  const handleSubmit = (event) => {
    event.preventDefault();

    const updatedBooking = {
      guestName,
      roomId: booking.room.id,
      startDate,
      endDate,
    };

    axios
      .put(`https://localhost:7036/api/booking/${id}`, updatedBooking, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then(() => {
        navigate('/');
      })
      .catch((error) => {
        console.error('Error updating booking:', error);
      });
  };

  return (
    <div>
      <h2>Update Booking</h2>
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
            value={booking.room?.id || ''}
            onChange={(e) => {
              const selectedRoom = rooms.find((room) => room.id === parseInt(e.target.value));
              setBooking({ ...booking, room: selectedRoom });
            }}
          >
            {rooms.map((room) => (
              <option key={room.id} value={room.id}>
                {room.name}
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
        <button type="submit" className="btn btn-primary">Update Booking</button>
      </form>
    </div>
  );
};

export default BookingUpdate;
