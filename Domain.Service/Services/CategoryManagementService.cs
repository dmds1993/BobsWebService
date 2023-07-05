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
                }
            }

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
    }
}