using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Stores;
using Common.Models;

namespace Application.Queries.Stores
{
    public static class GetAllStores
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IStoreRepository _storeRepository { get; set; }
            public Handler(IStoreRepository storeRepository)
            {
                _storeRepository = storeRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<IEnumerable<StoreDto>>();

                var results = _storeRepository
                        .GetAllQueryable()
                            .Include(x => x.Manager)
                            .Include(x => x.StoreStatus)
                            .Include(x => x.StoreLocation)
                        .Select(x => new StoreDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            Location = x.StoreLocation.Location,
                            Contact = x.Contact,
                            WorkingHours = x.WorkingHours,
                            ManagerName = x.Manager.Name,
                            ManagerId = x.ManagerId,
                            TotalSales = x.TotalSales,
                            StatusId = x.StoreStatusId,
                            StoreStatusName = x.StoreStatus.Description,
                            LastUpdated = x.ModifiedAt != null ?
                                            x.ModifiedAt.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm:ss") :
                                            x.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss")
                        });

                response.Success = true;
                response.Data = results;

                return new Response(response);
            }
        }


        // Response
        public record Response(ApiResponseModel<IEnumerable<StoreDto>> response);
    }
}

