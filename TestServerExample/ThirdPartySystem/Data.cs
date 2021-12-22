using System;
using System.Collections.Generic;
using System.Linq;

namespace ThirdPartySystem
{
    public static class Data
    {
        public static List<Invoice> Invoices { get; set; }

        static Data()
        {
            const int clientCount = 1000;
            const int maxInvoicePerClient = 20;
            var rnd = new Random();

            Invoices = Enumerable.Range(0, clientCount)
                .Select(clientId => Enumerable.Range(0, rnd.Next(maxInvoicePerClient))
                    .Select(_ => GenerateRandomInvoice(clientId))
                )
                .SelectMany(x => x).ToList();
        }

        private static Invoice GenerateRandomInvoice(int clientId)
        {
            var rng = new Random();
            var invoice = new Invoice
            {
                Amount = rng.Next(1, 1000),
                Currency = (new[] { "USD", "EUR", "GBR" })[rng.Next(3)],
                Issued = DateTime.Now.AddDays(-rng.Next(30)),
                ClientId = clientId
            };

            return invoice;
        }
    }
}
