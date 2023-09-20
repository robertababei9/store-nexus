using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;

namespace Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }



    }
}
