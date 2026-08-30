using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Customers.Response;

namespace SalesDepartment.Application.UseCases.Customers.Reports
{
    public class GetCustomersExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetCustomersExcelHandler : IRequestHandler<GetCustomersExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetCustomersExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook workbook = new())
            {
                var orderData = await GetCustomersAsync(cancellationToken);
                var excelSheet = workbook.AddWorksheet(orderData, "Customers");

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

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "Customer/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetCustomersAsync(CancellationToken cancellationToken = default)
        {
            var AllCustomers = await _context.Customers.ToListAsync(cancellationToken);

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Имя", typeof(string));
            excelDataTable.Columns.Add("Фамилия", typeof(string));
            excelDataTable.Columns.Add("Очество", typeof(string));
            excelDataTable.Columns.Add("Паспорт", typeof(string));
            excelDataTable.Columns.Add("Выдан Паспорт", typeof(string));
            excelDataTable.Columns.Add("Адрес", typeof(string));
            excelDataTable.Columns.Add("Номер телефона 1", typeof(string));
            excelDataTable.Columns.Add("Номер телефона 2", typeof(string));

            var CustomersList = _mapper.Map<List<CustomerResponse>>(AllCustomers);

            if (CustomersList.Count > 0)
            {
                CustomersList.ForEach(item =>
                {
                    excelDataTable.Rows.Add(item.FirstName, item.LastName, item.MiddleName, item.Passport,
                        item.PassportIssuedBy, item.Address, item.PhoneNumberOne, item.PhoneNumberTwo);
                });
            }

            return excelDataTable;
        }
    }
}
