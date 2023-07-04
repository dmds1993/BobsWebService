using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryHierarchyEntity
    {
        [Column("CategoryId")]
        public int Id { get; set; }
        [Column("ParentCategoryId")]
        public int ParentCategoryId { get; set; }
        [Column("CategoryId")]
        public int ChildCategoryId { get; set; }

        public CategoryEntity ParentCategory { get; set; }
        public CategoryEntity ChildCategory { get; set; }
    }
}
