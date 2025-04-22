import axios from 'axios';

const API_URL = 'https://localhost:7036/api';

export const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});


export const register = async (username, password) => {
  return await api.post('/auth/register', { username, password });
};


export const login = async (username, password) => {
  return await api.post('/auth/login', { username, password });
};


export const getBookings = async (token) => {
  return await api.get('/booking', {
    headers: { Authorization: `Bearer ${token}` },
  });
};


export const createBooking = async (token, bookingData) => {
  return await api.post('/booking', bookingData, {
    headers: { Authorization: `Bearer ${token}` },
  });
};


export const updateBooking = async (token, bookingId, bookingData) => {
  return await api.put(`/booking/${bookingId}`, bookingData, {
    headers: { Authorization: `Bearer ${token}` },
  });
};


export const deleteBooking = async (token, bookingId) => {
  return await api.delete(`/booking/${bookingId}`, {
    headers: { Authorization: `Bearer ${token}` },
  });
};


export const createPayment = async (token) => {
  return await api.get('/booking/create-payment', {
    headers: { Authorization: `Bearer ${token}` },
  });
};


export const getLocation = async (location) => {
  return await api.get(`/booking/location?location=${location}`);
};
