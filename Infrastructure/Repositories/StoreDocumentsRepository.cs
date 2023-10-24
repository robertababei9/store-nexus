using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class StoreDocumentsRepository : GenericRepository<StoreDocuments>, IStoreDocumentsRepository
    {
        public StoreDocumentsRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }

        // If you have a method that implies the database BUT it's not a generic one
        // this is the place to add it
        // Because if we will change the database the logic of the method will still be here
        // and it will aply the same ... but for that different database
        // It's call decoupling. Keeping the things as decoupled as posibile because of possible future changes
        // Good luck with your app, Robert Ababei. 23-Oct-2023

    }
}
