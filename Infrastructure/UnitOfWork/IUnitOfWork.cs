using Infrastructure.Persistence;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {

        ApplicationContext GetContext();
        void Dispose();
    }
}
