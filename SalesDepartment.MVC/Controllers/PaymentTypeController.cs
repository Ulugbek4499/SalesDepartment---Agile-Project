using Microsoft.AspNetCore.Mvc;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.CreatePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.DeletePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetAllPaymentTypes;
using SalesDepartment.Application.UseCases.PaymentTypes.Queries.GetPaymentTypeById;

namespace SalesDepartment.MVC.Controllers
{
    public class PaymentTypeController : ApiBaseController
    {
        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreatePaymentType()
        {
            return View();
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> CreatePaymentType([FromForm] CreatePaymentTypeCommand PaymentType)
        {
            await Mediator.Send(PaymentType);

            return RedirectToAction("GetAllPaymentTypes");
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> CreatePaymentTypeFromExcel()
        {
            return View();
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> GetAllPaymentTypes()
        {
            var PaymentTypes = await Mediator.Send(new GetAllPaymentTypesQuery());

            return View(PaymentTypes);
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> UpdatePaymentType(int Id)
        {
            var PaymentType = await Mediator.Send(new GetPaymentTypeByIdQuery(Id));

            return View(PaymentType);
        }

        [HttpPost("[action]")]
        public async ValueTask<IActionResult> UpdatePaymentType([FromForm] UpdatePaymentTypeCommand PaymentType)
        {
            await Mediator.Send(PaymentType);
            return RedirectToAction("GetAllPaymentTypes");
        }

        public async ValueTask<IActionResult> DeletePaymentType(int Id)
        {
            await Mediator.Send(new DeletePaymentTypeCommand(Id));

            return RedirectToAction("GetAllPaymentTypes");
        }

        [HttpGet("[action]")]
        public async ValueTask<IActionResult> ViewPaymentType(int id)
        {
            var PaymentType = await Mediator.Send(new GetPaymentTypeByIdQuery(id));

            return View("ViewPaymentType", PaymentType);
        }
    }
}
