using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class StoreLocationRepository : GenericRepository<StoreLocation>, IStoreLocationRepository
    {
        public StoreLocationRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }



    }
}
