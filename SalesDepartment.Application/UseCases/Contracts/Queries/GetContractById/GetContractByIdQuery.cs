using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Contracts.Queries.GetContractById
{
    public record GetContractByIdQuery(int Id) : IRequest<ContractResponse>;

    public class GetContractByIdQueryHandler : IRequestHandler<GetContractByIdQuery, ContractResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetContractByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ContractResponse> Handle(GetContractByIdQuery request, CancellationToken cancellationToken)
        {
            var contract = _dbContext.Contracts.Find(request.Id);

            Dictionary<DateTime, decimal> actualPaymentSchedule = contract.Payments
                         .ToDictionary(payment => payment.PaymentDate, payment => payment.Amount);


            Dictionary<DateTime, decimal> paymentSchedule = new Dictionary<DateTime, decimal>();

            DateTime paymentDate = contract.PaymentStartDate;

            for (int i = 1; i <=contract.NumberOfMonths; i++)
            {
                decimal paymentAmount = contract.InAdvancePaymentOfContract + 
                    (i * contract.AmountOfMonthlyPayment);

                paymentSchedule.Add(paymentDate, paymentAmount);

                paymentDate = paymentDate.AddMonths(1);
            }

            DateTime calculationDate = DateTime.Now;
            var sumOfPayments = contract.Payments.Sum(x => x.Amount);
            var scheduledPayment = paymentSchedule.FirstOrDefault(x => x.Key.Month == calculationDate.Month);
            decimal deptAmount = 0;

            if (scheduledPayment.Key != default(DateTime))
            {
                decimal scheduledPaymentAmount = scheduledPayment.Value;

                deptAmount = scheduledPaymentAmount - sumOfPayments;
            }


            ContractResponse contractResponse = new ContractResponse()
            {
                Id = request.Id,
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

            return contractResponse;
        }
    }
}
