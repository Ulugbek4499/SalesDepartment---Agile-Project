using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Founders.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Queries.GetAllFoundersPaginated
{
    public record GetFoundersPaginationQuery : IRequest<PaginatedList<FounderResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetFoundersPaginationQueryHandler : IRequestHandler<GetFoundersPaginationQuery,
        PaginatedList<FounderResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetFoundersPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<FounderResponse>> Handle(
            GetFoundersPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var Founders = _context.Founders.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            Founders = Founders.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (Founders is null || Founders.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(Founder), search);
                        }*/

            var paginatedFounders = await PaginatedList<Founder>.CreateAsync(
                Founders, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<FounderResponse>>(paginatedFounders.Items);

            var result = new PaginatedList<FounderResponse>
                (response, paginatedFounders.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
