using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class RolePermissionsRepository : GenericRepository<RolePermissions>, IRolePermissionsRepository
    {

        public RolePermissionsRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }
    }
}
