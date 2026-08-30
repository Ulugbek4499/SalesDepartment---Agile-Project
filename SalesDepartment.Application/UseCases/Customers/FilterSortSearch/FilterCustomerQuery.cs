using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Customers.Response;

namespace SalesDepartment.Application.UseCases.Customers.FilterSortSearch
{
    public class FilterCustomerQuery : IRequest<CustomerResponse[]>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Passport { get; set; }
        public string? PassportIssuedBy { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumberOne { get; set; }
        public string? PhoneNumberTwo { get; set; }
    }

    public class FilterCustomerQueryHandler : IRequestHandler<FilterCustomerQuery, CustomerResponse[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterCustomerQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerResponse[]> Handle(FilterCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                customers = customers.Where(c => c.FirstName == request.FirstName);
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                customers = customers.Where(c => c.LastName == request.LastName);
            }

            if (!string.IsNullOrWhiteSpace(request.MiddleName))
            {
                customers = customers.Where(c => c.MiddleName == request.MiddleName);
            }

            if (!string.IsNullOrWhiteSpace(request.Passport))
            {
                customers = customers.Where(c => c.Passport == request.Passport);
            }

            if (!string.IsNullOrWhiteSpace(request.PassportIssuedBy))
            {
                customers = customers.Where(c => c.PassportIssuedBy == request.PassportIssuedBy);
            }

            if (!string.IsNullOrWhiteSpace(request.Address))
            {
                customers = customers.Where(c => c.Address == request.Address);
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumberOne))
            {
                customers = customers.Where(c => c.PhoneNumberOne == request.PhoneNumberOne);
            }

            if (!string.IsNullOrWhiteSpace(request.PhoneNumberTwo))
            {
                customers = customers.Where(c => c.PhoneNumberTwo == request.PhoneNumberTwo);
            }

            var customerResponses = await customers
                .Select(c => _mapper.Map<CustomerResponse>(c))
                .ToArrayAsync(cancellationToken);

            return customerResponses;
        }
    }

}
