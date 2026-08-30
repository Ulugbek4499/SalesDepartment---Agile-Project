using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Commands.UpdateFounder
{
    public class UpdateFounderCommand : IRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class UpdateFounderCommandHandler : IRequestHandler<UpdateFounderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateFounderCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdateFounderCommand request, CancellationToken cancellationToken)
        {
            Founder? Founder = await _context.Founders.FindAsync(request.Id);
            _mapper.Map(request, Founder);

            if (Founder is null)
                throw new NotFoundException(nameof(Founder), request.Id);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
