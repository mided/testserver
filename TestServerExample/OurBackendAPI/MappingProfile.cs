using AutoMapper;
using DatabaseLayer;
using OurBackendAPI.Models.In;
using OurBackendAPI.Models.Out;
using OurBackendAPI.Models.ThirdPartyServices;

namespace OurBackendAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerOutModel>();

            CreateMap<CustomerInModel, Customer>();

            CreateMap<InvoiceModel, InvoiceOutModel>();
        }
    }
}
