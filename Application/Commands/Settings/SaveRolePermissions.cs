using Infrastructure.Repositories.Contracts;
using MediatR;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.ExecutionHelper;
using Application.ExecutionHelper.Exceptions;

namespace Application.Commands.Settings
{
    public static class SaveRolePermissions
    {
        // Query
        public record Command(Guid roleId, Dictionary<string, bool> rolePermissions) : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Command, Response>
        {
            private readonly ILogger<Handler> _logger;
            protected IRolesRepository _rolesRepository { get; set; }
            protected IRolePermissionsRepository _rolesPermissionsRepository { get; set; }

            public Handler(ILogger<Handler> logger,
                IRolesRepository rolesRepository,
                IRolePermissionsRepository rolesPermissionsRepository)
            {
                _logger = logger;
                _rolesRepository = rolesRepository;
                _rolesPermissionsRepository = rolesPermissionsRepository;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                return await ExecuteFunc.TryExecute<Task<Response>>(async () =>
                {
                    var response = new ApiResponseModel<bool>();

                    // get role permissions
                    var rolesEntity = await _rolesRepository
                            .GetAllQueryable()
                                .Include(x => x.RolePermissions)
                            .Where(x => x.Id == request.roleId)
                            .FirstOrDefaultAsync();

                    if (rolesEntity == null || rolesEntity.RolePermissions == null)
                    {
                        throw new StoreNexusException("SaveRolePermissions -> Role or RolePermissions doesn't exist");
                    }

                    bool permissionUpdated = rolesEntity.RolePermissions.MapPermissions(request.rolePermissions);
                    if (permissionUpdated)
                    {
                        _rolesPermissionsRepository.Update(rolesEntity.RolePermissions);
                        await _rolesRepository.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogInformation("SaveRolePermissions -> Could not update the role permissions. One or more properties doesn't exist");
                        response.Success = false;
                        response.Errors.Add("Something went wrong updating the permissions. Please contact support");
                        return new Response(response);
                    }


                    return new Response(response);
                }, _logger);
            }
        }


        // Response
        public record Response(ApiResponseModel<bool> response);
    }
}
