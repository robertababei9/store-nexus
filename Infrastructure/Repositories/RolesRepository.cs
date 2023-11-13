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

        public async Task<IEnumerable<string>> GetRolePermissionsByRoleId(Guid roleId)
        {
            var rolePermissionsEntity = GetAllQueryable()
                                        .Include(x => x.RolePermissions)
                                    .Where(x => x.Id == roleId)
                                    .Select(x => x.RolePermissions)
                                    .FirstOrDefault();

            var rolePermissions = new List<string>();

            if (rolePermissionsEntity != null)
            {
                var type = rolePermissionsEntity.GetType();
                var properties = type.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof(bool))  // permissions are of type bool
                    {
                        var value = (bool)property.GetValue(rolePermissionsEntity); // get the value
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
