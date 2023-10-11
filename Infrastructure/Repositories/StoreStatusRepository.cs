using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class StoreStatusRepository : GenericRepository<StoreStatus>, IStoreStatusRepository
    {
        public StoreStatusRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }



    }
}
