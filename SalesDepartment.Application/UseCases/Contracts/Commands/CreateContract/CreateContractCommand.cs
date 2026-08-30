using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Contracts.Commands.CreateContract
{
    public class CreateContractCommand : IRequest<int>
    {
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

    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreateContractCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            Contract contract = _mapper.Map<Contract>(request);
            await _context.Contracts.AddAsync(contract, cancellationToken);
            await _context.SaveChangesAsync();

            return contract.Id;
        }
    }
}
