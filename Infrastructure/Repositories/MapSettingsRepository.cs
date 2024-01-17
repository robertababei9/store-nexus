using Domain.Entities.App;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class MapSettingsRepository : GenericRepository<MapSettings>, IMapSettingsRepository
    {

        public MapSettingsRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }
    }
}
