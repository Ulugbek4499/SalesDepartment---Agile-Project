using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Payments.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Payments.Queries.GetAllPaymentsPaginated
{
    public record GetPaymentsPaginationQuery : IRequest<PaginatedList<PaymentResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetPaymentsPaginationQueryHandler : IRequestHandler<GetPaymentsPaginationQuery,
        PaginatedList<PaymentResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetPaymentsPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<PaymentResponse>> Handle(
            GetPaymentsPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var Payments = _context.Payments.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            Payments = Payments.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (Payments is null || Payments.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(Payment), search);
                        }*/

            var paginatedPayments = await PaginatedList<Payment>.CreateAsync(
                Payments, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<PaymentResponse>>(paginatedPayments.Items);

            var result = new PaginatedList<PaymentResponse>
                (response, paginatedPayments.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
