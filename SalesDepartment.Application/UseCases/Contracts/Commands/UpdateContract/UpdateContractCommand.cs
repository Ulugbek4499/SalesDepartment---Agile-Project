using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommand : IRequest
    {
        public int Id { get; set; }
        public string ContractNumber { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime PaymentStartDate { get; set; }

        public decimal TotalAmountOfContract { get; set; }
        public decimal InAdvancePaymentOfContract { get; set; }
        public int NumberOfMonths { get; set; }

        public int HomeId { get; set; }
        public int CustomerId { get; set; }
        public int FounderId { get; set; }
    }

    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdateContractCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            Contract? contract = await _context.Contracts.FindAsync(request.Id);
            _mapper.Map(request, contract);

            if (contract is null)
                throw new NotFoundException(nameof(contract), request.Id);

            var Home = await _context.Homes.FindAsync(request.HomeId);

            if (Home is null)
                throw new NotFoundException(nameof(Home), request.HomeId);

            var Customer = await _context.Customers.FindAsync(request.CustomerId);

            if (Customer is null)
                throw new NotFoundException(nameof(Customer), request.CustomerId);

            var Founder = await _context.Founders.FindAsync(request.FounderId);

            if (Founder is null)
                throw new NotFoundException(nameof(Founder), request.FounderId);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
