using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Domain.Dto.Stores;

namespace Application.Queries.Stores
{
    public static class GetById
    {
        // Query
        public record Query(Guid id) : IRequest<Response>;

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
                var response = new ApiResponseModel<CreateStoreDto>();

                var userData = _storeRepository
                    .GetAllQueryable()
                        .Include(x => x.StoreLocation)
                    .Where(x => x.Id == request.id)
                    .Select(x => new CreateStoreDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Contact = x.Contact,
                        WorkingHours = x.WorkingHours,
                        ManagerId = x.ManagerId,
                        StoreStatusId = x.StoreStatusId,
                        Country = x.StoreLocation.Country,
                        CountryCode = x.StoreLocation.CountryCode,
                        Location = x.StoreLocation.Location,
                        LatLng = x.StoreLocation.LatLng,
                    })
                    .FirstOrDefault();

                if (userData == null)
                {
                    response.Success = false;
                    response.Errors.Add("Something went wrong while getting the store");
                    return new Response(response);
                }

                response.Data = userData;

                return new Response(response);
            }
        }

        // Response
        public record Response(ApiResponseModel<CreateStoreDto> response);
    }
}
