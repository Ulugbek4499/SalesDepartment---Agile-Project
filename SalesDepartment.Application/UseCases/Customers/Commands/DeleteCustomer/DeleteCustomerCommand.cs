using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Customers.Commands.DeleteCustomer
{
    public record DeleteCustomerCommand(int Id) : IRequest;
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer? customer = await _context.Customers.FindAsync(request.Id, cancellationToken);

            if (customer is null)
                throw new NotFoundException(nameof(customer), request.Id);

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
