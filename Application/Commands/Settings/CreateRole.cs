
using Infrastructure.Repositories.Contracts;
using MediatR;
using Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.ExecutionHelper;
using Application.ExecutionHelper.Exceptions;
using Domain.Entities;

namespace Application.Commands.Settings
{
    public static class CreateRole
    {
        // Query
        public record Command(string roleName) : IRequest<Response>;


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

                    // check if a role with this name already exist
                    var entity = await _rolesRepository
                            .GetAllQueryable()
                            .Where(x => x.Name == request.roleName)
                            .FirstOrDefaultAsync();

                    if (entity != null)
                    {
                        // duplicate roleName
                        response.Success = false;
                        response.Errors.Add("A role with this name already exist");
                        return new Response(response);
                    }

                    var newRolePermissionsEntity = new RolePermissions
                    {
                        Name = request.roleName,
                    };

                    await _rolesPermissionsRepository.AddAsync(newRolePermissionsEntity);
                    await _rolesPermissionsRepository.SaveChangesAsync();

                    var newRoleEntity = new Role
                    {
                        Name = request.roleName,
                        Description = "",
                        RolePermissionsId = newRolePermissionsEntity.Id,
                        CreatedAt = DateTime.Now,
                    };

                    await _rolesRepository.AddAsync(newRoleEntity);
                    await _rolesRepository.SaveChangesAsync();


                    return new Response(response);
                }, _logger);
            }
        }


        // Response
        public record Response(ApiResponseModel<bool> response);
    }
}

