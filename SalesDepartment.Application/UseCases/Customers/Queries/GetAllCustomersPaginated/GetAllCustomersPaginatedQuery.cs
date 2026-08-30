using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Customers.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Customers.Queries.GetAllCustomersPaginated
{
    public record GetCustomersPaginationQuery : IRequest<PaginatedList<CustomerResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetCustomersPaginationQueryHandler : IRequestHandler<GetCustomersPaginationQuery,
        PaginatedList<CustomerResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetCustomersPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<CustomerResponse>> Handle(
            GetCustomersPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var Customers = _context.Customers.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            Customers = Customers.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (Customers is null || Customers.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(Customer), search);
                        }*/

            var paginatedCustomers = await PaginatedList<Customer>.CreateAsync(
                Customers, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<CustomerResponse>>(paginatedCustomers.Items);

            var result = new PaginatedList<CustomerResponse>
                (response, paginatedCustomers.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
