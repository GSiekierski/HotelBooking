import React, { useState } from 'react';
import { BrowserRouter as Router, Route, Routes, Link } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import BookingList from './components/BookingList';
import BookingForm from './components/BookingForm';
import BookingUpdate from './components/BookingUpdate';
import MapComponent from './components/MapComponent';

const App = () => {
  const [token, setToken] = useState(localStorage.getItem('token'));

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken(null);
    window.location.href = '/';
  };

  return (
    <Router>
      <div className="container">
        <nav>
          <ul className="nav">
            {!token ? (
              <>
                <li className="nav-item">
                  <Link className="btn btn-primary" to="/login">Login</Link>
                </li>
                <li className="nav-item">
                  <Link className="btn btn-secondary" to="/register">Register</Link>
                </li>
              </>
            ) : (
              <>
                <li className="nav-item">
                  <Link className="btn btn-primary" to="/">Home</Link>
                </li>
                <li className="nav-item">
                  <Link className="btn btn-secondary" to="/create-booking">Create Booking</Link>
                </li>
                <li className="nav-item">
                  <button className="btn btn-danger" onClick={handleLogout}>Logout</button>
                </li>
              </>
            )}
          </ul>
        </nav>

        <Routes>
          {!token ? (
            <>
              <Route path="/login" element={<Login onLogin={setToken} />} />
              <Route path="/register" element={<Register onRegister={() => window.location.href = '/login'} />} />
            </>
          ) : (
            <>
              <Route path="/" element={<BookingList token={token} />} />
              <Route path="/create-booking" element={<BookingForm token={token} onBookingCreated={() => window.location.href = '/'} />} />
              <Route path="/update-booking/:id" element={<BookingUpdate token={token} />} />
              <Route path="/map" element={<MapComponent token={token} />} />
            </>
          )}
        </Routes>
      </div>
    </Router>
  );
};

export default App;
