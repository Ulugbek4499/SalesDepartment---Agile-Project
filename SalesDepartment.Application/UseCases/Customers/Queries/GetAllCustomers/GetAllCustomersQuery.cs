using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Customers.Response;

namespace SalesDepartment.Application.UseCases.Customers.Queries.GetAllCustomers
{
    public record GetAllCustomersQuery : IRequest<CustomerResponse[]>;

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, CustomerResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllCustomersQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CustomerResponse[]> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var Customers = await _context.Customers.ToArrayAsync();

            return _mapper.Map<CustomerResponse[]>(Customers);
        }
    }
}
