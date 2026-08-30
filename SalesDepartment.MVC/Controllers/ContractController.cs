using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.Contracts.Commands.CreateContract;
using SalesDepartment.Application.UseCases.Contracts.Commands.DeleteContract;
using SalesDepartment.Application.UseCases.Contracts.Commands.UpdateContract;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetAllContracts;
using SalesDepartment.Application.UseCases.Contracts.Queries.GetContractById;
using SalesDepartment.Application.UseCases.Contracts.Reports;
using SalesDepartment.Application.UseCases.Contracts.Response;
using SalesDepartment.Application.UseCases.Customers.Queries.GetAllCustomers;
using SalesDepartment.Application.UseCases.Customers.Response;
using SalesDepartment.Application.UseCases.Founders.Queries.GetAllFounders;
using SalesDepartment.Application.UseCases.Founders.Response;
using SalesDepartment.Application.UseCases.Homes.Queries.GetAllHomes;
using SalesDepartment.Application.UseCases.Homes.Response;
using Xceed.Words.NET;

namespace SalesDepartment.MVC.Controllers;

public class ContractController : ApiBaseController
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ContractController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> CreateContract()
    {
        FounderResponse[] founders = await Mediator.Send(new GetAllFoundersQuery());
        ViewData["Founders"] = founders;

        CustomerResponse[] customers = await Mediator.Send(new GetAllCustomersQuery());
        ViewData["Customers"] = customers;

        HomeResponse[] homes = await Mediator.Send(new GetAllHomesQuery());
        ViewData["Homes"] = homes;

        return View();
    }

    [HttpPost("[action]")]
    public async ValueTask<IActionResult> CreateContract([FromForm] CreateContractCommand Contract)
    {
        await Mediator.Send(Contract);

        return RedirectToAction("GetAllContracts");
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> CreateContractFromExcel()
    {
        return View();
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> GetAllContracts()
    {
        var Contracts = await Mediator.Send(new GetAllContractsQuery());

        return View(Contracts);
    }

    [HttpGet("[action]")]
    public async ValueTask<FileResult> GetAllContractsExcel(string fileName = "ВсеКонтракты")
    {
        var result = await Mediator.Send(new GetContractsExcel { FileName = fileName });

        return File(result.FileContents, result.Option, result.FileName);
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> UpdateContract(int Id)
    {
        var Contract = await Mediator.Send(new GetContractByIdQuery(Id));

        FounderResponse[] founders = await Mediator.Send(new GetAllFoundersQuery());
        ViewData["Founders"] = founders;

        CustomerResponse[] customers = await Mediator.Send(new GetAllCustomersQuery());
        ViewData["Customers"] = customers;

        HomeResponse[] homes = await Mediator.Send(new GetAllHomesQuery());
        ViewData["Homes"] = homes;

        return View(Contract);
    }

    [HttpPost("[action]")]
    public async ValueTask<IActionResult> UpdateContract([FromForm] UpdateContractCommand Contract)
    {
        await Mediator.Send(Contract);
        return RedirectToAction("GetAllContracts");
    }

    public async ValueTask<IActionResult> DeleteContract(int Id)
    {
        await Mediator.Send(new DeleteContractCommand(Id));

        return RedirectToAction("GetAllContracts");
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> ViewContract(int id)
    {
        var Contract = await Mediator.Send(new GetContractByIdQuery(id));

        return View("ViewContract", Contract);
    }

    [HttpGet("[action]")]
    public async ValueTask<IActionResult> ContractInDocx(int id)
    {

        ContractResponse contract = await Mediator.Send(new GetContractByIdQuery(id));
        string templateFileName = "ДОГОВОР.docx"; // Assuming the template file is in the wwwroot/docs folder

        string TotalAmountInWords = PropLat(contract.TotalAmountOfContract);
        decimal SumOfOneMeterSquare = Math.Floor(contract.TotalAmountOfContract / contract.Home.Area);
        string SumOfOneMeterSquareInWords = PropLat(SumOfOneMeterSquare);

        // Get the wwwroot path using IWebHostEnvironment
        string webRootPath = _webHostEnvironment.WebRootPath;

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
        doc.ReplaceText("«Сумма_контракта_прописью»", TotalAmountInWords);
        doc.ReplaceText("«Сумма_1_М2»", SumOfOneMeterSquare.ToString("N2"));

        doc.ReplaceText("«Сумма_1_М2_прописью»", SumOfOneMeterSquareInWords);
        doc.ReplaceText("«Тел_Ном_»", contract.Customer.PhoneNumberOne);

        string newFilename = $"Договор_{contract.Customer.LastName + "_" + contract.Customer.FirstName}.docx";
        string newFilePath = Path.Combine(webRootPath, "docs", newFilename);

        doc.SaveAs(newFilePath);

        var fileBytes = System.IO.File.ReadAllBytes(newFilePath);
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

