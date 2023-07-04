using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryHierarchyEntity
    {
        public int Id { get; set; }
        public int ParentCategoryId { get; set; }
        public int ChildCategoryId { get; set; }

        public CategoryEntity ParentCategory { get; set; }
        public CategoryEntity ChildCategory { get; set; }
    }
}
