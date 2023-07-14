﻿using Application.Repositories;
using Application.Repositories.Contracts;
using Microsoft.Extensions.DependencyInjection;




namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {

            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}
