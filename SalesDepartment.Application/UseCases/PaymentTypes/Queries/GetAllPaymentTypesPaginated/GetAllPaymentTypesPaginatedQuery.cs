using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.PaymentTypes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetAllPaymentTypesPaginated
{
    public record GetPaymentTypesPaginationQuery : IRequest<PaginatedList<PaymentTypeResponse>>
    {
        public string? SearchTerm { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
    public class GetPaymentTypesPaginationQueryHandler : IRequestHandler<GetPaymentTypesPaginationQuery,
        PaginatedList<PaymentTypeResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetPaymentTypesPaginationQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<PaymentTypeResponse>> Handle(
            GetPaymentTypesPaginationQuery request, CancellationToken cancellationToken)
        {
            var search = request.SearchTerm?.Trim();
            var PaymentTypes = _context.PaymentTypes.AsQueryable();
            /*
                        if (!string.IsNullOrEmpty(search))
                        {
                            PaymentTypes = PaymentTypes.Where(s => s.Name.ToLower().Contains(search.ToLower())
                                                        || s.Description.ToLower().Contains(search.ToLower()));
                        }
                        if (PaymentTypes is null || PaymentTypes.Count() <= 0)
                        {
                            throw new NotFoundException(nameof(PaymentType), search);
                        }*/

            var paginatedPaymentTypes = await PaginatedList<PaymentType>.CreateAsync(
                PaymentTypes, request.PageNumber, request.PageSize);

            var response = _mapper.Map<List<PaymentTypeResponse>>(paginatedPaymentTypes.Items);

            var result = new PaginatedList<PaymentTypeResponse>
                (response, paginatedPaymentTypes.TotalCount, request.PageNumber, request.PageSize);

            return result;
        }
    }
}
