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
using System.Runtime.InteropServices;

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

        public async Task CreateCategory(Dictionary<string, string> categoryDictionary)
        {
            var executionStrategy = sqlServerContext.Database.CreateExecutionStrategy();

            executionStrategy.Execute(()=> 
            {
                using var transaction = sqlServerContext.Database.BeginTransaction();

                Create(categoryDictionary);
                
                transaction.Commit();
            });
        }

        public void Create(Dictionary<string, string> categoryDictionary)
        {
            foreach (var categoryEntry in categoryDictionary)
            {
                var categoryName = categoryEntry.Key;
                var child = categoryEntry.Value;

                var category = sqlServerContext.Categories.FirstOrDefault(c => c.CategoryName == categoryName);

                if (category == null)
                {
                    category = new CategoryEntity { CategoryName = categoryName, Level = INITIAL_LEVEL };
                }

                if (!string.IsNullOrEmpty(child))
                {
                    var newChild = new CategoryEntity
                    {
                        Level = category.NewLevel(),
                        ParentCategoryId = category.CategoryId,
                        CategoryName = child
                    };

                    category.AddNewChild(newChild);
                }

                sqlServerContext.Categories.Update(category);
                sqlServerContext.SaveChanges();
            }
        }

        public Dictionary<string, object> GetCategory(string categoryName)
        {
            var category = sqlServerContext.Categories
                .Include(c => c.ChildCategories)
                .FirstOrDefault(c => c.CategoryName == categoryName);


            if (category == null)
            {
                return default;
            }

            var dictionarieCategories = new Dictionary<string, object>();

            return CreateDictionaryCategory(dictionarieCategories, category);
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

        private Dictionary<string, object> CreateDictionaryCategory(
            Dictionary<string, object> dictionarieCategories,
            CategoryEntity categoryEntity)
        {

            var parent = new Dictionary<string, object>();

            dictionarieCategories.Add(categoryEntity.CategoryName, parent);

            if (categoryEntity.ChildCategories != null && categoryEntity.ChildCategories.Any())
            {
                foreach (var child in categoryEntity.ChildCategories)
                {
                    AddParent(parent, child);

                    var grandchild = sqlServerContext.Categories
                        .Include(c => c.ParentCategory)
                        .FirstOrDefault(c => c.ParentCategoryId == categoryEntity.ParentCategoryId);
                }
            }

            return dictionarieCategories;

        }
    }
}