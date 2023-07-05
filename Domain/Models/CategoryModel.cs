using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public CategoryModel ParentCategory { get; set; }
        public IEnumerable<CategoryModel> ChildCategories { get; set; }
    }
}
