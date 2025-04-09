import React from 'react';
import axios from 'axios';

const PaymentForm = () => {
  const createPayment = () => {
    axios.get('https://localhost:7036/api/booking/create-payment')
      .then(response => {
        window.location.href = response.data.paymentLink; // Przekierowanie do strony płatności
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
