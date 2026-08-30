using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<int>
    {
        public string PaymentNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public int ContractId { get; set; }
        public int PaymentTypeId { get; set; }
    }

    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreatePaymentCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            Payment Payment = _mapper.Map<Payment>(request);
            await _context.Payments.AddAsync(Payment, cancellationToken);
            await _context.SaveChangesAsync();

            return Payment.Id;
        }
    }
}
