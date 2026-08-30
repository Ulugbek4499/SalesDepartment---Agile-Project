using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Commands.DeletePaymentType
{
    public record DeletePaymentTypeCommand(int Id) : IRequest;
    public class DeletePaymentTypeCommandHandler : IRequestHandler<DeletePaymentTypeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePaymentTypeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePaymentTypeCommand request, CancellationToken cancellationToken)
        {
            PaymentType? PaymentType = await _context.PaymentTypes.FindAsync(request.Id, cancellationToken);

            if (PaymentType is null)
                throw new NotFoundException(nameof(PaymentType), request.Id);

            _context.PaymentTypes.Remove(PaymentType);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
