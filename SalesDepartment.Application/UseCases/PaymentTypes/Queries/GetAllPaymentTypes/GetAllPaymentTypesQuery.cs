using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.PaymentTypes.Response;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetAllPaymentTypes
{
    public record GetAllPaymentTypesQuery : IRequest<PaymentTypeResponse[]>;

    public class GetAllPaymentTypesQueryHandler : IRequestHandler<GetAllPaymentTypesQuery, PaymentTypeResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllPaymentTypesQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaymentTypeResponse[]> Handle(GetAllPaymentTypesQuery request, CancellationToken cancellationToken)
        {
            var PaymentTypes = await _context.PaymentTypes.ToArrayAsync();

            return _mapper.Map<PaymentTypeResponse[]>(PaymentTypes);
        }
    }
}
