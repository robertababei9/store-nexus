using Domain.Dto;
using Domain.Entities;
using Infrastructure.GenericRepository;


namespace Infrastructure.Repositories.Contracts
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<InvoiceFormDto> GetInvoiceForm(Guid invoiceId);
    }
}
