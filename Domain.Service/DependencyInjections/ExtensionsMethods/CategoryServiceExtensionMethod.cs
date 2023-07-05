using Domain.Interfaces.Service;
using Domain.Service.Service;
using Infra.SqlServer.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Service.DependencyInjections.ExtensionsMethods
{
    public static class CategoryServiceExtensionMethod
    {
        public static IServiceCollection AddCategoryService(
            this IServiceCollection services)
        {
            return services
                .AddScoped<ICategoryManagementService, CategoryManagementService>();
        }
    }
}
