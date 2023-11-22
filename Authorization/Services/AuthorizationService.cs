using Infrastructure.Repositories.Contracts;


namespace Authorization.Services
{
    public sealed class AuthorizationService : IAuthorizationService
    {
        private readonly IRolesRepository _rolesRepository;


        public AuthorizationService(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }


        public async Task<bool> HasPermission(string role, string[] permissions)
        {
            var rolePermissions = (await _rolesRepository.GetRolePermissionsByRoleName(role)).ToList();

            return permissions.All(x => rolePermissions.Contains(x));

        }
    }

}
