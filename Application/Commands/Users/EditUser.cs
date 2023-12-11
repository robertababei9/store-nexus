using Infrastructure.Repositories.Contracts;
using Authentication.Services;
using Domain.Dto;
using Domain.Entities;
using MediatR;
using PasswordHashExample.WebAPI.Resources;
using Microsoft.EntityFrameworkCore;

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
                var user = _userRepository
                    .GetAllQueryable()
                        .Include(x => x.UserDetails)
                    .Where(x => x.Id == request.userToEdit.Id)
                    .FirstOrDefault();

                if (user != null)
                {
                    // It could be encapsulated to something like 'from dto -> to entity'
                    // but I would like to keep all the logic here in case the request model changes

                    user.Email = request.userToEdit.Email;
                    user.Name = request.userToEdit.FirstName + " " + request.userToEdit.LastName;
                    user.RoleId = request.userToEdit.RoleId;

                    user.UserDetails.FirstName = request.userToEdit.FirstName;
                    user.UserDetails.LastName = request.userToEdit.LastName;
                    user.UserDetails.Contact = request.userToEdit.PhoneNumber;
                    user.UserDetails.Country = request.userToEdit.Country;
                    user.UserDetails.City = request.userToEdit.City;
                    user.UserDetails.SignUpDate = request.userToEdit.SignUpDate;

                    // If the password have a value --> Update password
                    if (!string.IsNullOrEmpty(request.userToEdit.Password))
                    {
                        var userRegisterModel = _userService.GetRegisteredUserModel(new RegisterResource(
                            request.userToEdit.FirstName,
                            request.userToEdit.LastName,
                            request.userToEdit.Email,
                            request.userToEdit.Password,
                            request.userToEdit.RoleId,
                            user.CompanyId));

                        user.PasswordSalt = userRegisterModel.PasswordSalt;
                        user.PasswordHash = userRegisterModel.PasswordHash;
                    }

                    _userRepository.Update(user);   // if the entity it's being tracked -> no need to call .Update
                    await _userRepository.SaveChangesAsync();

                    return true;

                }

                return false;

            }
        }
    }
}
