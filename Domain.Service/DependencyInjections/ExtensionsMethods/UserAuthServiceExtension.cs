using Domain.Interfaces.Service;
using Domain.Service.Service;
using Domain.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.DependencyInjections.ExtensionsMethods
{
    public static class UserAuthServiceExtension
    {
        public static IServiceCollection AddUserAuth(
            this IServiceCollection services)
        {
            return services
                .AddScoped<IUserAuthService, UserAuthService>();
        }
    }
}
