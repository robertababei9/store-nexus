using Infrastructure.Repositories.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PasswordHashExample.WebAPI.Resources;
using Domain.Common;
using System.Reflection;

namespace Authentication.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IOptions<ApplicationSettings> _appSettings;
        private readonly string _pepper;
        private readonly int _iteration = 3;

        private readonly IUserRepository _userRepository;
        private readonly IRolesRepository _rolesRepository;

        public UserService(IUserRepository userRepository, IRolesRepository rolesRepository, IOptions<ApplicationSettings> appSettings)
        {
            _appSettings = appSettings;
            _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");

            _userRepository = userRepository;
            _rolesRepository = rolesRepository;
        }

        public async Task<LoginResponseResource> Login(LoginResource resource, CancellationToken cancellationToken)
        {
            //var user = await _context.Users
            //    .FirstOrDefaultAsync(x => x.Username == resource.Username, cancellationToken);

            var user = await _userRepository.GetAllQueryable()
                    .Include(x => x.Role)
                        .ThenInclude(y => y.RolePermissions)
                .Where(x => x.Email == resource.Email)
                .FirstOrDefaultAsync();

            //var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == resource.Email, x => x);

            if (user == null)
                throw new Exception("Username or password did not match");

            var passwordHash = PasswordHasher.ComputeHash(resource.Password, user.PasswordSalt, _pepper, _iteration);
            if (user.PasswordHash != passwordHash)
                throw new Exception("Username or password did not match");

            // authentication successful so generate jwt token
            string secretKey = _appSettings.Value.Secret;
            string token = new JwtGenerator().GetToken(secretKey, user);

            bool needsToCreateCompany = 
                user.CompanyId == null && 
                user.Role.Name.ToUpper() == Enums.Roles.Admin.ToString().ToUpper();

            var rolePermissions = (await _rolesRepository.GetRolePermissionsByRoleId(user.Role.Id)).ToList();

            LoginResponseResource result = new LoginResponseResource(token, needsToCreateCompany, rolePermissions);

            return result;


        }

        public async Task<UserResource> Register(RegisterResource resource, CancellationToken cancellationToken)
        {
            var user = GetRegisteredUserModel(resource);
            var userDetails = new UserDetails
            {
                FirstName = resource.FirstName,
                LastName = resource.LastName,
                Contact = null,
                Country = null,
                City = null,
                SignUpDate = DateOnly.FromDateTime(DateTime.Now),
                //UserId = user.Id,
            };
            user.UserDetails = userDetails;

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
                Name = resource.FirstName + " " + resource.LastName,
                Email = resource.Email,
                RoleId = resource.RoleId,
                PasswordSalt = PasswordHasher.GenerateSalt(),
                CompanyId = resource.CompanyId,
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
