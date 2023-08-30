using Domain.Entities;
using Infrastructure.GenericRepository;

namespace Infrastructure.Repositories.Contracts
{
    public interface IInvoiceItemRepository : IGenericRepository<InvoiceItem>
    {
    }
}
