using System;

namespace OurBackendAPI.Models.ThirdPartyServices
{
    public class InvoiceModel
    {
        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public DateTime Issued { get; set; }
    }
}
