using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Founders.Response;

namespace SalesDepartment.Application.UseCases.Founders.Queries.GetAllFounders
{
    public record GetAllFoundersQuery : IRequest<FounderResponse[]>;

    public class GetAllFoundersQueryHandler : IRequestHandler<GetAllFoundersQuery, FounderResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllFoundersQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<FounderResponse[]> Handle(GetAllFoundersQuery request, CancellationToken cancellationToken)
        {
            var Founders = await _context.Founders.ToArrayAsync();

            return _mapper.Map<FounderResponse[]>(Founders);
        }
    }
}