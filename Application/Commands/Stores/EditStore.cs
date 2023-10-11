using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.EntityFrameworkCore;
using Domain.Dto.Stores;
using Common.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Stores
{
    public static class EditStore
    {
        // Command
        public record Command(CreateStoreDto storeToEdit) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            protected IStoreRepository _storeRepository { get; set; }

            public Handler(
                IStoreRepository storeRepository)
            {
                _storeRepository = storeRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new ApiResponseModel<bool>();

                // check if a store already have this name
                var storeWithSameName = await _storeRepository.FirstOrDefaultAsync(
                    x => x.Name == request.storeToEdit.Name && x.Id != request.storeToEdit.Id, 
                    x => x
                );
                if (storeWithSameName != null)
                {
                    response.Success = false;
                    response.Data = false;
                    response.Errors.Add("A store with that name already exist");
                    return new Response(response);
                }

                var store = _storeRepository
                    .GetAllQueryable()
                        .Include(x => x.StoreLocation)
                    .Where(x => x.Id == request.storeToEdit.Id)
                    .FirstOrDefault();

                if (store == null)
                {
                    response.Data = false;
                    response.Errors.Add("Something went wrong. Couldn't find the store. Please contact support");
                    return new Response(response);
                }

                store.Name = request.storeToEdit.Name;
                store.Description = request.storeToEdit.Description;
                store.ManagerId = request.storeToEdit.ManagerId;
                store.Contact = request.storeToEdit.Contact;
                store.StoreStatusId = request.storeToEdit.StoreStatusId;
                store.WorkingHours = request.storeToEdit.WorkingHours;

                store.StoreLocation.Country = request.storeToEdit.Country;
                store.StoreLocation.CountryCode = request.storeToEdit.CountryCode;
                store.StoreLocation.Location = request.storeToEdit.Location;
                store.StoreLocation.LatLng = request.storeToEdit.LatLng;
                

                _storeRepository.Update(store);
                await _storeRepository.SaveChangesAsync();

                return new Response(response);

            }
        }

        // Response
        public record Response(ApiResponseModel<bool> response);
    }
}
