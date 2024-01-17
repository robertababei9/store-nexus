using Infrastructure.Repositories.Contracts;
using MediatR;
using Common.Models;
using Application.ExecutionHelper;
using Microsoft.Extensions.Logging;
using Domain.Dto.Settings;
using Microsoft.AspNetCore.Http;
using Application.ExecutionHelper.Exceptions;

namespace Application.Queries.Settings
{
    public static class GetMapSettings
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            private readonly ILogger<Handler> _logger;
            protected IMapSettingsRepository _mapSettingsRepository { get; set; }

            public Handler(IHttpContextAccessor httpContextAccessor,
                ILogger<Handler> logger,
                IMapSettingsRepository mapSettingsRepository)
            {
                _httpContextAccessor = httpContextAccessor;
                _logger = logger;
                _mapSettingsRepository = mapSettingsRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    var response = new ApiResponseModel<MapSettingsDto>();

                    // We will get the map setting for the current user and companyId
                    var userId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                    var companyId = _httpContextAccessor.HttpContext.User.FindFirst("CompanyId")?.Value;

                    if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrEmpty(companyId))
                    {
                        throw new StoreNexusException("User or compnay not found for the current user");
                    }

                    var mapSettingsDto = await _mapSettingsRepository
                            .FirstOrDefaultAsync(x => x.UserId == Guid.Parse(userId) && x.CompanyId == Guid.Parse(companyId),
                                                x => new MapSettingsDto
                                                {
                                                    Lat = x.Lat,
                                                    Lng = x.Lng,
                                                    Zoom = x.Zoom
                                                });

                    response.Data = mapSettingsDto;

                    return new Response(response);

                }, _logger);
            }
        }


        // Response
        public record Response(ApiResponseModel<MapSettingsDto> response);
    }
}

