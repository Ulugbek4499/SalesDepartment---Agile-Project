using AutoMapper;
using SalesDepartment.Application.UseCases.Payments.Commands.CreatePayment;
using SalesDepartment.Application.UseCases.Payments.Commands.DeletePayment;
using SalesDepartment.Application.UseCases.Payments.Commands.UpdatePayment;
using SalesDepartment.Application.UseCases.Payments.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Mappings
{
    public class PaymentMapping : Profile
    {
        public PaymentMapping()
        {
            CreateMap<CreatePaymentCommand, Payment>().ReverseMap();
            CreateMap<DeletePaymentCommand, Payment>().ReverseMap();
            CreateMap<UpdatePaymentCommand, Payment>().ReverseMap();
            CreateMap<PaymentResponse, Payment>().ReverseMap();
        }
    }
}
