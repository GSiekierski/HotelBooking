public class CreatePaymentRequest
    {
        public string OrderId { get; set; }
        public int TotalAmount { get; set; }
        public string CustomerIp { get; set; }
        public string Description { get; set; }
    }
