import React from 'react';
import axios from 'axios';

const PaymentForm = () => {
  const createPayment = () => {
    axios.get('https://localhost:7036/api/booking/create-payment', { responseType: 'text' })
    .then(response => {
      if (!response.data) {
        console.error('No payment HTML returned.');
        return;
      }
      
      window.location.href = response.data;
    })
    .catch(error => {
      console.error('Error creating payment:', error);
    });
  };
  

  return (
    <div className="container">
      <h2>Create Payment</h2>
      <button onClick={createPayment} className="btn btn-success">
        Create Payment
      </button>
    </div>
  );
};

export default PaymentForm;
