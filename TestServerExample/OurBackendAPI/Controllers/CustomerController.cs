using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using CommonInterfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OurBackendAPI.Models.In;
using OurBackendAPI.Models.Out;
using OurBackendAPI.Models.ThirdPartyServices;

namespace OurBackendAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly DblContext _dblContext;

        private readonly IMapper _mapper;

        private readonly IHttpWrapper _httpWrapper;

        public CustomerController(
            DblContext dblContext,
            IMapper mapper,
            IHttpWrapper httpWrapper)
        {
            _dblContext = dblContext;
            _mapper = mapper;
            _httpWrapper = httpWrapper;
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<CustomerOutModel>))]
        public async Task<IActionResult> Get()
        {
            var data = await _dblContext.Customers.ToListAsync();

            var result = _mapper.Map<IEnumerable<CustomerOutModel>>(data);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces(typeof(CustomerOutModel))]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await _dblContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (data == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<CustomerOutModel>(data);

            var (statusCode, response) = await _httpWrapper.Send(HttpMethod.Get, $"invoice?clientId={id}");

            if (statusCode == HttpStatusCode.OK)
            {
                var invoices = JsonConvert.DeserializeObject<IEnumerable<InvoiceModel>>(response);
                result.Invoices = _mapper.Map<IEnumerable<InvoiceOutModel>>(invoices);
            }

            return Ok(result);
        }

        [HttpPost]
        [Produces(typeof(CustomerOutModel))]
        public async Task<IActionResult> Create([BindRequired] CustomerInModel input)
        {
            var entity = _mapper.Map<Customer>(input);

            var data = await _dblContext.Customers.AddAsync(entity);
            await _dblContext.SaveChangesAsync();

            var result = _mapper.Map<CustomerOutModel>(data.Entity);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] int id)
        {
            var data = await _dblContext.Customers.FirstOrDefaultAsync(c => c.CustomerId == id);

            if (data == null)
            {
                return NotFound();
            }

            _dblContext.Remove(data);
            await _dblContext.SaveChangesAsync();
            
            return Ok();
        }
    }
}
