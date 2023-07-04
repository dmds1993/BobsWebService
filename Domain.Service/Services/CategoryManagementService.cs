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

namespace Domain.Service.Service
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly SqlServerContext sqlServerContext;
        private readonly ILogger<CategoryManagementService> logger;

        public CategoryManagementService(
            SqlServerContext sqlServerContext,
            ILogger<CategoryManagementService> logger)
        {
            this.sqlServerContext = sqlServerContext;
            this.logger = logger;
        }

        public async Task Create(Dictionary<string, string> categoryDictionary)
        {
            foreach (var categoryEntry in categoryDictionary)
            {
                var categoryName = categoryEntry.Key;
                var parentCategoryName = categoryEntry.Value;

                var category = sqlServerContext.Categories.FirstOrDefault(c => c.Name == categoryName);

                if (category == null)
                {
                    category = new CategoryEntity { Name = categoryName };
                    sqlServerContext.Add(category);
                }

                if (!string.IsNullOrEmpty(parentCategoryName))
                {
                    var parentCategory = sqlServerContext.Categories.FirstOrDefault(c => c.Name == parentCategoryName);

                    if (parentCategory == null)
                    {
                        parentCategory = new CategoryEntity { Name = parentCategoryName };
                        sqlServerContext.Categories.Add(parentCategory);
                    }

                    var categoryHierarchy = new CategoryHierarchyEntity
                    {
                        ParentCategoryId = parentCategory.Id,
                        ChildCategoryId = parentCategory.Id
                    };

                    sqlServerContext.CategoryHierarchies.Add(categoryHierarchy);
                }
            }

            sqlServerContext.SaveChanges();
        }

        public Task<CategoryModel> GetCategoryByIdAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> GetCategoryHierarchy()
        {
            var categoryHierarchy = new Dictionary<string, object>();
            var rootCategories = sqlServerContext.Categories
                .Where(c => !sqlServerContext.CategoryHierarchies.Any(ch => ch.ChildCategoryId == c.Id))
                .ToList();

            foreach (var rootCategory in rootCategories)
            {
                var hierarchy = GenerateHierarchy(rootCategory);
                categoryHierarchy[rootCategory.Name] = hierarchy;
            }

            return categoryHierarchy;
        }

        public Task<IEnumerable<CategoryModel>> GetList()
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, object> GenerateHierarchy(CategoryEntity category)
        {
            var hierarchy = new Dictionary<string, object>();
            var childCategories = sqlServerContext.CategoryHierarchies
                .Where(ch => ch.ParentCategoryId == category.Id)
                .Select(ch => ch.ChildCategory)
                .ToList();

            foreach (var childCategory in childCategories)
            {
                var childHierarchy = GenerateHierarchy(childCategory);
                hierarchy[childCategory.Name] = childHierarchy;
            }

            return hierarchy;
        }
    }
}