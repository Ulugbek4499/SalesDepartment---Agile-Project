using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetAllContracts;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetContractById;
using SalesDepartment.Application.UseCases.Contracts.Reports;
using SalesDepartment.Application.UseCases.Contracts.Response;
using SalesDepartment.Application.UseCases.Reports;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace SalesDepartment.MVC.Controllers
{
    public class ReportController:ApiBaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        public ReportController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetStatistics()
        {
            var statistic = await Mediator.Send(new GetStatisticsQuery());

            return View(statistic);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetContractsBetweenDates(DateTime fromDate, DateTime toDate, string fileName = "ContractExcel")
        {
            var result = await Mediator.Send(new ContractsWithDept { CreatedFromDate = fromDate, ToDate = toDate, FileName = fileName}) ;

            return File(result.FileContents, result.Option, result.FileName);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetContractsWithDepts(DateTime Time)
        {
            var contractsWithDepts = await Mediator.Send(new GetContractsWithDepts(Time));

            return View(contractsWithDepts);
        }

        [HttpGet("[action]")]
        public async ValueTask<FileResult> GetContractsWithDebtExcel(string fileName = "Контракты_с_Долгом")
        {
            var result = await Mediator.Send(new GetContractsWithDebtExcel { FileName = fileName });

            return File(result.FileContents, result.Option, result.FileName);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GraphicInDocx(int id)
        {

            ContractResponse contract = await Mediator.Send(new GetContractByIdQuery(id));
            string templateFileName = "Grafik.docx"; // Assuming the template file is in the wwwroot/docs folder

            // Get the wwwroot path using IWebHostEnvironment
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Construct the full path to the template file
            string templatePath = Path.Combine(webRootPath, "docs", templateFileName);

            var doc = DocX.Load(templatePath);

            doc.ReplaceText("«ДОГОВОР_»", contract.ContractNumber);
            doc.ReplaceText("«Дата_контракта»", contract.ContractStartDate.ToString("dd/MM/yyyy"));
            doc.ReplaceText("«ФИO_Инвестор»", contract.Customer.LastName + " " + contract.Customer.FirstName + " " + contract.Customer.MiddleName);
            doc.ReplaceText("«Паспорт_Серия_Инвестор»", contract.Customer.Passport);
            doc.ReplaceText("«Выдан_Паспорт__Инвестор»", contract.Customer.PassportIssuedBy);

            doc.ReplaceText("«Mесто_жительства»", contract.Customer.Address);
            doc.ReplaceText("««Здания»»", contract.Home.Block);
            doc.ReplaceText("«Подъезд_»", contract.Home.Entrance.ToString());
            doc.ReplaceText("«Квартира_»", contract.Home.ApartmentNumber.ToString());
            doc.ReplaceText("«Количество_комнат»", contract.Home.NumberOfRooms.ToString());

            doc.ReplaceText("«Этаже»", contract.Home.Floor.ToString());
            doc.ReplaceText("«Проектной_площадью»", contract.Home.Area.ToString());
            doc.ReplaceText("«Общая_сумма_контракта»", contract.TotalAmountOfContract.ToString("N2"));
            doc.ReplaceText("«advance_amount»", contract.InAdvancePaymentOfContract.ToString("N2"));
            doc.ReplaceText("«monthly_payment»", contract.AmountOfMonthlyPayment.ToString("N2"));
            doc.ReplaceText("«number_of_month»", contract.NumberOfMonths.ToString());

            doc.ReplaceText("«Тел_Ном_»", contract.Customer.PhoneNumberOne);

            var paymentTable = doc.AddTable(contract.NumberOfMonths + 1, 4); // +1 for the header row
            paymentTable.Design = TableDesign.TableGrid;

            paymentTable.Rows[0].Cells[0].Paragraphs.First().Append("№");
            paymentTable.Rows[0].Cells[1].Paragraphs.First().Append("Даты Oплаты");
            paymentTable.Rows[0].Cells[2].Paragraphs.First().Append("Ежемесячно Oплата");
            paymentTable.Rows[0].Cells[3].Paragraphs.First().Append("Остаток Долга");

            decimal monthlyPayment = (contract.TotalAmountOfContract - contract.InAdvancePaymentOfContract) / contract.NumberOfMonths;
            decimal remainingDebt = contract.TotalAmountOfContract - contract.InAdvancePaymentOfContract;

            DateTime paymentDate = contract.PaymentStartDate;

            for (int month = 1; month <= contract.NumberOfMonths; month++)
            {
                // Fill in the table with the calculated values
                paymentTable.Rows[month].Cells[0].Paragraphs.First().Append(month.ToString());
                paymentTable.Rows[month].Cells[1].Paragraphs.First().Append(paymentDate.ToString("dd/MM/yyyy"));
                paymentTable.Rows[month].Cells[2].Paragraphs.First().Append(monthlyPayment.ToString("N2"));
                paymentTable.Rows[month].Cells[3].Paragraphs.First().Append(remainingDebt.ToString("N2"));

                // Update payment date and remaining debt for the next iteration
                paymentDate = paymentDate.AddMonths(1);
                remainingDebt -= monthlyPayment;
            }

            doc.InsertTable(paymentTable);

            var newFilename = $"Graphic_{contract.ContractNumber}.docx";

            doc.SaveAs(newFilename);

            var fileBytes = System.IO.File.ReadAllBytes(newFilename);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", newFilename);

        }
    }
}
