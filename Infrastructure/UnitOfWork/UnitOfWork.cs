using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;




namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly ApplicationContext _context;
        //private readonly ILogger _logger;


        public UnitOfWork(ApplicationContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            //_logger = loggerFactory.CreateLogger("logs");

        }
        
        public ApplicationContext GetContext()
        {
            return _context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
