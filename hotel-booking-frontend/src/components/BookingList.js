import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';

const BookingList = ({ token }) => {
  const [bookings, setBookings] = useState([]);

  useEffect(() => {
    axios
      .get('https://localhost:7036/api/booking', {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        setBookings(response.data);
      })
      .catch((error) => {
        console.error('Error fetching bookings:', error);
      });
  }, [token]);

  const handleDelete = (id) => {
    axios
      .delete(`https://localhost:7036/api/booking/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then(() => {
        setBookings(bookings.filter((booking) => booking.id !== id));
      })
      .catch((error) => {
        console.error('Error deleting booking:', error);
      });
  };

  const handleCreatePayment = () => {
    axios
      .get('https://localhost:7036/api/Booking/create-payment', {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
      .then((response) => {
        const paymentLink = response.data.paymentLink;
        if (paymentLink) {
          window.open(paymentLink, '_blank');
        } else {
          alert('No payment link returned.');
        }
      })
      .catch((error) => {
        alert('Error creating payment.');
        console.error('Payment error:', error);
      });
  };

  return (
    <div>
      <h2>Bookings</h2>
      <ul>
        {bookings.map((booking) => (
          <li key={booking.id}>
            <strong>{booking.guestName}</strong> - {booking.room.name} - {booking.startDate} to {booking.endDate}
            <div>
              <Link to={`/update-booking/${booking.id}`} className="btn btn-warning">Update</Link>
              <button onClick={() => handleDelete(booking.id)} className="btn btn-danger">Delete</button>
              <button onClick={handleCreatePayment} className="btn btn-success">Create Payment</button>
            </div>
          </li>
        ))}
      </ul>

      <div>
        <Link to="/map" className="btn btn-info">View Map</Link>
      </div>
    </div>
  );
};

export default BookingList;
