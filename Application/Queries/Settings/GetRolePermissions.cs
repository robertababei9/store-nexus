using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Common.Models;
using Application.ExecutionHelper;
using Microsoft.Extensions.Logging;

namespace Application.Queries.Settings
{
    public static class GetRolePermissions
    {
        // Query
        public record Query(Guid roleId) : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly ILogger<Handler> _logger;
            protected IRolesRepository _rolesRepository { get; set; }

            public Handler(ILogger<Handler> logger,
                IRolesRepository rolesRepository)
            {
                _logger = logger;
                _rolesRepository = rolesRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    var response = new ApiResponseModel<List<string>>();

                    try
                    {
                        var rolePermissions = (await _rolesRepository.GetRolePermissionsByRoleId(request.roleId)).ToList();
                        response.Data = rolePermissions;
                    }
                    catch (Exception e)
                    {
                        response.Success = false;
                        response.Errors.Add(e.Message);
                    }


                    return new Response(response);

                }, _logger);
            }
        }


        // Response
        public record Response(ApiResponseModel<List<string>> response);
    }
}

