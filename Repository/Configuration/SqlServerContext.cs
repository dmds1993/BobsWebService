using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.SqlServer.Context
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options)
        {
        }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CategoryHierarchyEntity> CategoryHierarchies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryEntity>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<CategoryHierarchyEntity>()
                .HasKey(ch => new { ch.ParentCategoryId, ch.ChildCategoryId });

            modelBuilder.Entity<CategoryHierarchyEntity>()
                .HasOne(ch => ch.ParentCategory)
                .WithMany()
                .HasForeignKey(ch => ch.ParentCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CategoryHierarchyEntity>()
                .HasOne(ch => ch.ChildCategory)
                .WithMany()
                .HasForeignKey(ch => ch.ChildCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}