using AutoMapper;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.CreatePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.DeletePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Commands.UpdatePaymentType;
using SalesDepartment.Application.UseCases.PaymentTypes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.Common.Mappings
{
    public class PaymentTypeMapping : Profile
    {
        public PaymentTypeMapping()
        {
            CreateMap<CreatePaymentTypeCommand, PaymentType>().ReverseMap();
            CreateMap<DeletePaymentTypeCommand, PaymentType>().ReverseMap();
            CreateMap<UpdatePaymentTypeCommand, PaymentType>().ReverseMap();
            CreateMap<PaymentTypeResponse, PaymentType>().ReverseMap();
        }
    }
}
