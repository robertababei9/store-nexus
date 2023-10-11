using Infrastructure.Repositories.Contracts;
using Domain.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Common.Models;

namespace Application.Queries.Users
{
    public static class GetAllManagers
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
                var response = new ApiResponseModel<IEnumerable<SelectOptionModel<Guid>>>();

                var results = _userRepository
                    .GetAllQueryable()
                        .Include(x => x.Role)
                    .Where(x => x.Role.Name.ToUpper() == "MANAGER")
                    .Select(x => new SelectOptionModel<Guid>
                    {
                        label = x.Name,
                        value = x.Id
                    });

                response.Data = results;

                return new Response(response);
            }
        }



        // Response
        public record Response(
            ApiResponseModel<
                IEnumerable<SelectOptionModel<Guid>>
            > response
        );
    }
}
