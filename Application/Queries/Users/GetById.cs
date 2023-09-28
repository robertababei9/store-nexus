using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.Users
{
    public static class GetById
    {
        // Query
        public record Query(Guid id) : IRequest<Response>;

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
                var userData = await _userRepository
                    .GetAllQueryable()
                        .Include(x => x.Role)
                        .Include(x => x.UserDetails)
                    .Where(x => x.Id == request.id)
                    .Select(x => new UsersDto
                    {
                        Id = x.Id,
                        FirstName = x.UserDetails.FirstName,
                        LastName = x.UserDetails.LastName,
                        Email = x.Email,
                        Role = x.Role.Description,
                        RoleId = x.Role.Id,
                        Store = "x.Store.Name",
                        StoreId = Guid.NewGuid(),   // "x.Store.Id"
                        PhoneNumber = x.UserDetails.Contact,
                        Country = x.UserDetails.Country,
                        City = x.UserDetails.City,
                        SignUpDate = x.UserDetails.SignUpDate,
                    })
                    .FirstOrDefaultAsync();

                return new Response(userData);
            }
        }

        // Response
        public record Response(UsersDto userData);
    }
}
