using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class EFContext : DbContext
    {        
            public DbSet<NationalityCuisine> NationalityCuisine { get; set; }
            public DbSet<TypeOfDish> TypeOfDish { get; set; }
            public DbSet<ProductType> ProductType { get; set; }
            public DbSet<Product> Product { get; set; }
            public DbSet<OriginalImage> OriginalImage { get; set; }

        public EFContext(DbContextOptions<EFContext> options)
                : base(options)
            {
            }
        
    }
}
