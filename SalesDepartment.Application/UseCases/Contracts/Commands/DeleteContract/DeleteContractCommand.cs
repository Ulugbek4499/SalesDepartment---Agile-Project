using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Contracts.Commands.DeleteContract
{
    public record DeleteContractCommand(int Id) : IRequest;
    public class DeleteContractCommandHandler : IRequestHandler<DeleteContractCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteContractCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteContractCommand request, CancellationToken cancellationToken)
        {
            Contract? contract = await _context.Contracts.FindAsync(request.Id, cancellationToken);

            if (contract is null)
                throw new NotFoundException(nameof(contract), request.Id);

            _context.Contracts.Remove(contract);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
