using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class EFContext : DbContext
    {
        public DbSet<NationalCuisine> NationalCuisines { get; set; }
        public DbSet<TypeOfDish> TypeOfDishes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OriginalImage> OriginalImages { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeDetail> RecipeDetails { get; set;}
        public DbSet<Recipe_Product> Recipe_Products { get; set; }
        public DbSet<Recipe_TypeOfDish> Recipe_TypeOfDishes { get; set; }
        public DbSet<Recipe_NationalCuisene> Recipe_NationalCuisenes { get; set; }

        //не только для того что бы создавались таблицы, но и для того что бы asp.net знал связи
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe_Product>()
                .HasKey(t => new { t.ProductId, t.RecipeId });

            modelBuilder.Entity<Recipe_Product>()
                .HasOne(sc => sc.Product)
                .WithMany(s => s.Recipe_Products)
                .HasForeignKey(sc => sc.ProductId);

            modelBuilder.Entity<Recipe_Product>()
                .HasOne(sc => sc.Recipe)
                .WithMany(c => c.Recipe_Products)
                .HasForeignKey(sc => sc.RecipeId);
        }

        public EFContext(DbContextOptions<EFContext> options)
                : base(options)
            {
            }

    }
}