﻿using Domain.Entities;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repositories.Contracts
{
    public interface IRolesRepository : IGenericRepository<Role>
    {
        Task<IEnumerable<string>> GetRolePermissionsByRoleName(string roleName);
        Task<IEnumerable<string>> GetRolePermissionsByRoleId(Guid roleId);
    }
}
