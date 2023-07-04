using Domain.Interfaces.Repository;
using Infra.SqlServer.Context;
using Infra.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.SqlServer.DepedencyInjections.ExtensionMethods
{
    public static class SqlServerExtensions
    {
        public static IServiceCollection AddCategoryRepository(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            return services
                .AddDbContext<SqlServerContext>(options =>
                options.UseSqlServer(connectionString))
                .AddScoped<ICategoryRepository, CategoryRepository>()
                .AddScoped<ICategoryHierarchiesRepository, CategoryHierarchiesRepository>();
        }
    }
}
