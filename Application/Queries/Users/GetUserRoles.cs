using Infrastructure.Repositories.Contracts;
using MediatR;
using Domain.Entities;

namespace Application.Queries.Users
{
    public static class GetUserRoles
    {
        // Query
        public record Query() : IRequest<Response>;

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
                var results = _rolesRepository.GetAllQueryable().ToList();

                return new Response(results);
            }
        }


        // Response
        public record Response(IEnumerable<Role> data);
    }
}
