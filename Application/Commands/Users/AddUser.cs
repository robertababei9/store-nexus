using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.Users
{
    public static class AddUser
    {
        // Command
        public record Command(UserToAddDto userToAdd) : IRequest<Guid>;

        // Handler
        public class Handler : IRequestHandler<Command, Guid>
        {
            protected IHttpContextAccessor _httpContextAccessor { get; set; }
            protected IUserRepository _userRepository { get; set; }
            protected IUserDetailsRepository _userDetailsRepository { get; set; }

            private readonly IUserService _userService;


            public Handler(
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IUserDetailsRepository userDetailsRepository,
                IUserService userService )
            {
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _userService = userService;
                _userDetailsRepository = userDetailsRepository;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                // check if another email already exist
                bool emailAlreadyExist = (await _userRepository.FirstOrDefaultAsync(x => x.Email == request.userToAdd.Email, x => x)) != null;
                if (emailAlreadyExist)
                {
                    return Guid.Empty;
                }


                var currentLoggedInUserId = _httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value;
                var companyId = await _userRepository.FirstOrDefaultAsync(x => x.Id == Guid.Parse(currentLoggedInUserId), x => x.CompanyId);

                // Adding the user
                User user = new User
                {
                    Email = request.userToAdd.Email,
                    Name = request.userToAdd.FirstName + " " + request.userToAdd.LastName,
                    RoleId = request.userToAdd.RoleId,
                    CompanyId = companyId
                };


                var userWithPasswordModel = _userService.GetRegisteredUserModel(
                    new RegisterResource(
                        request.userToAdd.FirstName + " " + request.userToAdd.LastName,
                        request.userToAdd.Email,
                        request.userToAdd.Password,
                        request.userToAdd.RoleId)
                );
                user.PasswordSalt = userWithPasswordModel.PasswordSalt;
                user.PasswordHash = userWithPasswordModel.PasswordHash;

                await _userRepository.AddAsync(user);

                // Adding user details
                DateTime signUpDate = DateTime.Parse(request.userToAdd.SignUpDate);
                var userDetails = new UserDetails
                {
                    UserId = user.Id,
                    FirstName = request.userToAdd.FirstName,
                    LastName = request.userToAdd.LastName,
                    Contact = request.userToAdd.Contact,
                    Country = request.userToAdd.Country,
                    City = request.userToAdd.City,
                    SignUpDate = DateOnly.FromDateTime(signUpDate),
                };
                await _userDetailsRepository.AddAsync(userDetails);

                // Saving everything to the DB
                await _userRepository.SaveChangesAsync();

                return user.Id;
            }
        }

    }
}
