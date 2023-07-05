using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Infra.SqlServer.Context;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Domain.Service.Service
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly SqlServerContext sqlServerContext;
        private readonly ILogger<CategoryManagementService> logger;
        private static int INITIAL_LEVEL = 1;
        
        public CategoryManagementService(
            SqlServerContext sqlServerContext,
            ILogger<CategoryManagementService> logger)
        {
            this.sqlServerContext = sqlServerContext;
            this.logger = logger;
        }

        public async Task Create(Dictionary<string, string> categoryDictionary)
        {
            //categoria ja existe
            //novo filho
            var executionStrategy = sqlServerContext.Database.CreateExecutionStrategy();

            executionStrategy.Execute(() => 
            {
                using var transaction = sqlServerContext.Database.BeginTransaction();

                foreach (var categoryEntry in categoryDictionary)
                {
                    var categoryName = categoryEntry.Key;
                    var children = categoryEntry.Value;

                    var category = sqlServerContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

                    if (category == null)
                    {
                        category = new CategoryEntity { CategoryName = categoryName, Level = INITIAL_LEVEL };
                    }

                    if (!string.IsNullOrEmpty(children))
                    {
                        var newChild = new CategoryEntity
                        {
                            Level = category.NewLevel(),
                            ParentCategoryId = category.CategoryId,
                            CategoryName = children
                        };

                        category.AddNewChild(newChild);
                    }

                    sqlServerContext.Categories.Update(category);
                    sqlServerContext.SaveChanges();
                }

                transaction.Commit();
            });
        }

        public Task<CategoryModel> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        //public async Task<CategoryEntity> GetCategory(int categoryId)
        //{
        //    var options = new JsonSerializerOptions
        //    {
        //        ReferenceHandler = ReferenceHandler.Preserve
        //    };

        //    var category = await sqlServerContext.Categories
        //        .Include(c => c.ChildCategories)
        //        .Include(c => c.ParentCategory)
        //        .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        //    if (category == null)
        //        return default;

        //    //await AddChildAndParentCategories(category, category);

        //    var json = JsonSerializer.Serialize(category, options);
        //    var result = JsonSerializer.Deserialize<CategoryEntity>(json, options);

        //    return result;
        //}

        public Dictionary<string, object> GetCategory(int categoryId)
        {
            var category = sqlServerContext.Categories
                .Include(c => c.ChildCategories)
                .FirstOrDefault(c => c.CategoryId == categoryId);

            var dictionarieCategories = new Dictionary<string, object>();

            if (category == null)
            {
                return null;
            }

            var parent = new Dictionary<string, object>();

            dictionarieCategories.Add(category.CategoryName, parent);

            if (category.ChildCategories != null && category.ChildCategories.Any())
            {
                foreach (var child in category.ChildCategories)
                {
                    AddParent(parent, child);

                    var grandchild = sqlServerContext.Categories
                        .Include(c => c.ParentCategory)
                        .FirstOrDefault(c => c.ParentCategoryId == category.ParentCategoryId);


                    //if (grandchild != null) 
                    //{
                    //    NextParent(parent, grandchild.ParentCategory);
                    //}

                }
            }

            var total = GetCategoryDepth(categoryId);
            Console.WriteLine(total);

            //var categoryModel = new CategoryEntity
            //{
            //    CategoryName = category.CategoryName,
            //    ParentCategory = GetParentCategories(category),
            //    ChildCategories = GetChildCategories(category.ChildCategories)
            //};

            return dictionarieCategories;
        }

        public void AddParent(Dictionary<string, object> parent, CategoryEntity child)
        {
            var newParent = new Dictionary<string, object>();

            parent.Add(child.CategoryName, newParent);

            var category = sqlServerContext.Categories
                .Include(c => c.ChildCategories)
                .FirstOrDefault(c => c.ParentCategoryId == child.CategoryId);

            if(category != null) 
            {
                AddParent(newParent, category);
            }
        }

        private void NextParent(Dictionary<string, object> parent, CategoryEntity parentCategory)
        {
            var newParent = new Dictionary<string, object>();

            if (parentCategory == null)
                return;

            var parentEntity = sqlServerContext.Categories
                        .Include(c => c.ParentCategory)
                        .FirstOrDefault(c => c.ParentCategoryId == parentCategory.ParentCategoryId);

            while(parentEntity != null) 
            {
                newParent.Add(parentEntity.CategoryName, new Dictionary<string, object>());

                foreach (var child in parentEntity.ChildCategories)
                {
                    var grandChild = sqlServerContext.Categories
                        .Include(c => c.ParentCategory)
                        .FirstOrDefault(c => c.CategoryId == child.CategoryId);

                    var parentCategorie = new Dictionary<string, object>();

                    if (grandChild != null)
                    {
                        NextParent(newParent, grandChild);
                    }

                    newParent[child.CategoryName] = parentCategorie;

                    parentEntity = sqlServerContext.Categories
                        .Include(c => c.ParentCategory)
                        .FirstOrDefault(c => c.CategoryId == child.CategoryId);
                }
            }

        }

        private CategoryEntity GetParentCategories(CategoryEntity category)
        {
            var parentCategories = new CategoryEntity();

            if (category.ParentCategory != null)
            {
                var categoryEntity = new CategoryEntity
                {
                    CategoryName = category.ParentCategory.CategoryName,
                    ParentCategory = GetParentCategories(category.ParentCategory),
                    ChildCategories = GetChildCategories(category.ParentCategory.ChildCategories)
                };
            }

            return parentCategories;
        }

        private List<CategoryEntity> GetChildCategories(List<CategoryEntity> childCategories)
        {
            var childCategoryModels = new List<CategoryEntity>();

            if (childCategories != null)
            {
                foreach (var childCategory in childCategories)
                {
                    var childCategoryModel = new CategoryEntity
                    {
                        CategoryName = childCategory.CategoryName,
                        ParentCategory = GetParentCategories(childCategory),
                        ChildCategories = GetChildCategories(childCategory.ChildCategories)
                    };

                    childCategoryModels.Add(childCategoryModel);
                }
            }

            return childCategoryModels;
        }


        private Dictionary<string, object> GenerateHierarchy(CategoryEntity category)
        {
            var hierarchy = new Dictionary<string, object>();
            var childCategories = sqlServerContext.Categories
                .Where(ch => ch.ParentCategoryId == category.CategoryId)
                .ToList();

            foreach (var childCategory in childCategories)
            {
                var childHierarchy = GenerateHierarchy(childCategory);
                hierarchy[childCategory.CategoryName] = childHierarchy;
            }

            return hierarchy;
        }

        private int GetCategoryDepth(int categoryId)
        {
            var category = sqlServerContext.Categories.Find(categoryId);
                
            if (category == null)
            {
                return 0;
            }

            int depth = 0;
            var parentCategoryId = category.ParentCategoryId;

            while (parentCategoryId != null)
            {
                depth++;
                category = sqlServerContext.Categories.Find(parentCategoryId.Value);
                parentCategoryId = category.ParentCategoryId;
            }

            return depth;
        }
    }
}