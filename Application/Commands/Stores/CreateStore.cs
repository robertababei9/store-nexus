using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.AspNetCore.Http;
using Domain.Dto.Stores;
using Common.Models;

namespace Application.Commands.Users
{
    public static class CreateStore
    {
        // Command
        public record Command(CreateStoreDto storeToCreate) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            protected IStoreRepository _storeRepository { get; set; }
            protected IStoreLocationRepository _storeLocationRepository { get; set; }

            public Handler(
                IStoreRepository storeRepository, IStoreLocationRepository storeLocationRepository)
            {
                _storeRepository = storeRepository;
                _storeLocationRepository = storeLocationRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<Guid>();

                // check if a store with the same name already exist
                var store = await _storeRepository.FirstOrDefaultAsync(x => x.Name == request.storeToCreate.Name, x => x);
                if (store != null)
                {
                    response.Success = false;
                    response.Errors.Add("A store with that name already exist");
                    return new Response(response);
                }

                var storeLocationEntity = new StoreLocation
                {
                    Country = request.storeToCreate.Country,
                    CountryCode = request.storeToCreate.CountryCode,
                    LatLng = request.storeToCreate.LatLng,
                    Location = request.storeToCreate.Location
                };

                await _storeLocationRepository.AddAsync(storeLocationEntity);

                var storeEntity = new Store
                {
                    Name = request.storeToCreate.Name,
                    Description = request.storeToCreate.Description,
                    Contact = request.storeToCreate.Contact,
                    StoreLocationId = storeLocationEntity.Id,
                    WorkingHours = request.storeToCreate.WorkingHours,
                    ManagerId = request.storeToCreate.ManagerId,
                    StoreStatusId = request.storeToCreate.StoreStatusId
                };

                await _storeRepository.AddAsync(storeEntity);
                await _storeRepository.SaveChangesAsync();

                response.Data = storeEntity.Id;

                return new Response(response);
            }
        }

        // Response
        public record Response(ApiResponseModel<Guid> response);
    }
}
