using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.SqlServer.Context;
using Infra.SqlServer.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.SqlServer.Repositories
{
    public class CategoryRepository : RepositoryBase<CategoryEntity>, ICategoryRepository
    {
        public CategoryRepository(SqlServerContext context) : base(context) { }
    }
}
