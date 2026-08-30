using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Homes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Queries.GetAllHomesPaginated
{
    public record GetHomesPaginationQuery : IRequest<PaginatedList<HomeResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetHomesPaginationQueryHandler : IRequestHandler<GetHomesPaginationQuery,
        PaginatedList<HomeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetHomesPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<HomeResponse>> Handle(
            GetHomesPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var Homes = _context.Homes.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            Homes = Homes.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (Homes is null || Homes.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(Home), search);
                        }*/

            var paginatedHomes = await PaginatedList<Home>.CreateAsync(
                Homes, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<HomeResponse>>(paginatedHomes.Items);

            var result = new PaginatedList<HomeResponse>
                (response, paginatedHomes.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
