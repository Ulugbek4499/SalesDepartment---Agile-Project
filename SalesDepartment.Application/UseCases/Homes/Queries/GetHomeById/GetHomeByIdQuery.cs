using AutoMapper;
using MediatR;
using SalesDepartment.Application.Common.Exceptions;
using SalesDepartment.Application.Common.Interfaces;
using SalesDepartment.Application.UseCases.Homes.Response;
using SalesDepartment.Domain.Entities;

namespace SalesDepartment.Application.UseCases.Homes.Queries.GetHomeById
{
    public record GetHomeByIdQuery(int Id) : IRequest<HomeResponse>;

    public class GetHomeByIdQueryHandler : IRequestHandler<GetHomeByIdQuery, HomeResponse>
    {
        IApplicationDbContext _dbContext;
        IMapper _mapper;

        public GetHomeByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<HomeResponse> Handle(GetHomeByIdQuery request, CancellationToken cancellationToken)
        {
            var Home = FilterIfHomeExsists(request.Id);

            var result = _mapper.Map<HomeResponse>(Home);
            return await Task.FromResult(result);
        }

        private Home FilterIfHomeExsists(int id)
            => _dbContext.Homes
                .Find(id)
                     ?? throw new NotFoundException(
                            " There is no Home with this Id. ");
    }
}
