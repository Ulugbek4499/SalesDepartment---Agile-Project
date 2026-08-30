using AutoMapper;
using SalesDepartment.Application.UseCases.Homes.Commands.CreateHome;
using SalesDepartment.Application.UseCases.Homes.Commands.DeleteHome;
using SalesDepartment.Application.UseCases.Homes.Commands.UpdateHome;
using SalesDepartment.Application.UseCases.Homes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Mappings
{
    public class HomeMapping : Profile
    {
        public HomeMapping()
        {
            CreateMap<CreateHomeCommand, Home>().ReverseMap();
            CreateMap<DeleteHomeCommand, Home>().ReverseMap();
            CreateMap<UpdateHomeCommand, Home>().ReverseMap();
            CreateMap<HomeResponse, Home>().ReverseMap();
        }
    }
}
