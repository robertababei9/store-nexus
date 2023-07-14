using Application.Repositories.Contracts;
using Domain.Entities;
using PasswordHashExample.WebAPI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _pepper;
        private readonly int _iteration = 3;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
        }

        public async Task<UserResource> Login(LoginResource resource, CancellationToken cancellationToken)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(x => x.Username == resource.Username, cancellationToken);

            var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == resource.Email, x => x);

            if (user == null)
                throw new Exception("Username or password did not match.");

            var passwordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Username or password did not match.");

            return new UserResource(user.Id, user.Email);


        }

        public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = resource.Name,
                Email = resource.Email,
                PasswordSalt = PasswordHasher.GenerateSalt()
            };
            user.PasswordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);

            try
            {
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            return new UserResource(user.Id, user.Email);
        }
    }
}
