using Application.Services.FileService;
using Authentication.Services;
using Authorization.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Application
{
    public static class DependencyInjection
    {


        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<FileService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            


            services.AddMediatR(configuration => 
                configuration.RegisterServicesFromAssemblies(assembly));

            services.AddValidatorsFromAssembly(assembly);

            return services;

        }


    }
}
