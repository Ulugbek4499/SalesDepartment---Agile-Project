using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;

namespace SalesDepartment.Application.UseCases.Reports
{
    public record GetStatisticsQuery : IRequest<StatisticResponse>;

    public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticResponse>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetStatisticsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<StatisticResponse> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;

            var contracts = await _context.Contracts.ToArrayAsync();
            var payments = await _context.Payments.ToArrayAsync();
            var customers = await _context.Customers.ToArrayAsync();
            var homes = await _context.Homes.ToArrayAsync();

            var founderContractsCount = new Dictionary<string, int>();

            foreach (var contract in contracts)
            {
                var founderName = $"{contract.Founder.LastName} {contract.Founder.FirstName} {contract.Founder.MiddleName}";

                if (founderContractsCount.ContainsKey(founderName))
                {
                    founderContractsCount[founderName]++;
                }
                else
                {
                    founderContractsCount[founderName] = 1;
                }
            }

            var statistic = new StatisticResponse
            {
                CountOfAllCustomers = customers.Length,
                CountOfAllHomes = homes.Length,
                CountOfHomeWithoutContract = homes.Count(h => h.Contract == null),
                CountOfHomeWithContract =homes.Count(h => h.Contract!= null),

                AllContracts = contracts.Length,
                AllContractsInCurrentMonth = contracts.Count(c => c.ContractStartDate.Month == currentDate.Month),
                AllContractsInCurrentWeek = contracts.Count(c => (currentDate - c.ContractStartDate).TotalDays <= 7),
                AllContractsInCurrentDay = contracts.Count(c => c.ContractStartDate.Date == currentDate.Date),
                TotalAmountOfAllContracts = contracts.Sum(c => c.TotalAmountOfContract),
                TotalAmountOfAllContractsInCurrentMonth = contracts
                    .Where(c => c.ContractStartDate.Month == currentDate.Month)
                    .Sum(c => c.TotalAmountOfContract),
                TotalAmountOfAllContractsInCurrentWeek = contracts
                    .Where(c => (currentDate - c.ContractStartDate).TotalDays <= 7)
                    .Sum(c => c.TotalAmountOfContract),
                TotalAmountOfAllContractsInCurrentDay = contracts
                    .Where(c => c.ContractStartDate.Date == currentDate.Date)
                    .Sum(c => c.TotalAmountOfContract),
                AllContractsByFounders = founderContractsCount,

                CountOfAllPayments = payments.Length,
                CountOfPaymentsInCurrentMonth = payments.Count(p => p.PaymentDate.Month == currentDate.Month),
                CountOfPaymentsInLastSevenDays = payments.Count(p => (currentDate - p.PaymentDate).TotalDays <= 7),
                CountOfPaymentsInCurrentDay = payments.Count(p => p.PaymentDate.Date == currentDate.Date),
                
                TotalAmountOfAllPayments = payments.Sum(p => p.Amount),
                TotalAmountOfAlPaymentsInCurrentMonth = payments
                      .Where(p => p.PaymentDate.Month == currentDate.Month)
                      .Sum(p => p.Amount),
                TotalAmountOfAllPaymentsInLastSevenDays = payments
                      .Where(p => (currentDate - p.PaymentDate).TotalDays <= 7)
                      .Sum(p => p.Amount),
                TotalAmountOfAllPaymentsInCurrentDay = payments
                      .Where(p => p.PaymentDate.Date == currentDate.Date)
                      .Sum(p => p.Amount),
               
                AllContractsNumberInCurrentYear = contracts.Count(c => c.ContractStartDate.Year == currentYear),
                TotalAmountOfAllContractsInCurrentYear = contracts
                        .Where(c => c.ContractStartDate.Year == currentYear)
                        .Sum(c => c.TotalAmountOfContract),
                CountOfAllPaymentsInCurrentYear = payments.Count(p => p.PaymentDate.Year == currentYear),
                TotalAmountOfAllPaymentsInCurrentYear = payments
                        .Where(p => p.PaymentDate.Year == currentYear)
                          .Sum(p => p.Amount)
            };

            return statistic;
        }
    }
}
