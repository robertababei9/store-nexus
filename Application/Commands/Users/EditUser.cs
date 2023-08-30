using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;

namespace Application.Commands.Users
{
    public static class EditUser
    {
        // Command
        public record Command(UsersDto userToEdit) : IRequest<bool>;

        // Handler
        public class Handler : IRequestHandler<Command, bool>
        {
            protected IUserRepository _userRepository { get; set; }
            private readonly IUserService _userService;
            public Handler(IUserRepository userRepository, IUserService userService)
            {
                _userRepository = userRepository;
                _userService = userService;
            }

            public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == request.userToEdit.Id, x => x);

                if (user != null)
                {
                    user.FromDto(request.userToEdit);

                    // If the password have a value --> Update password
                    if (!string.IsNullOrEmpty(request.userToEdit.Password))
                    {
                        var userRegisterModel = _userService.GetRegisteredUserModel(new RegisterResource(
                            request.userToEdit.Name,
                            request.userToEdit.Email,
                            request.userToEdit.Password));

                        user.PasswordSalt = userRegisterModel.PasswordSalt;
                        user.PasswordHash = userRegisterModel.PasswordHash;
                    }

                    _userRepository.Update(user);
                    await _userRepository.SaveChangesAsync();

                    return true;

                }

                return false;

            }
        }
    }
}
