using System;

namespace OurBackendAPI.Models.Out
{
    public class InvoiceOutModel
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Issued { get; set; }
    }
}
