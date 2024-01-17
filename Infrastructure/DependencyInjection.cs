using Infrastructure.Email;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;




namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<Infrastructure.UnitOfWork.IUnitOfWork, Infrastructure.UnitOfWork.UnitOfWork>();

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserDetailsRepository, UserDetailsRepository>();
            services.AddTransient<IUserInvitationsRepository, UserInvitationsRepository>();
            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IRolePermissionsRepository, RolePermissionsRepository>();
            services.AddTransient<IInvoiceRepository, InvoiceRepository>();
            services.AddTransient<IInvoiceItemRepository, InvoiceItemRepository>();
            services.AddTransient<ICompanyRepository, CompanyRepository>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IStoreLocationRepository, StoreLocationRepository>();
            services.AddTransient<IStoreStatusRepository, StoreStatusRepository>();
            services.AddTransient<IStoreDocumentsRepository, StoreDocumentsRepository>();
            services.AddTransient<IMapSettingsRepository, MapSettingsRepository>();
            #endregion

            services.AddTransient<IMailService, MailService>();

            return services;
        }
    }
}
