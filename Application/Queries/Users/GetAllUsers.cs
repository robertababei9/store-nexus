using Infrastructure.Repositories.Contracts;
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
                var results = _userRepository
                    .GetAllQueryable()
                        .Include(x => x.Role)
                        .Include(x => x.UserDetails)
                    .Select(x => new UsersDto
                    {
                        Id = x.Id,
                        FirstName = x.UserDetails.FirstName,
                        LastName = x.UserDetails.LastName,
                        Email = x.Email,
                        Role = x.Role.Name,
                        RoleId = x.Role.Id,
                        Country = x.UserDetails.Country,
                        City = x.UserDetails.City,
                        Store = "x.Store.Name",
                        StoreId = Guid.NewGuid(),   // "x.Store.Id"
                        PhoneNumber = x.UserDetails.Contact,
                        SignUpDate = x.UserDetails.SignUpDate,

                    }).ToList();

                return new Response(results);
            }
        }



        // Response
        public record Response(IEnumerable<UsersDto> data);
    }
}
