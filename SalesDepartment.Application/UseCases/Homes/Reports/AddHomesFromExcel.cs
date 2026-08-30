using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Homes.Response;

namespace SalesDepartment.Application.UseCases.Homes.Reports
{
    public record AddHomesFromExcel(IFormFile ExcelFile) : IRequest<List<HomeResponse>>;

    public class AddHomesFromExcelHandler : IRequestHandler<AddHomesFromExcel, List<HomeResponse>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddHomesFromExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HomeResponse>> Handle(AddHomesFromExcel request, CancellationToken cancellationToken)
        {
            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<Domain.Entities.Home> result = new();
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var Home = new Domain.Entities.Home()
                        {
                            Block = sheet1.Cell(row, 1).GetString(),
                            Entrance = int.Parse(sheet1.Cell(row, 2).GetString()),
                            Floor = int.Parse(sheet1.Cell(row, 3).GetString()),
                            ApartmentNumber = int.Parse(sheet1.Cell(row, 4).GetString()),
                            NumberOfRooms = int.Parse(sheet1.Cell(row, 5).GetString()),
                            Area = decimal.Parse(sheet1.Cell(row, 6).GetString())
                        };

                        result.Add(Home);
                    }
                }
            }

            await _context.Homes.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return _mapper.Map<List<HomeResponse>>(result);
        }
    }
}
