using Domain.Entities;
using Domain.Interfaces.Repository;
using Domain.Interfaces.Service;
using Domain.Models;
using Domain.Service.Service;
using Infra.SqlServer.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.Common;

namespace Domain.Service.Tests
{
    public class CategoryManagementServiceTests
    {
        [Fact]
        public void Create_Should_AddCategoriesToDatabase()
        {
            // Arrange
            var categoryDictionary = new Dictionary<string, string>
            {
                { "Category1", null },
                { "Category2", "Child1" },
                { "Category3", "Child2" }
            };

            var mockLogger = new Mock<ILogger<CategoryManagementService>>();
            var mockContextOptions = new DbContextOptionsBuilder<SqlServerContext>()
                .UseInMemoryDatabase(databaseName: "BOBS_DATABASE")
                .Options;

            using (var context = new SqlServerContext(mockContextOptions))
            {
                var databaseMock = new Mock<DatabaseFacade>(context);
                var executionStrategyMock = new Mock<IExecutionStrategy>();

                var transactionMock = new Mock<IDbContextTransaction>();
                transactionMock.Setup(t => t.Commit());

                // Create a derived class of DatabaseFacade to mock the BeginTransaction method
                databaseMock.Setup(d => d.BeginTransaction())
                    .Returns(transactionMock.Object);

                var executed = false;
                databaseMock.Setup(d => d.CreateExecutionStrategy())
                    .Callback(() => executed = true)
                    .Returns(executionStrategyMock.Object);

                var service = new CategoryManagementService(context, mockLogger.Object);

                service.Create(categoryDictionary);

            }

            // Assert
            using (var context = new SqlServerContext(mockContextOptions))
            {
                Assert.Equal(5, context.Categories.Count());

                var category1 = context.Categories.Include(c => c.ChildCategories).FirstOrDefault(c => c.CategoryName == "Category1");
                Assert.NotNull(category1);
                Assert.Equal(1, category1.Level);
                Assert.Empty(category1.ChildCategories);

                var category2 = context.Categories.Include(c => c.ChildCategories).FirstOrDefault(c => c.CategoryName == "Category2");
                Assert.NotNull(category2);
                Assert.Equal(1, category2.Level);
                Assert.Single(category2.ChildCategories);
                Assert.Equal("Child1", category2.ChildCategories.First().CategoryName);

                var category3 = context.Categories.Include(c => c.ChildCategories).FirstOrDefault(c => c.CategoryName == "Category3");
                Assert.NotNull(category3);
                Assert.Equal(1, category3.Level);
                Assert.Single(category3.ChildCategories);
                Assert.Equal("Child2", category3.ChildCategories.First().CategoryName);
            }
        }

        [Fact]
        public void Create_Should_AddCategoriesToDatabase_AndValidateLevel()
        {
            var categoryDictionary = new Dictionary<string, string>
            {
                { "Father", "" },
                { "First Child", "Son from First Child" }
            };

            // Arrange
            var categoryId = 1;
            var mockLogger = new Mock<ILogger<CategoryManagementService>>();
            var mockContextOptions = new DbContextOptionsBuilder<SqlServerContext>()
                .UseInMemoryDatabase(databaseName: "BOBS_DATABASE")
                .Options;

            using (var context = new SqlServerContext(mockContextOptions))
            {
                {
                    var databaseMock = new Mock<DatabaseFacade>(context);
                    var executionStrategyMock = new Mock<IExecutionStrategy>();

                    var transactionMock = new Mock<IDbContextTransaction>();
                    transactionMock.Setup(t => t.Commit());

                    // Create a derived class of DatabaseFacade to mock the BeginTransaction method
                    databaseMock.Setup(d => d.BeginTransaction())
                        .Returns(transactionMock.Object);

                    var executed = false;
                    databaseMock.Setup(d => d.CreateExecutionStrategy())
                        .Callback(() => executed = true)
                        .Returns(executionStrategyMock.Object);

                    var service = new CategoryManagementService(context, mockLogger.Object);

                    service.Create(categoryDictionary);

                    service.Create(new Dictionary<string, string>
                    {
                        { "Father", "my first child" }
                    });

                    // Act
                    var result = service.GetCategory(categoryDictionary.First().Key);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Single(result);
                    Assert.Contains("Father", result.FirstOrDefault().Key);
                    var categoryChildren = (Dictionary<string, object>)result.FirstOrDefault().Value;
                    Assert.NotNull(categoryChildren);
                    Assert.Contains("my first child", categoryChildren.FirstOrDefault().Key);

                }
            }
        }

        [Fact]
        public void Create_Should_Return_Exception_When_Has_Limit_Level()
        {
            var categoryDictionary = new Dictionary<string, string>
            {
                { "Father", "" },
                { "First Child", "Son from First Child" }
            };

            // Arrange
            var categoryId = 1;
            var mockLogger = new Mock<ILogger<CategoryManagementService>>();
            var mockContextOptions = new DbContextOptionsBuilder<SqlServerContext>()
                .UseInMemoryDatabase(databaseName: "BOBS_DATABASE")
                .Options;

            using (var context = new SqlServerContext(mockContextOptions))
            {
                {
                    var databaseMock = new Mock<DatabaseFacade>(context);
                    var executionStrategyMock = new Mock<IExecutionStrategy>();

                    var transactionMock = new Mock<IDbContextTransaction>();
                    transactionMock.Setup(t => t.Commit());

                    // Create a derived class of DatabaseFacade to mock the BeginTransaction method
                    databaseMock.Setup(d => d.BeginTransaction())
                        .Returns(transactionMock.Object);

                    var executed = false;
                    databaseMock.Setup(d => d.CreateExecutionStrategy())
                        .Callback(() => executed = true)
                        .Returns(executionStrategyMock.Object);

                    var service = new CategoryManagementService(context, mockLogger.Object);

                    service.Create(categoryDictionary);

                    Assert.Throws<Exception>(() => service.Create(new Dictionary<string, string>
                    {
                        { "Father", "Father Child" },
                        { "Father Child", "Father Child Son" },
                        { "Father Child Son", "Father Child Son Grand Child" },
                        { "Father Child Son Grand Child", "Father Child Son Grand Child 2" },
                        { "Father Child Son Grand Child 2", "Father Child Son Grand Child 3" },
                        { "Father Child Son Grand Child 3", "Father Child Son Grand Child 4" },
                        { "Father Child Son Grand Child 4", "Father Child Son Grand Child 5" },
                        { "Father Child Son Grand Child 5", "Father Child Son Grand Child 6" },
                        { "Father Child Son Grand Child 6", "Father Child Son Grand Child 7" },
                        { "Father Child Son Grand Child 7", "Father Child Son Grand Child 8" }
                    }));
                }
            }
        }
    }
}