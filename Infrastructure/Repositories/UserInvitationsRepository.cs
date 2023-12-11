using Domain.Entities;
using Infrastructure.GenericRepository;
using Infrastructure.Persistence;
using Infrastructure.Repositories.Contracts;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class UserInvitationsRepository : GenericRepository<UserInvitations>, IUserInvitationsRepository
    {
        public UserInvitationsRepository(IUnitOfWork uow, ApplicationContext context) : base(uow, context)
        {

        }
    }
}
