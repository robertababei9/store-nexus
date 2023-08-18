using Application.Repositories.Contracts;
using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;


namespace Application.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }


    }
}
