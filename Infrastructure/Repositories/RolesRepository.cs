using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Repositories
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {

        public RolesRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }

        public async Task<IEnumerable<string>> GetRolePermissionsByRoleName(string roleName)
        {
            var rolePermissionsEntity = await GetAllQueryable()
                                        .Include(x => x.RolePermissions)
                                    .Where(x => x.Name == roleName)
                                    .Select(x => x.RolePermissions)
                                    .FirstOrDefaultAsync();


            var rolePermissions = ExtractPermissionsFromEntity(rolePermissionsEntity);

            return rolePermissions;
        }

        public async Task<IEnumerable<string>> GetRolePermissionsByRoleId(Guid roleId)
        {
            var rolePermissionsEntity = await GetAllQueryable()
                                        .Include(x => x.RolePermissions)
                                    .Where(x => x.Id == roleId)
                                    .Select(x => x.RolePermissions)
                                    .FirstOrDefaultAsync();

            var rolePermissions = ExtractPermissionsFromEntity(rolePermissionsEntity);

            return rolePermissions;
        }




        private IEnumerable<string> ExtractPermissionsFromEntity(RolePermissions entity)
        {
            var rolePermissions = new List<string>();

            if (entity != null)
            {
                var type = entity.GetType();
                var properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(bool))  // permissions are of type bool
                    {
                        var value = (bool)property.GetValue(entity); // get the value
                        if (value)
                        {
                            rolePermissions.Add(property.Name);
                        }
                    }
                }
            }

            return rolePermissions;
        }
    }
}
