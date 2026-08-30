using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.Founders.Commands.CreateFounder;
using SalesDepartment.Application.UseCases.Founders.Commands.DeleteFounder;
using SalesDepartment.Application.UseCases.Founders.Commands.UpdateFounder;
using SalesDepartment.Application.UseCases.Founders.Queries.GetAllFounders;
using SalesDepartment.Application.UseCases.Founders.Queries.GetFounderById;

namespace SalesDepartment.MVC.Controllers
{
    public class FounderController : ApiBaseController
    {
        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreateFounder()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> CreateFounder([FromForm] CreateFounderCommand Founder)
        {
            await Mediator.Send(Founder);

            return RedirectToAction("GetAllFounders");
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreateFounderFromExcel()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetAllFounders()
        {
            var Founders = await Mediator.Send(new GetAllFoundersQuery());

            return View(Founders);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> UpdateFounder(int Id)
        {
            var Founder = await Mediator.Send(new GetFounderByIdQuery(Id));

            return View(Founder);
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> UpdateFounder([FromForm] UpdateFounderCommand Founder)
        {
            await Mediator.Send(Founder);
            return RedirectToAction("GetAllFounders");
        }

        public async ValueTask<IActionResult> DeleteFounder(int Id)
        {
            await Mediator.Send(new DeleteFounderCommand(Id));

            return RedirectToAction("GetAllFounders");
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> ViewFounder(int id)
        {
            var Founder = await Mediator.Send(new GetFounderByIdQuery(id));

            return View("ViewFounder", Founder);
        }
    }
}
