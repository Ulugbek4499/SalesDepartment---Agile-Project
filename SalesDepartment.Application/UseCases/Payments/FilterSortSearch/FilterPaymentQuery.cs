

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Payments.Response;

namespace SalesDepartment.Application.UseCases.Payments.FilterSortSearch
{
    public class FilterPaymentQuery : IRequest<PaymentResponse[]>
    {
        public string? PaymentNumber { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public int? ContractId { get; set; }
        public int? PaymentTypeId { get; set; }
    }

    public class FilterPaymentQueryHandler : IRequestHandler<FilterPaymentQuery, PaymentResponse[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterPaymentQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentResponse[]> Handle(FilterPaymentQuery request, CancellationToken cancellationToken)
        {
            var payments = _context.Payments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.PaymentNumber))
            {
                payments = payments.Where(p => p.PaymentNumber == request.PaymentNumber);
            }

            if (request.PaymentDate.HasValue)
            {
                payments = payments.Where(p => p.PaymentDate == request.PaymentDate);
            }

            if (request.Amount.HasValue)
            {
                payments = payments.Where(p => p.Amount == request.Amount);
            }

            if (request.ContractId.HasValue)
            {
                payments = payments.Where(p => p.ContractId == request.ContractId);
            }

            if (request.PaymentTypeId.HasValue)
            {
                payments = payments.Where(p => p.PaymentTypeId == request.PaymentTypeId);
            }

            var paymentResponses = await payments
                .Select(p => _mapper.Map<PaymentResponse>(p))
                .ToArrayAsync(cancellationToken);

            return paymentResponses;
        }
    }

}
