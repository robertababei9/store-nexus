using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;




namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();

            return services;
        }
    }
}
