using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Customers.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(int Id) : IRequest<CustomerResponse>;

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetCustomerByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var Customer = FilterIfCustomerExsists(request.Id);

            var result = _mapper.Map<CustomerResponse>(Customer);
            return await Task.FromResult(result);
        }

        private Customer FilterIfCustomerExsists(int id)
            => _dbContext.Customers
                .Find(id)
                     ?? throw new NotFoundException(
                            " There is no Customer with this Id. ");
    }
}
