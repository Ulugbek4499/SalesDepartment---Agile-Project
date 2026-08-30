using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Contracts.Reports
{
    public class GetContractsExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetContractsExcelHandler : IRequestHandler<GetContractsExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContractsExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetContractsExcel request, CancellationToken cancellationToken)
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

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "Contract/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetContractsAsync(CancellationToken cancellationToken = default)
        {
            var AllContracts = await _context.Contracts.ToListAsync(cancellationToken);

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Контракт №", typeof(string));
            excelDataTable.Columns.Add("Дата контракта", typeof(DateTime));
            excelDataTable.Columns.Add("Сумма контракта", typeof(decimal));
            excelDataTable.Columns.Add("Дата начала платежа", typeof(DateTime));
            excelDataTable.Columns.Add("Количество месяцев", typeof(int));
            excelDataTable.Columns.Add("Предоплата", typeof(decimal));
            excelDataTable.Columns.Add("Квартира", typeof(string));
            excelDataTable.Columns.Add("Клиент", typeof(string));
            excelDataTable.Columns.Add("Основатель", typeof(string));

            var ContractsList = _mapper.Map<List<ContractResponse>>(AllContracts);

            if (ContractsList.Count > 0)
            {
                ContractsList.ForEach(item =>
                {
                    excelDataTable.Rows.Add(item.ContractNumber, item.ContractStartDate, item.TotalAmountOfContract, item.PaymentStartDate,
                        item.NumberOfMonths, item.InAdvancePaymentOfContract, item.Home.Block+" " + item.Home.Entrance.ToString() + " " + item.Home.Floor.ToString() + " " + item.Home.ApartmentNumber.ToString() + " " + item.Home.Area.ToString(), item.Customer.FirstName + " " + item.Customer.LastName, item.Founder.FirstName + " " + item.Founder.LastName);
                });
            }

            return excelDataTable;
        }
    }
}
