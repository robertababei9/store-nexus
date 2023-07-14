using Application.Repositories.Contracts;
using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;


namespace Application.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(ApplicationContext context) : base(context)
        {

        }


    }
}
