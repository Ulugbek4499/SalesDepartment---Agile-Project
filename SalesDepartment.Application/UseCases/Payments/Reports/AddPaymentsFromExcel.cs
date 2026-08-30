using AutoMapper;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Http;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Payments.Response;

namespace SalesDepartment.Application.UseCases.Payments.Reports
{
    public record AddPaymentsFromExcel(IFormFile ExcelFile) : IRequest<List<PaymentResponse>>;

    public class AddPaymentsFromExcelHandler : IRequestHandler<AddPaymentsFromExcel, List<PaymentResponse>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AddPaymentsFromExcelHandler(IApplicationDbContext context, IMapper mapper)
        {

            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PaymentResponse>> Handle(AddPaymentsFromExcel request, CancellationToken cancellationToken)
        {
            if (request.ExcelFile == null || request.ExcelFile.Length == 0)
                throw new ArgumentNullException("File", "file is empty or null");

            var file = request.ExcelFile;
            List<Domain.Entities.Payment> result = new();
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, cancellationToken);
                using (var wb = new XLWorkbook(ms))
                {
                    var sheet1 = wb.Worksheet(1);
                    int startRow = 2;
                    for (int row = startRow; row <= sheet1.LastRowUsed().RowNumber(); row++)
                    {
                        var Payment = new Domain.Entities.Payment()
                        {
                            PaymentNumber = sheet1.Cell(row, 1).GetString(),
                            PaymentDate = DateTime.Parse(sheet1.Cell(row, 2).GetString()),
                            Amount = decimal.Parse(sheet1.Cell(row, 3).GetString()),
                            ContractId = int.Parse(sheet1.Cell(row, 4).GetString()),
                            PaymentTypeId = int.Parse(sheet1.Cell(row, 5).GetString())
                        };

                        result.Add(Payment);
                    }
                }
            }

            await _context.Payments.AddRangeAsync(result);
            await _context.SaveChangesAsync();
            return _mapper.Map<List<PaymentResponse>>(result);
        }
    }
}
