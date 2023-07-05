using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public CategoryEntity ParentCategory { get; set; }
        public List<CategoryEntity> ChildCategories { get; set; }
        public void AddNewChild(CategoryEntity category)
        {
            if(ChildCategories == null)
                ChildCategories = new List<CategoryEntity>();

            ChildCategories.Add(category);
        }
    }
}
