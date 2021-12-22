using System;

namespace ThirdPartySystem
{
    public class Invoice
    {
        public int ClientId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Issued { get; set; }
    }
}
