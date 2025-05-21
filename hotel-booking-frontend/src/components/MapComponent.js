import React, { useState } from 'react';
import axios from 'axios';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';

const MapComponent = ({ token }) => {
  const [location, setLocation] = useState({ lat: 52.2296756, lng: 21.0122287 });
  const [place, setPlace] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const fetchLocation = async (place) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get(`https://localhost:7036/api/Booking/location?location=${place}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
        responseType: 'text',
      });
  
      const responseText = response.data;
      console.log('Raw response:', responseText);
  
      const latMatch = responseText.match(/Lat:\s*([\d.,-]+)/);
      const lngMatch = responseText.match(/Lng:\s*([\d.,-]+)/);
  
      if (latMatch && lngMatch) {
        const lat = parseFloat(latMatch[1].replace(',', '.'));
        const lng = parseFloat(lngMatch[1].replace(',', '.'));
  
        console.log('Parsed lat/lng:', lat, lng);
  
        if (!isNaN(lat) && !isNaN(lng)) {
          setLocation({ lat, lng });
          return;
        }
      }
  
      setError('Coordinates not found in the response.');
    } catch (error) {
      console.error(error);
      setError('Error fetching location.');
    } finally {
      setLoading(false);
    }
  };
  
  
  

  const searchLocation = () => {
    if (!place) return;
    fetchLocation(place);
  };

  return (
    <div>
      <h3>Search and view location on the map</h3>

      <input
        type="text"
        placeholder="Enter a place (e.g., Warsaw)"
        value={place}
        onChange={(e) => setPlace(e.target.value)}
      />
      <button onClick={searchLocation} disabled={loading}>
        {loading ? 'Loading...' : 'Search'}
      </button>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      <div style={{ width: '100%', maxWidth: '600px', height: '350px', margin: '20px auto', border: '1px solid #ccc', borderRadius: '10px', overflow: 'hidden', boxShadow: '0 2px 8px rgba(0,0,0,0.15)' }}>
        {/* Mapa */}
        <MapContainer center={[location.lat, location.lng]} zoom={13} style={{ width: '100%', height: '100%' }}>
          <TileLayer url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png" />
          <Marker position={[location.lat, location.lng]} icon={new L.Icon({ iconUrl: 'https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png', iconSize: [25, 41] })}>
            <Popup>
              Location: {location.lat}, {location.lng}
            </Popup>
          </Marker>
        </MapContainer>
      </div>
    </div>
  );
};

export default MapComponent;
