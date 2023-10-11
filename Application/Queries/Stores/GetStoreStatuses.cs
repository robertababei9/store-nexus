using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Stores;
using Common.Models;

namespace Application.Queries.Stores
{
    public static class GetStoreStatuses
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IStoreStatusRepository _storeStatusRepository { get; set; }
            public Handler(IStoreStatusRepository storeStatusRepository)
            {
                _storeStatusRepository = storeStatusRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<IEnumerable<SelectOptionModel<Guid>>>();

                var results = _storeStatusRepository
                        .GetAllQueryable()
                        .OrderBy(x => x.StoreStatusType)
                        .Select(x => new SelectOptionModel<Guid>
                        {
                            label = x.Description,
                            value = x.Id
                        })
                        .ToList();

                response.Success = true;
                response.Data = results;

                return new Response(response);
            }
        }


        // Response
        public record Response(
            ApiResponseModel<
                IEnumerable<SelectOptionModel<Guid>>
            > response
        );
    }
}

