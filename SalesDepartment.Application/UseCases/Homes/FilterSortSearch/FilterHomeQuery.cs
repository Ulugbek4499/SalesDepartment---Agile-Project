using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Homes.Response;

namespace SalesDepartment.Application.UseCases.Homes.FilterSortSearch
{
    public class FilterHomeQuery : IRequest<HomeResponse[]>
    {
        public string? Block { get; set; }
        public int? Entrance { get; set; }
        public int? Floor { get; set; }
        public int? ApartmentNumber { get; set; }
        public int? NumberOfRooms { get; set; }
        public decimal? Area { get; set; }
    }

    public class FilterHomeQueryHandler : IRequestHandler<FilterHomeQuery, HomeResponse[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterHomeQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HomeResponse[]> Handle(FilterHomeQuery request, CancellationToken cancellationToken)
        {
            var homes = _context.Homes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Block))
            {
                homes = homes.Where(h => h.Block == request.Block);
            }

            if (request.Entrance.HasValue)
            {
                homes = homes.Where(h => h.Entrance == request.Entrance);
            }

            if (request.Floor.HasValue)
            {
                homes = homes.Where(h => h.Floor == request.Floor);
            }

            if (request.ApartmentNumber.HasValue)
            {
                homes = homes.Where(h => h.ApartmentNumber == request.ApartmentNumber);
            }

            if (request.NumberOfRooms.HasValue)
            {
                homes = homes.Where(h => h.NumberOfRooms == request.NumberOfRooms);
            }

            if (request.Area.HasValue)
            {
                homes = homes.Where(h => h.Area == request.Area);
            }

            var homeResponses = await homes
                .Select(h => _mapper.Map<HomeResponse>(h))
                .ToArrayAsync(cancellationToken);

            return homeResponses;
        }
    }

}
