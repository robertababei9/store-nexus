using Infrastructure.Repositories.Contracts;
using MediatR;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.ExecutionHelper;
using Application.ExecutionHelper.Exceptions;
using Domain.Dto.Settings;
using Microsoft.AspNetCore.Http;
using Domain.Entities.App;

namespace Application.Commands.Settings
{
    public static class SaveMapSettings
    {
        // Query
        public record Command(MapSettingsDto mapSettingsRequest) : IRequest<Response>;

        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            private readonly ILogger<Handler> _logger;
            protected IMapSettingsRepository _mapSettingsRepository { get; set; }

            public Handler(IHttpContextAccessor httpContextAccessor,
                ILogger<Handler> logger,
                IMapSettingsRepository mapSettingsRepository
            )
            {
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
                _mapSettingsRepository = mapSettingsRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    var response = new ApiResponseModel<Guid>();

                    // We will set the map setting for the current user and companyId
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                    var companyId = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId")?.Value;

                    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrEmpty(companyId))
                    {
                        throw new StoreNexusException("Couldn't set the map settings. User or compnay not found for the current user");
                    }

                    var mapSettings = await _mapSettingsRepository.FirstOrDefaultAsync(
                                            x => x.UserId == Guid.Parse(userId) && x.CompanyId == Guid.Parse(companyId),
                                            x => x);

                    // Scenario 1: Settings already exist so we just update them
                    if (mapSettings != null)
                    {
                        mapSettings.Lat = request.mapSettingsRequest.Lat;
                        mapSettings.Lng = request.mapSettingsRequest.Lng;
                        mapSettings.Zoom = request.mapSettingsRequest.Zoom;

                        _mapSettingsRepository.Update(mapSettings);
                        await _mapSettingsRepository.SaveChangesAsync();

                        response.Data = mapSettings.Id;
                    }
                    // Scenario 2: Settings doesn't exist so we create them
                    else
                    {
                        var mapSettingsEntity = new MapSettings
                        {
                            CompanyId = Guid.Parse(companyId),
                            UserId = Guid.Parse(userId),
                            Lat = request.mapSettingsRequest.Lat,
                            Lng = request.mapSettingsRequest.Lng,
                            Zoom = request.mapSettingsRequest.Zoom,
                        };

                        await _mapSettingsRepository.AddAsync(mapSettingsEntity);
                        await _mapSettingsRepository.SaveChangesAsync();

                        response.Data = mapSettingsEntity.Id;
                    }


                    return new Response(response);
                }, _logger);
            }
        }


        // Response
        public record Response(ApiResponseModel<Guid> response);
    }
}
