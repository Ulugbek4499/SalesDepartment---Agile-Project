using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Commands.CreateHome
{
    public class CreateHomeCommand : IRequest<int>
    {
        public string Block { get; set; }
        public int Entrance { get; set; }
        public int Floor { get; set; }
        public int ApartmentNumber { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Area { get; set; }
    }

    public class CreateHomeCommandHandler : IRequestHandler<CreateHomeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateHomeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateHomeCommand request, CancellationToken cancellationToken)
        {
            Home Home = _mapper.Map<Home>(request);
            await _context.Homes.AddAsync(Home, cancellationToken);
            await _context.SaveChangesAsync();

            return Home.Id;
        }
    }
}
