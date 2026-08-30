using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Commands.DeleteFounder
{
    public record DeleteFounderCommand(int Id) : IRequest;
    public class DeleteFounderCommandHandler : IRequestHandler<DeleteFounderCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteFounderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteFounderCommand request, CancellationToken cancellationToken)
        {
            Founder? Founder = await _context.Founders.FindAsync(request.Id, cancellationToken);

            if (Founder is null)
                throw new NotFoundException(nameof(Founder), request.Id);

            _context.Founders.Remove(Founder);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
