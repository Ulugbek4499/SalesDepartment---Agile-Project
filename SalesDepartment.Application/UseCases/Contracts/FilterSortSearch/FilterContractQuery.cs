using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Contracts.FilterSortSearch
{
    public class FilterContractQuery : IRequest<ContractResponse[]>
    {
        public string? ContractNumber { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? PaymentStartDate { get; set; }

        public decimal? TotalAmountOfContract { get; set; }
        public decimal? InAdvancePaymentOfContract { get; set; }
        public int? NumberOfMonths { get; set; }

        public int? HomeId { get; set; }
        public int? CustomerId { get; set; }
        public int? FounderId { get; set; }
    }

    public class FilterContractQueryHandler : IRequestHandler<FilterContractQuery, ContractResponse[]>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FilterContractQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContractResponse[]> Handle(FilterContractQuery request, CancellationToken cancellationToken)
        {
            var contracts = _context.Contracts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.ContractNumber))
            {
                contracts = contracts.Where(c => c.ContractNumber == request.ContractNumber);
            }

            if (request.ContractStartDate.HasValue)
            {
                contracts = contracts.Where(c => c.ContractStartDate == request.ContractStartDate);
            }

            if (request.PaymentStartDate.HasValue)
            {
                contracts = contracts.Where(c => c.PaymentStartDate == request.PaymentStartDate);
            }

            if (request.TotalAmountOfContract.HasValue)
            {
                contracts = contracts.Where(c => c.TotalAmountOfContract == request.TotalAmountOfContract);
            }

            if (request.NumberOfMonths.HasValue)
            {
                contracts = contracts.Where(c => c.NumberOfMonths == request.NumberOfMonths);
            }

            if (request.PaymentStartDate.HasValue)
            {
                contracts = contracts.Where(c => c.PaymentStartDate == request.PaymentStartDate);
            }

            if (request.HomeId.HasValue)
            {
                contracts = contracts.Where(c => c.HomeId == request.HomeId);
            }

            if (request.CustomerId.HasValue)
            {
                contracts = contracts.Where(c => c.CustomerId == request.CustomerId);
            }

            if (request.FounderId.HasValue)
            {
                contracts = contracts.Where(c => c.FounderId == request.FounderId);
            }

            var contractResponses = await contracts
                .Select(c => _mapper.Map<ContractResponse>(c))
                .ToArrayAsync(cancellationToken);

            return contractResponses;
        }
    }

}
