using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;

namespace Application.Commands.Users
{
    public static class AddUser
    {
        // Command
        public record Command(UsersDto userToAdd) : IRequest<Guid>;

        // Handler
        public class Handler : IRequestHandler<Command, Guid>
        {
            protected IUserRepository _userRepository { get; set; }
            private readonly IUserService _userService;
            public Handler(IUserRepository userRepository, IUserService userService)
            {
                _userRepository = userRepository;
                _userService = userService;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                User user = new User
                {
                    Email = request.userToAdd.Email,
                    Name = request.userToAdd.FullName,
                    RoleId = request.userToAdd.RoleId,
                };


                var userWithPasswordModel = _userService.GetRegisteredUserModel(
                    new RegisterResource(
                        request.userToAdd.FullName,
                        request.userToAdd.Email,
                        request.userToAdd.Password,
                        request.userToAdd.RoleId)
                );
                user.PasswordSalt = userWithPasswordModel.PasswordSalt;
                user.PasswordHash = userWithPasswordModel.PasswordHash;

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                return user.Id;
            }
        }

    }
}
