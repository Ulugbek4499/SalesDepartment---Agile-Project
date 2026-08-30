using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetAllContracts;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetContractById;
using SalesDepartment.Application.UseCases.Contracts.Response;
using SalesDepartment.Application.UseCases.Payments.Commands.CreatePayment;
using SalesDepartment.Application.UseCases.Payments.Commands.DeletePayment;
using SalesDepartment.Application.UseCases.Payments.Commands.UpdatePayment;
using SalesDepartment.Application.UseCases.Payments.Queries.GetAllPayments;
using SalesDepartment.Application.UseCases.Payments.Queries.GetPaymentById;
using SalesDepartment.Application.UseCases.Payments.Reports;
using SalesDepartment.Application.UseCases.Payments.Response;
using SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetAllPaymentTypes;
using SalesDepartment.Application.UseCases.PaymentTypes.Response;
using Xceed.Words.NET;

namespace SalesDepartment.MVC.Controllers;

public class PaymentController : ApiBaseController
{

    private readonly IWebHostEnvironment _hostingEnvironment;

    public PaymentController(IWebHostEnvironment webHostEnvironment)
    {
        _hostingEnvironment = webHostEnvironment;
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> CreatePayment()
    {
        PaymentTypeResponse[] paymentTypes = await Mediator.Send(new GetAllPaymentTypesQuery());
        ViewData["PaymentTypes"] = paymentTypes;

        ContractResponse[] contracts = await Mediator.Send(new GetAllContractsQuery());
        ViewData["Contracts"] = contracts;

        return View();
    }

    [HttpPost("[action]")]
    public async ValueTask<IActionResult> CreatePayment([FromForm] CreatePaymentCommand Payment)
    {
        ContractResponse[] contracts = await Mediator.Send(new GetAllContractsQuery());
        var contractExists = contracts.Any(c => c.Id == Payment.ContractId);

        if (!contractExists)
        {
            ModelState.AddModelError("ContractId", "Invalid contract selected.");
            return View();
        }

        await Mediator.Send(Payment);

        return RedirectToAction("GetAllPayments");
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> CreatePaymentFromExcel()
    {
        return View();
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> GetAllPayments()
    {
        var Payments = await Mediator.Send(new GetAllPaymentsQuery());

        return View(Payments);
    }

    [HttpGet("[action]")]
    public async ValueTask<FileResult> GetAllPaymentsExcel(string fileName = "ВсеПлатежи")
    {
        var result = await Mediator.Send(new GetPaymentsExcel { FileName = fileName });

        return File(result.FileContents, result.Option, result.FileName);
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> UpdatePayment(int Id)
    {
        var Payment = await Mediator.Send(new GetPaymentByIdQuery(Id));

        PaymentTypeResponse[] paymentTypes = await Mediator.Send(new GetAllPaymentTypesQuery());
        ViewData["PaymentTypes"] = paymentTypes;

        ContractResponse[] contracts = await Mediator.Send(new GetAllContractsQuery());
        ViewData["Contracts"] = contracts;

        return View(Payment);
    }

    [HttpPost("[action]")]
    public async ValueTask<IActionResult> UpdatePayment([FromForm] UpdatePaymentCommand Payment)
    {
        await Mediator.Send(Payment);
        return RedirectToAction("GetAllPayments");
    }

    public async ValueTask<IActionResult> DeletePayment(int Id)
    {
        await Mediator.Send(new DeletePaymentCommand(Id));

        return RedirectToAction("GetAllPayments");
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> ViewPayment(int id)
    {
        var Payment = await Mediator.Send(new GetPaymentByIdQuery(id));

        return View("ViewPayment", Payment);
    }


    [HttpGet("[action]")]
    public async Task<IActionResult> PaymentInDocxAsync(int id)
    {
        PaymentResponse payment = await Mediator.Send(new GetPaymentByIdQuery(id));
        string templateFileName = "Kvitansiya.docx"; // Assuming the template file is in the wwwroot/docs folder

        string amountInWords = PropLat(payment.Amount);

        // Get the wwwroot path using IWebHostEnvironment
        string webRootPath = _hostingEnvironment.WebRootPath;

        // Construct the full path to the template file
        string templatePath = Path.Combine(webRootPath, "docs", templateFileName);

        var doc = DocX.Load(templatePath);

        doc.ReplaceText("«Договар_»", payment.Contract.ContractNumber);
        doc.ReplaceText("«Договар_дата»", payment.Contract.ContractStartDate.ToString("dd/MM/yyyy"));
        doc.ReplaceText("«Принято_от_ФИО»", $"{payment.Contract.Customer.LastName} {payment.Contract.Customer.FirstName} {payment.Contract.Customer.MiddleName}");
        doc.ReplaceText("«Сумма»", payment.Amount.ToString("N2"));
        doc.ReplaceText("«Summa_soz_bilan»", amountInWords);

        doc.ReplaceText("«Номер_документа_»", payment.PaymentNumber);
        doc.ReplaceText("«Дата_составления»", payment.PaymentDate.ToString("dd/MM/yyyy"));

        var newFilename = $"Квитансия_{payment.Contract.Customer.LastName}_{payment.Contract.Customer.FirstName}_{payment.PaymentNumber}.docx";

        doc.SaveAs(newFilename);

        var fileBytes = System.IO.File.ReadAllBytes(newFilename);
        return File(fileBytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", newFilename);
    }

    public static string PropLat(decimal c)
    {
        string[] ed = { "", "один ", "два ", "три ", "четыре ", "пять ", "шесть ", "семь ", "восемь ", "девять " };
        string[] des = { "", "десять ", "двадцать ", "тридцать ", "сорок ", "пятьдесят ", "шестьдесят ", "семьдесят ", "восемьдесят ", "девяносто " };
        string[] razr = { "миллиардов ", "миллионов ", "тысяч ", "" };

        string st, qp, m = "";

        if (c > 999999999999 || c < 0)
        {
            return "";
        }

        st = c.ToString("000000000000.00", CultureInfo.InvariantCulture);
        qp = c.ToString(" 0.00", CultureInfo.InvariantCulture);

        if (Convert.ToDouble(st) == 0)
        {
            m = "ноль ";
        }

        for (int i = 0; i < 12; i += 3)
        {
            int hundreds = int.Parse(st[i].ToString());
            int tens = int.Parse(st[i + 1].ToString());
            int ones = int.Parse(st[i + 2].ToString());

            if (hundreds > 0)
            {
                m += ed[hundreds] + "сто ";
            }

            if (tens > 1)
            {
                m += des[tens];
                m += ed[ones];
            }
            else if (tens == 1)
            {
                m += des[tens + ones];
            }
            else if (ones > 0)
            {
                m += ed[ones];
            }

            if (hundreds + tens + ones > 0)
            {
                m += razr[i / 3];
            }
        }

        return char.ToUpper(m[0]) + m.Substring(1);
    }

}