using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Contracts.Response;

namespace SalesDepartment.Application.UseCases.Reports;

public class ContractsWithDept : IRequest<ExcelReportResponse>
{
    public string FileName { get; set; }
    public DateTime CreatedFromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class ContractsByDayExcelHandler : IRequestHandler<ContractsWithDept, ExcelReportResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ContractsByDayExcelHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ExcelReportResponse> Handle(ContractsWithDept request, CancellationToken cancellationToken)
    {
        using (var workbook = new XLWorkbook())
        {
            var contractsInRange = await GetContractsInRangeAsync(request.CreatedFromDate, request.ToDate, cancellationToken);
            var excelSheet = workbook.AddWorksheet(contractsInRange, "Contracts");

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

            using (var memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);

                return new ExcelReportResponse(
                    memoryStream.ToArray(),
                    "Contract/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{request.FileName}.xlsx");
            }
        }
    }

    private async Task<DataTable> GetContractsInRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
    {
        var contractsInRange = await _context.Contracts
            .Where(c => c.ContractStartDate >= fromDate && c.ContractStartDate <= toDate)
            .ToListAsync(cancellationToken);

        var excelDataTable = new DataTable
        {
            TableName = "ContractsInRange"
        };

        excelDataTable.Columns.Add("ContractNumber", typeof(string));
        excelDataTable.Columns.Add("ContractStartDate", typeof(DateTime));
        excelDataTable.Columns.Add("TotalAmountOfContract", typeof(decimal));
        excelDataTable.Columns.Add("PaymentStartDate", typeof(DateTime));
        excelDataTable.Columns.Add("NumberOfMonths", typeof(int));
        excelDataTable.Columns.Add("InAdvancePaymentOfContract", typeof(decimal));
        excelDataTable.Columns.Add("Home Number", typeof(string));
        excelDataTable.Columns.Add("Customer FullName", typeof(string));
        excelDataTable.Columns.Add("Founder Name", typeof(string));

        var contractResponses = _mapper.Map<List<ContractResponse>>(contractsInRange);

        if (contractResponses.Count > 0)
        {
            contractResponses.ForEach(item =>
            {
                excelDataTable.Rows.Add(item.ContractNumber, item.ContractStartDate, item.TotalAmountOfContract, item.PaymentStartDate,
                     item.NumberOfMonths, item.InAdvancePaymentOfContract, item.Home.Block + " " + item.Home.Entrance.ToString() + " " + item.Home.Floor.ToString() + " " + item.Home.ApartmentNumber.ToString() + " " + item.Home.Area.ToString(), item.Customer.FirstName + " " + item.Customer.LastName, item.Founder.FirstName + " " + item.Founder.LastName);
            });
        }

        return excelDataTable;
    }
}
