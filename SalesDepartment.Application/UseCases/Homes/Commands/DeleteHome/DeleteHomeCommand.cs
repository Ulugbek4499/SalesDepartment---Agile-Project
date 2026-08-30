using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Commands.DeleteHome
{
    public record DeleteHomeCommand(int Id) : IRequest;
    public class DeleteHomeCommandHandler : IRequestHandler<DeleteHomeCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteHomeCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteHomeCommand request, CancellationToken cancellationToken)
        {
            Home? Home = await _context.Homes.FindAsync(request.Id, cancellationToken);

            if (Home is null)
                throw new NotFoundException(nameof(Home), request.Id);

            _context.Homes.Remove(Home);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
