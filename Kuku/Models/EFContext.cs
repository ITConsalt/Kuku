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
        public DbSet<NationalityCuisine> NationalityCuisine { get; set; }
        public DbSet<TypeOfDish> TypeOfDish { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OriginalImage> OriginalImage { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeDetail> RecipeDetails { get; set;}
        public DbSet<Recipe_Product> Recipe_Products { get; set; }


        public EFContext(DbContextOptions<EFContext> options)
                : base(options)
            {
            }

    }
}