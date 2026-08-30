using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Commands.CreateFounder
{
    public class CreateFounderCommand : IRequest<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateFounderCommandHandler : IRequestHandler<CreateFounderCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateFounderCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateFounderCommand request, CancellationToken cancellationToken)
        {
            Founder Founder = _mapper.Map<Founder>(request);
            await _context.Founders.AddAsync(Founder, cancellationToken);
            await _context.SaveChangesAsync();

            return Founder.Id;
        }
    }
}
