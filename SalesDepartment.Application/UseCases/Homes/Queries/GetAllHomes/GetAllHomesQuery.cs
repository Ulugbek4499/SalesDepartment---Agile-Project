using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Homes.Response;

namespace SalesDepartment.Application.UseCases.Homes.Queries.GetAllHomes
{
    public record GetAllHomesQuery : IRequest<HomeResponse[]>;

    public class GetAllHomesQueryHandler : IRequestHandler<GetAllHomesQuery, HomeResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllHomesQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<HomeResponse[]> Handle(GetAllHomesQuery request, CancellationToken cancellationToken)
        {
            var Homes = await _context.Homes.ToArrayAsync();

            return _mapper.Map<HomeResponse[]>(Homes);
        }
    }
}
