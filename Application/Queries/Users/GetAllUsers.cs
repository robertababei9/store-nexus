using Application.Repositories.Contracts;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users
{
    public static class GetAllUsers
    {
        // Query
        public record Query() : IRequest<Response>;


        // Handler
        public class Handler : IRequestHandler<Query, Response>
        {
            protected IUserRepository _userRepository { get; set; }
            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var result = _userRepository
                    .GetAllQueryable().Include(x => x.Role)
                    .ToList()
                    .Select(x => new UsersDto(x))
                    .ToList();

                return new Response(result);
            }
        }



        // Response
        public record Response(IEnumerable<UsersDto> data);
    }
}
