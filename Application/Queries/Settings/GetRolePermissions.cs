using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Common.Models;

namespace Application.Queries.Stores
{
    public static class GetRolePermissions
    {
        // Query
        public record Query(Guid roleId) : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IRolesRepository _rolesRepository { get; set; }

            public Handler(IRolesRepository rolesRepository)
            {
                _rolesRepository = rolesRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
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
            }
        }


        // Response
        public record Response(ApiResponseModel<List<string>> response);
    }
}

