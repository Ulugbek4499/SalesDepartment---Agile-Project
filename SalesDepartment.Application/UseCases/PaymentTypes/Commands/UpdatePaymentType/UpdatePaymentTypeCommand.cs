using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType
{
    public class UpdatePaymentTypeCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdatePaymentTypeCommandHandler : IRequestHandler<UpdatePaymentTypeCommand>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public UpdatePaymentTypeCommandHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task Handle(UpdatePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType? PaymentType = await _context.PaymentTypes.FindAsync(request.Id);
            _mapper.Map(request, PaymentType);

            if (PaymentType is null)
                throw new NotFoundException(nameof(PaymentType), request.Id);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
