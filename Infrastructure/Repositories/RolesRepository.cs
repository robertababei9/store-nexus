using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class RolesRepository : GenericRepository<Role>, IRolesRepository
    {

        public RolesRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }
    }
}
