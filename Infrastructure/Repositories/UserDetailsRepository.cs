using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UserDetailsRepository : GenericRepository<UserDetails>, IUserDetailsRepository
    {
        public UserDetailsRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }
    }
}
