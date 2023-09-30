using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.AspNetCore.Http;
using Domain.Dto.Users;
using Common.Models;

namespace Application.Commands.Users
{
    public static class ChangePassword
    {
        // Command
        public record Command(ChangePasswordDto changePasswordModel) : IRequest<ApiResponseModel<bool>>;

        // Handler
        public class Handler : IRequestHandler<Command, ApiResponseModel<bool>>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IUserRepository _userRepository { get; set; }
            private readonly IUserService _userService;


            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IUserDetailsRepository userDetailsRepository,
                IUserService userService)
            {
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _userService = userService;
            }

            public async Task<ApiResponseModel<bool>> Handle(Command request, CancellationToken cancellationToken)
            {
                var result = new ApiResponseModel<bool>
                {
                    Success = false,
                    Data = false,
                };

                var model = request.changePasswordModel;
                // check if passwords match
                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    result.Errors.Append("Password don't match");
                    return result;
                }

                var user = await _userRepository.FirstOrDefaultAsync(x => x.Id == model.UserId, x => x);

                if (user == null)
                {
                    return result;
                }

                // check if new password it's different from the old pass
                var newPasswordHash = _userService.GetExistingPasswordHash(new ExistingPasswordResource(
                        model.NewPassword, user.PasswordSalt
                ));

                if (user.PasswordHash == newPasswordHash)
                {
                    result.Errors.Add("The new password must be different from the old password");
                    return result;
                }

                user.PasswordHash = newPasswordHash;

                _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                result.Success = true;
                result.Data = true;
                return result;
            }
        }

    }
}
