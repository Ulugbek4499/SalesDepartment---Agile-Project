using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Founders.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Founders.Queries.GetFounderById
{
    public record GetFounderByIdQuery(int Id) : IRequest<FounderResponse>;

    public class GetFounderByIdQueryHandler : IRequestHandler<GetFounderByIdQuery, FounderResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetFounderByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<FounderResponse> Handle(GetFounderByIdQuery request, CancellationToken cancellationToken)
        {
            var Founder = FilterIfFounderExsists(request.Id);

            var result = _mapper.Map<FounderResponse>(Founder);
            return await Task.FromResult(result);
        }

        private Founder FilterIfFounderExsists(int id)
            => _dbContext.Founders
                .Find(id) ?? throw new NotFoundException(
                    " There is no Founder with this Id. ");
    }
}
