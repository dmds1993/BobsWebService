using Domain.Entities;
using Domain.Interfaces.Repository;
using Infra.SqlServer.Context;
using Infra.SqlServer.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.SqlServer.Repositories
{
    public class CategoryHierarchiesRepository : RepositoryBase<CategoryHierarchyEntity>, ICategoryHierarchiesRepository
    {
        public CategoryHierarchiesRepository(SqlServerContext context) : base(context) { }

    }
}
