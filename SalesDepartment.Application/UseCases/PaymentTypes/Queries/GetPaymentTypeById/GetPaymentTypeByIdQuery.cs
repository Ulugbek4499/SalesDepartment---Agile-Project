using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.PaymentTypes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetPaymentTypeById
{
    public record GetPaymentTypeByIdQuery(int Id) : IRequest<PaymentTypeResponse>;

    public class GetPaymentTypeByIdQueryHandler : IRequestHandler<GetPaymentTypeByIdQuery, PaymentTypeResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetPaymentTypeByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PaymentTypeResponse> Handle(GetPaymentTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var PaymentType = FilterIfPaymentTypeExsists(request.Id);

            var result = _mapper.Map<PaymentTypeResponse>(PaymentType);
            return await Task.FromResult(result);
        }

        private PaymentType FilterIfPaymentTypeExsists(int id)
            => _dbContext.PaymentTypes
                .Find(id)
                     ?? throw new NotFoundException(
                            " There is no PaymentType with this Id. ");
    }
}
