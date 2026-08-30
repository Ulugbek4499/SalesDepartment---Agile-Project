using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Reports
{
    public record GetContractsWithDepts(DateTime Time) : IRequest<ContractResponse[]>;

    public class GetAllContractsQueryHandler : IRequestHandler<GetContractsWithDepts, ContractResponse[]>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetAllContractsQueryHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        /*        public async Task<ContractResponse[]> Handle(GetContractsWithDepts request, CancellationToken cancellationToken)
                {
                    var contracts = await _context.Contracts.ToArrayAsync();

                    var contractResponses = new List<ContractResponse>();

                    foreach (var entity in contracts)
                    {
                        var contract = _context.Contracts.Find(entity.Id);

                        Dictionary<DateTime, decimal> actualPaymentSchedule = contract.Payments
                            .ToDictionary(payment => payment.PaymentDate, payment => payment.Amount);

                        Dictionary<string, decimal> paymentSchedule = new Dictionary<string, decimal>();

                        DateTime paymentDate = contract.PaymentStartDate;
                        int paymentIndex = 1;

                        for (int i = 1; i <= contract.NumberOfMonths; i++)
                        {
                            decimal paymentAmount = contract.InAdvancePaymentOfContract + (i * contract.AmountOfMonthlyPayment);

                            // Generate a unique key based on the paymentIndex
                            string paymentKey = $"Payment-{paymentIndex}";

                            paymentSchedule.Add(paymentKey, paymentAmount);

                            paymentIndex++;
                        }

                        DateTime calculationDate = DateTime.Now;
                        var sumOfPayments = contract.Payments.Sum(x => x.Amount);

                        decimal sumForCurrentMonth = 0;

                        foreach (var entry in paymentSchedule)
                        {
                            if (entry.Key.StartsWith("Payment-") && int.TryParse(entry.Key.Substring("Payment-".Length), out int index))
                            {
                                if (index <= calculationDate.Month)
                                {
                                    sumForCurrentMonth = entry.Value;
                                }
                            }
                        }

                        decimal deptAmount = sumForCurrentMonth - sumOfPayments;

                        ContractResponse contractResponse = new ContractResponse()
                        {
                            Id = entity.Id,
                            ContractNumber = contract.ContractNumber,
                            ContractStartDate = contract.ContractStartDate,
                            PaymentStartDate = contract.PaymentStartDate,
                            TotalAmountOfContract = contract.TotalAmountOfContract,
                            InAdvancePaymentOfContract = contract.InAdvancePaymentOfContract,
                            NumberOfMonths = contract.NumberOfMonths,
                            AmountOfMonthlyPayment = contract.AmountOfMonthlyPayment,
                            HomeId = contract.HomeId,
                            Home = contract.Home,
                            CustomerId = contract.CustomerId,
                            Customer = contract.Customer,
                            FounderId = contract.FounderId,
                            Founder = contract.Founder,
                            CreatedDate = contract.CreatedDate,
                            ModifyDate = contract.ModifyDate,
                            Payments = contract.Payments,
                            DeptAmout = deptAmount,
                            ScheduledInfo2 = paymentSchedule,
                            ActualInfo = actualPaymentSchedule
                        };

                        contractResponses.Add(contractResponse);
                    }

                    return contractResponses.ToArray();
                }
        */


        public async Task<ContractResponse[]> Handle(GetContractsWithDepts request, CancellationToken cancellationToken)
        {
            var contracts = await _context.Contracts.ToArrayAsync();

            var contractResponses = new List<ContractResponse>();

            foreach (var entity in contracts)
            {
                var contract = _context.Contracts.Find(entity.Id);

                Dictionary<DateTime, decimal> actualPaymentSchedule = contract.Payments
                             .ToDictionary(payment => payment.PaymentDate, payment => payment.Amount);


                Dictionary<DateTime, decimal> paymentSchedule = new Dictionary<DateTime, decimal>();

                DateTime paymentDate = contract.PaymentStartDate;

                for (int i = 1; i <= contract.NumberOfMonths; i++)
                {
                    decimal paymentAmount = contract.InAdvancePaymentOfContract +
                        (i * contract.AmountOfMonthlyPayment);

                    paymentSchedule.Add(paymentDate, paymentAmount);

                    paymentDate = paymentDate.AddMonths(1);
                }

                DateTime calculationDate = DateTime.Now;
                var sumOfPayments = contract.Payments.Sum(x => x.Amount);

                decimal sumForCurrentMonth = 0;

                foreach (var entry in paymentSchedule)
                {
                    if (entry.Key.Month <= calculationDate.Month
                        && entry.Key.Year <= calculationDate.Year)
                    {
                        sumForCurrentMonth = entry.Value;
                    }
                }

                decimal deptAmount = 0;

                deptAmount = sumForCurrentMonth - sumOfPayments;


                ContractResponse contractResponse = new ContractResponse()
                {
                    Id = entity.Id,
                    ContractNumber = contract.ContractNumber,
                    ContractStartDate = contract.ContractStartDate,
                    PaymentStartDate = contract.PaymentStartDate,
                    TotalAmountOfContract = contract.TotalAmountOfContract,
                    InAdvancePaymentOfContract = contract.InAdvancePaymentOfContract,
                    NumberOfMonths = contract.NumberOfMonths,
                    AmountOfMonthlyPayment = contract.AmountOfMonthlyPayment,
                    HomeId = contract.HomeId,
                    Home = contract.Home,
                    CustomerId = contract.CustomerId,
                    Customer = contract.Customer,
                    FounderId = contract.FounderId,
                    Founder = contract.Founder,
                    CreatedDate = contract.CreatedDate,
                    ModifyDate = contract.ModifyDate,
                    Payments = contract.Payments,
                    DeptAmout = deptAmount,
                    ScheduledInfo = paymentSchedule,
                    ActualInfo = actualPaymentSchedule
                };

                contractResponses.Add(contractResponse);
            }

            return contractResponses.ToArray();
        }
    }
}
