using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Contracts.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Contracts.Queries.GetAllContractsPaginated
{
    public record GetContractsPaginationQuery : IRequest<PaginatedList<ContractResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetContractsPaginationQueryHandler : IRequestHandler<GetContractsPaginationQuery,
        PaginatedList<ContractResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetContractsPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<ContractResponse>> Handle(
            GetContractsPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var Contracts = _context.Contracts.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            Contracts = Contracts.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (Contracts is null || Contracts.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(Contract), search);
                        }*/

            var paginatedContracts = await PaginatedList<Contract>.CreateAsync(
                Contracts, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<ContractResponse>>(paginatedContracts.Items);

            var result = new PaginatedList<ContractResponse>
                (response, paginatedContracts.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
