//using Domain.Entities;
//using Domain.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Service.Factories
//{
//    public static class CategoryModelFactory
//    {
//        public static CategoryModel EntityToModel(this CategoryEntity categoryEntity)
//        {
//            if (categoryEntity == null)
//            {
//                return default;
//            }

//            return new CategoryModel
//            {
//                CategoryId = categoryEntity.CategoryId,
//                Name = categoryEntity.Name,
//                ParentCategory = categoryEntity.ParentModelToEntity(),
//                ChildCategories = categoryEntity.ChildrenModelToEntity()
//            };
//        }

//        private static CategoryModel ParentModelToEntity(this CategoryEntity parentCategory)
//        {
//            if (parentCategory == null)
//                return default;

//            return new CategoryModel
//            {
//                CategoryId = parentCategory.CategoryId,
//                Name = parentCategory.Name,
//            };
//        }

//        private static IEnumerable<CategoryModel> ChildrenModelToEntity(this CategoryEntity childrens)
//        {
//            if (childrens == null || childrens.ChildCategories != null)
//                return default;


//            return childrens.ChildCategories.Select(c =>
//            {
//                return new CategoryModel
//                {
//                    CategoryId = c.CategoryId,
//                    Name = c.Name,
//                    ParentCategory = c.ParentModelToEntity()
//                };
//            });
//        }
//    }
//}
