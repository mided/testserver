using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ThirdPartySystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InvoiceController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Invoice> Get([FromQuery] int clientId)
        {
            return Data.Invoices.Where(i => i.ClientId == clientId).ToList();
        }
    }
}
