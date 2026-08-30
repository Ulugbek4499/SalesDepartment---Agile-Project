using AutoMapper;
using SalesDepartment.Application.UseCases.Customers.Commands.CreateCustomer;
using SalesDepartment.Application.UseCases.Customers.Commands.DeleteCustomer;
using SalesDepartment.Application.UseCases.Customers.Commands.UpdateCustomer;
using SalesDepartment.Application.UseCases.Customers.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Mappings
{
    public class CustomerMapping : Profile
    {
        public CustomerMapping()
        {
            CreateMap<CreateCustomerCommand, Customer>().ReverseMap();
            CreateMap<DeleteCustomerCommand, Customer>().ReverseMap();
            CreateMap<UpdateCustomerCommand, Customer>().ReverseMap();
            CreateMap<CustomerResponse, Customer>().ReverseMap();
        }
    }
}
