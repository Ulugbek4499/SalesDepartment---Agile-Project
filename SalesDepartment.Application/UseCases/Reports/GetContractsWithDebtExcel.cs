using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Reports
{
    public class GetContractsWithDebtExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetContractsWithDebtExcelHandler : IRequestHandler<GetContractsWithDebtExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractsWithDebtExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetContractsWithDebtExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook workbook = new())
            {
                var orderData = await GetContractsAsync(cancellationToken);
                var excelSheet = workbook.AddWorksheet(orderData, "Contracts");

                excelSheet.RowHeight = 20;
                excelSheet.Column(1).Width = 18;
                excelSheet.Column(2).Width = 18;
                excelSheet.Column(3).Width = 18;
                excelSheet.Column(4).Width = 18;
                excelSheet.Column(5).Width = 18;
                excelSheet.Column(6).Width = 18;
                excelSheet.Column(7).Width = 18;
                excelSheet.Column(8).Width = 18;
                excelSheet.Column(9).Width = 18;
                excelSheet.Column(10).Width = 18;
                excelSheet.Column(11).Width = 18;
                excelSheet.Column(12).Width = 18;
                excelSheet.Column(13).Width = 18;
                excelSheet.Column(14).Width = 18;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "Contract/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetContractsAsync(CancellationToken cancellationToken = default)
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

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Контракт №", typeof(string));
            excelDataTable.Columns.Add("Клиент", typeof(string));
            excelDataTable.Columns.Add("Клиент Номер тел 1", typeof(string));
            excelDataTable.Columns.Add("Клиент Номер тел 2", typeof(string));
            excelDataTable.Columns.Add("Сумма контракта", typeof(decimal));
            excelDataTable.Columns.Add("Предоплата", typeof(decimal));
            excelDataTable.Columns.Add("Ежемесячно оплата", typeof(decimal));
            excelDataTable.Columns.Add("Запланированный платеж", typeof(decimal));
            excelDataTable.Columns.Add("Выплаченная сумма", typeof(decimal));
            excelDataTable.Columns.Add("Долг", typeof(decimal));
            excelDataTable.Columns.Add("Дата контракта", typeof(DateTime));
            excelDataTable.Columns.Add("Дата начала платежа", typeof(DateTime));
            excelDataTable.Columns.Add("Количество месяцев", typeof(int));
            excelDataTable.Columns.Add("Квартира", typeof(string));

            var ContractsList = _mapper.Map<List<ContractResponse>>(contractResponses);

            if (ContractsList.Count > 0)
            {
                ContractsList.ForEach(item =>
                {
                    var currentDate = DateTime.Now;
                    var lastScheduledPayment = item.ScheduledInfo
                        .Where(entry => entry.Key <= currentDate)
                        .OrderByDescending(entry => entry.Key)
                        .FirstOrDefault();

                    decimal lastScheduledPaymentValue = lastScheduledPayment.Value;

                    excelDataTable.Rows.Add(
                        item.ContractNumber, 
                        item.Customer.FirstName + " " + item.Customer.LastName + " " +item.Customer.MiddleName,
                        item.Customer.PhoneNumberOne,
                        item.Customer.PhoneNumberTwo,
                        item.TotalAmountOfContract, 
                        item.InAdvancePaymentOfContract,
                        item.AmountOfMonthlyPayment,
                        lastScheduledPaymentValue,
                        item.ActualInfo.Values.Sum(),
                        item.DeptAmout,
                        item.ContractStartDate, 
                        item.PaymentStartDate,
                        item.NumberOfMonths, 
                        item.Home.Block + " / " + item.Home.Entrance.ToString() + " / " + item.Home.Floor.ToString() + " / " + item.Home.ApartmentNumber.ToString() + " / " + item.Home.Area.ToString()
                        );
                });
            }

            return excelDataTable;
        }
    }
}
