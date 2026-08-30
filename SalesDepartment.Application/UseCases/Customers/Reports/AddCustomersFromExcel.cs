using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Customers.Response;

namespace SalesDepartment.Application.UseCases.Customers.Reports
{
    public record AddCustomersFromExcel(IFormFile ExcelFile) : IRequest<List<CustomerResponse>>;

    public class AddCustomersFromExcelHandler : IRequestHandler<AddCustomersFromExcel, List<CustomerResponse>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddCustomersFromExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerResponse>> Handle(AddCustomersFromExcel request, CancellationToken cancellationToken)
        {
            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<Domain.Entities.Customer> result = new();
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var Customer = new Domain.Entities.Customer()
                        {
                            FirstName = sheet1.Cell(row, 1).GetString(),
                            LastName = sheet1.Cell(row, 2).GetString(),
                            MiddleName = sheet1.Cell(row, 3).GetString(),
                            Passport = sheet1.Cell(row, 4).GetString(),
                            PassportIssuedBy = sheet1.Cell(row, 5).GetString(),
                            Address = sheet1.Cell(row, 6).GetString(),
                            PhoneNumberOne = sheet1.Cell(row, 7).GetString(),
                            PhoneNumberTwo = sheet1.Cell(row, 8).GetString()
                        };

                        result.Add(Customer);
                    }
                }
            }

            await _context.Customers.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return _mapper.Map<List<CustomerResponse>>(result);
        }
    }
}
