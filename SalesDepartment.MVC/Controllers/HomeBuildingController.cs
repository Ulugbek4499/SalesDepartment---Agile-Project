using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.Homes.Commands.CreateHome;
using SalesDepartment.Application.UseCases.Homes.Commands.DeleteHome;
using SalesDepartment.Application.UseCases.Homes.Commands.UpdateHome;
using SalesDepartment.Application.UseCases.Homes.Queries.GetAllHomes;
using SalesDepartment.Application.UseCases.Homes.Queries.GetHomeById;
using SalesDepartment.Application.UseCases.Homes.Reports;

namespace SalesDepartment.MVC.Controllers
{
    public class HomeBuildingController : ApiBaseController
    {
        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreateHome()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> CreateHome([FromForm] CreateHomeCommand Home)
        {
            await Mediator.Send(Home);

            return RedirectToAction("GetAllHomes");
        }
     
        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreateHomeFromExcel()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> CreateHomeFromExcel(IFormFile excelfile)
        {
            var result = await Mediator.Send(new AddHomesFromExcel(excelfile));

            return RedirectToAction("GetAllHomes");
        }


        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetAllHomes()
        {
            var Homes = await Mediator.Send(new GetAllHomesQuery());

            return View(Homes);
        }

        [HttpGet("[action]")]
        public async ValueTask<FileResult> GetAllHomesExcel(string fileName = "ВсеКвартиры")
        {
            var result = await Mediator.Send(new GetHomesExcel { FileName = fileName });

            return File(result.FileContents, result.Option, result.FileName);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> UpdateHome(int Id)
        {
            var Home = await Mediator.Send(new GetHomeByIdQuery(Id));

            return View(Home);
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> UpdateHome([FromForm] UpdateHomeCommand Home)
        {
            await Mediator.Send(Home);
            return RedirectToAction("GetAllHomes");
        }

        public async ValueTask<IActionResult> DeleteHome(int Id)
        {
            await Mediator.Send(new DeleteHomeCommand(Id));

            return RedirectToAction("GetAllHomes");
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> ViewHome(int id)
        {
            var Home = await Mediator.Send(new GetHomeByIdQuery(id));

            return View("ViewHome", Home);
        }
    }
}
