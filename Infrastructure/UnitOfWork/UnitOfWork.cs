using Application.Repositories;
using Application.Repositories.Contracts;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;




namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly ApplicationContext _context;
        //private readonly ILogger _logger;

        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            //_logger = loggerFactory.CreateLogger("logs");

            User = new UserRepository(_context);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
