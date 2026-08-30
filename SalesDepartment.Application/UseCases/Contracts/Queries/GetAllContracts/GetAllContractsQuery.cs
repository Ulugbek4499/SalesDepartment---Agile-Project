using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Contracts.Queries.GetAllContracts
{
    public record GetAllContractsQuery : IRequest<ContractResponse[]>;

    public class GetAllContractsQueryHandler : IRequestHandler<GetAllContractsQuery, ContractResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllContractsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ContractResponse[]> Handle(GetAllContractsQuery request, CancellationToken cancellationToken)
        {
            var contracts = await _context.Contracts.ToArrayAsync();

            return _mapper.Map<ContractResponse[]>(contracts);
        }
    }
}
