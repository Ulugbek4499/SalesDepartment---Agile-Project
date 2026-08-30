using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Payments.Commands.DeletePayment
{
    public record DeletePaymentCommand(int Id) : IRequest;
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePaymentCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            Payment? Payment = await _context.Payments.FindAsync(request.Id, cancellationToken);

            if (Payment is null)
                throw new NotFoundException(nameof(Payment), request.Id);

            _context.Payments.Remove(Payment);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
