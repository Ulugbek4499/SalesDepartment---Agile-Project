using AutoMapper;
using SalesDepartment.Application.UseCases.Contracts.Commands.CreateContract;
using SalesDepartment.Application.UseCases.Contracts.Commands.DeleteContract;
using SalesDepartment.Application.UseCases.Contracts.Commands.UpdateContract;
using SalesDepartment.Application.UseCases.Contracts.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Mappings
{
    public class ContractMapping : Profile
    {
        public ContractMapping()
        {
            CreateMap<CreateContractCommand, Contract>().ReverseMap();
            CreateMap<DeleteContractCommand, Contract>().ReverseMap();
            CreateMap<UpdateContractCommand, Contract>().ReverseMap();
            CreateMap<ContractResponse, Contract>().ReverseMap();
        }
    }
}
