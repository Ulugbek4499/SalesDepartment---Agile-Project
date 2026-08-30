using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Payments.Response;

namespace SalesDepartment.Application.UseCases.Payments.Reports
{
    public class GetPaymentsExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetPaymentsExcelHandler : IRequestHandler<GetPaymentsExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentsExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetPaymentsExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook workbook = new())
            {
                var orderData = await GetPaymentsAsync(cancellationToken);
                var excelSheet = workbook.AddWorksheet(orderData, "Payments");

                excelSheet.RowHeight = 20;
                excelSheet.Column(1).Width = 18;
                excelSheet.Column(2).Width = 18;
                excelSheet.Column(3).Width = 18;
                excelSheet.Column(4).Width = 18;
                excelSheet.Column(5).Width = 18;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "Payment/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetPaymentsAsync(CancellationToken cancellationToken = default)
        {
            var AllPayments = await _context.Payments.ToListAsync(cancellationToken);

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Платеж №", typeof(string));
            excelDataTable.Columns.Add("Дата платежа", typeof(DateTime));
            excelDataTable.Columns.Add("Сумма", typeof(decimal));
            excelDataTable.Columns.Add("Контракт №", typeof(string));
            excelDataTable.Columns.Add("Тип платежей", typeof(string));

            var PaymentsList = _mapper.Map<List<PaymentResponse>>(AllPayments);

            if (PaymentsList.Count > 0)
            {
                PaymentsList.ForEach(item =>
                {
                    excelDataTable.Rows.Add(item.PaymentNumber, item.PaymentDate, item.Amount, item.Contract.ContractNumber,
                        item.PaymentType.Name);
                });
            }

            return excelDataTable;
        }
    }
}
