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
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IRolePermissionsRepository, RolePermissionsRepository>();
            services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IStoreLocationRepository, StoreLocationRepository>();
            services.AddTransient<IStoreStatusRepository, StoreStatusRepository>();
            services.AddTransient<IStoreDocumentsRepository, StoreDocumentsRepository>();

            return services;
        }
    }
}
