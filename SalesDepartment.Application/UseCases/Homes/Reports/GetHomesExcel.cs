using System.Data;
using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.Common.Models;
using SalesDepartment.Application.UseCases.Homes.Response;

namespace SalesDepartment.Application.UseCases.Homes.Reports
{
    public class GetHomesExcel : IRequest<ExcelReportResponse>
    {
        public string FileName { get; set; }
    }

    public class GetHomesExcelHandler : IRequestHandler<GetHomesExcel, ExcelReportResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetHomesExcelHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ExcelReportResponse> Handle(GetHomesExcel request, CancellationToken cancellationToken)
        {
            using (XLWorkbook workbook = new())
            {
                var orderData = await GetHomesAsync(cancellationToken);
                var excelSheet = workbook.AddWorksheet(orderData, "Homes");

                excelSheet.RowHeight = 20;
                excelSheet.Column(1).Width = 18;
                excelSheet.Column(2).Width = 18;
                excelSheet.Column(3).Width = 18;
                excelSheet.Column(4).Width = 18;
                excelSheet.Column(5).Width = 18;
                excelSheet.Column(6).Width = 18;
                excelSheet.Column(7).Width = 18;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);

                    return new ExcelReportResponse(memoryStream.ToArray(), "Home/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{request.FileName}.xlsx");
                }
            }
        }

        private async Task<DataTable> GetHomesAsync(CancellationToken cancellationToken = default)
        {
            var AllHomes = await _context.Homes.ToListAsync(cancellationToken);

            DataTable excelDataTable = new()
            {
                TableName = "Empdata"
            };

            excelDataTable.Columns.Add("Здания", typeof(string));
            excelDataTable.Columns.Add("Подъезд №", typeof(int));
            excelDataTable.Columns.Add("Этаж", typeof(int));
            excelDataTable.Columns.Add("Квартира №", typeof(int));
            excelDataTable.Columns.Add("Количество комнат", typeof(int));
            excelDataTable.Columns.Add("Проектной площадью", typeof(decimal));
            excelDataTable.Columns.Add("Контракт №", typeof(string));

            var HomesList = _mapper.Map<List<HomeResponse>>(AllHomes);

            if (HomesList.Count > 0)
            {
                HomesList.ForEach(item =>
                {
                    excelDataTable.Rows.Add(
                        item.Block,
                        item.Entrance,
                        item.Floor,
                        item.ApartmentNumber,
                        item.NumberOfRooms,
                        item.Area,
                        item.Contract?.ContractNumber); // Use ?. to handle nullable Contract.ContractNumber
                });
            }

            return excelDataTable;
        }
    }
}
