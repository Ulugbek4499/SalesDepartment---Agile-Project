using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Commands.CreatePaymentType
{
    public class CreatePaymentTypeCommand : IRequest<int>
    {
        public string Name { get; set; }
    }

    public class CreatePaymentTypeCommandHandler : IRequestHandler<CreatePaymentTypeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public CreatePaymentTypeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> Handle(CreatePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType PaymentType = _mapper.Map<PaymentType>(request);
            await _context.PaymentTypes.AddAsync(PaymentType, cancellationToken);
            await _context.SaveChangesAsync();

            return PaymentType.Id;
        }
    }
}
