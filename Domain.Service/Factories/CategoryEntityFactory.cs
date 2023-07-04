//using Domain.Entities;
//using Domain.Models;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Service.Factories
//{
//    public static class CategoryEntityFactory
//    {
//        public static IEnumerable<CategoryEntity> DictionaryToEntity(
//            IDictionary<string, string> categoryDictionary,
//            IEnumerable<CategoryEntity> categoryEntitiesDb
//            )
//        {
//            CategoryEntity categoryEntity = new CategoryEntity();
//            var categoryCreated = new CategoryEntity();

//            Dictionary<string, CategoryEntity> categoryEntityDictionary = new Dictionary<string, CategoryEntity>();

//            //foreach(var category in categoryEntitiesDb)
//            //{
//            //    if(categoryEntityDictionary.ContainsKey(category.Name))
//            //    {
//            //        if(categoryEntityDictionary[category.Name].ChildCategories != null && categoryDictionary.ContainsKey(category.Name))
//            //        {
//            //            categoryEntityDictionary[category.Name].ChildCategories.Add(new CategoryEntity
//            //            {
//            //                Name = 
//            //            });
//            //        }
//            //    }
//            //}

//            foreach (var key in categoryDictionary.Keys)
//            {
//                //novo filho
//                if(categoryEntityDictionary.ContainsKey(key))
//                {
//                    var child = categoryEntityDictionary[key];
//                    child.ChildCategories.Add(new CategoryEntity
//                    {
//                        Name = categoryDictionary[key],
//                        ParentCategory = categoryEntityDictionary[key]
//                    });
//                }
//                if(!categoryEntityDictionary.ContainsKey(key) && !categoryEntityDictionary.Any(c => c.Value.ChildCategories.Any(z => z.Name == c.Key)))
//                {
//                    categoryEntityDictionary.Add(key, new CategoryEntity
//                    {
//                        Name = key
//                    });
//                }
//            }

//            return categoryEntityDictionary.Values;
//        }

//        //public static CategoryEntity NewParent(
//        //    IDictionary<string, string> categoryDictionary,
//        //    IEnumerable<CategoryEntity> categoryEntitiesDb
//        //    )
//        //{
//        //    Dictionary<string, CategoryEntity> categoryEntityDictionary = new Dictionary<string, CategoryEntity>();

//        //    var filterDictionary = categoryDictionary.Where(c => !categoryEntitiesDb.Contains(c.Key));

//        //    foreach (var key in categoryDictionary.Keys)
//        //    {
//        //        if (categoryEntityDictionary.ContainsKey(key))
//        //        {
//        //            if (categoryEntityDictionary[key].ChildCategories != null)
//        //            {
//        //                var child = new CategoryEntity
//        //                {
//        //                    Name = $"{key} {categoryDictionary[key]}",
//        //                    ParentCategory = key
//        //                };
//        //                var listChildres = new List<CategoryEntity>();
//        //                listChildres.AddRange(categoryEntityDictionary[key].ChildCategories);
//        //                listChildres.Add(child);

//        //                categoryEntityDictionary[key].ChildCategories = listChildres;
//        //            }
//        //        }
//        //    }
//        //}


//        //private static IEnumerable<CategoryEntity> AddChildCategories(
//        //    IEnumerable<CategoryEntity> childCategorie, 
//        //    IDictionary<string, string> categoryDictionary)
//        //{
//        //    var listChildCategory = new List<CategoryEntity>();

//        //    if(childCategorie == null)
//        //    {
//        //        listChildCategory.Add(new CategoryEntity
//        //            {
//        //                Name = categoryDictionary.
//        //        });
//        //    }
//        //}

//        //public static CategoryEntity ModelToEntity(this CategoryModel categoryEntity)
//        //{
//        //    if (categoryEntity == null)
//        //    {
//        //        return default;
//        //    }

//        //    return new CategoryEntity
//        //    {
//        //        CategoryId = categoryEntity.CategoryId,
//        //        Name = categoryEntity.Name,
//        //        ParentCategory = categoryEntity.ParentModelToEntity(),
//        //        ChildCategories = categoryEntity.ChildrenModelToEntity()
//        //    };
//        //}

//        private static CategoryEntity ParentModelToEntity(this CategoryModel parentCategory)
//        {
//            if (parentCategory == null)
//                return default;

//            return new CategoryEntity
//            {
//                CategoryId = parentCategory.CategoryId,
//                Name = parentCategory.Name,
//            };
//        }

//        private static IEnumerable<CategoryEntity> AddChildCategories(this CategoryModel childrens)
//        {
//            if (childrens == null || childrens.ChildCategories != null)
//                return default;


//            return childrens.ChildCategories.Select(c =>
//            {
//                return new CategoryEntity
//                { 
//                    CategoryId = c.CategoryId, 
//                    Name = c.Name, 
//                    ParentCategory = c.ParentModelToEntity() 
//                };
//            });
//        }
//    }
//}
