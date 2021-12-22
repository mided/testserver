﻿using System.Collections.Generic;

namespace OurBackendAPI.Models.Out
{
    public class CustomerOutModel
    {
        public int CustomerId { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string EmailAddress { get; set; }

        public string Phone { get; set; }

        public IEnumerable<InvoiceOutModel> Invoices { get; set; }
    }
}
