using Domain.Entities;
using Infrastructure.GenericRepository;

namespace Application.Repositories.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        
    }
}
