using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Commands.UpdateHome
{
    public class UpdateHomeCommand : IRequest
    {
        public int Id { get; set; }
        public string Block { get; set; }
        public int Entrance { get; set; }
        public int Floor { get; set; }
        public int ApartmentNumber { get; set; }
        public int NumberOfRooms { get; set; }
        public decimal Area { get; set; }
    }

    public class UpdateHomeCommandHandler : IRequestHandler<UpdateHomeCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateHomeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdateHomeCommand request, CancellationToken cancellationToken)
        {
            Home? Home = await _context.Homes.FindAsync(request.Id);
            _mapper.Map(request, Home);

            if (Home is null)
                throw new NotFoundException(nameof(Home), request.Id);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

