using Infrastructure.Repositories.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PasswordHashExample.WebAPI.Resources;
using Domain.Common;

namespace Authentication.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IOptions<ApplicationSettings> _appSettings;
        private readonly IUserRepository _userRepository;
        private readonly string _pepper;
        private readonly int _iteration = 3;

        public UserService(IUserRepository userRepository, IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings;
            _userRepository = userRepository;
            _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
        }

        public async Task<LoginResponseResource> Login(LoginResource resource, CancellationToken cancellationToken)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(x => x.Username == resource.Username, cancellationToken);

            var user = await _userRepository.GetAllQueryable()
                    .Include(x => x.Role)
                .Where(x => x.Email == resource.Email)
                .FirstOrDefaultAsync();

            //var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == resource.Email, x => x);

            if (user == null)
                throw new Exception("Username or password did not match 1.");

            var passwordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Username or password did not match 2. " + user.PasswordHash + " --- " + passwordHash);

            // authentication successful so generate jwt token
            string secretKey = _appSettings.Value.Secret;
            string token = new JwtGenerator().GetToken(secretKey, user);

            bool needsToCreateCompany = 
                user.CompanyId == null && 
                user.Role.Name.ToUpper() == Enums.Roles.Admin.ToString().ToUpper();

            LoginResponseResource result = new LoginResponseResource(token, needsToCreateCompany);

            return result;


        }

        public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken)
        {
            var user = GetRegisteredUserModel(resource);

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

        public User GetRegisteredUserModel(RegisterResource resource)
        {
            var user = new User
            {
                Name = resource.Name,
                Email = resource.Email,
                RoleId = resource.RoleId,
                PasswordSalt = PasswordHasher.GenerateSalt()
            };

            user.PasswordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);

            return user;
        }

        public string GetExistingPasswordHash(ExistingPasswordResource resource)
        {
            return PasswordHasher.ComputeHash(resource.Password, resource.PasswordSalt, _pepper, _iteration);
        }
    }
}
