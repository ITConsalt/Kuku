using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } // продукты (яблоки, картофель, сахар, свенина...)
        public int ProductTypeId { get; set; } // тип продукта, отношение у таблице типов продуктов
        public ProductType ProductType { get; set; }//for adding products to recipe
        public int MeasuringSystemId { get; set; }
        public MeasuringSystem MeasuringSystem { get; set; }

        public List<Recipe_Product> Recipe_Products { get; set; }


        public Product()
        {
            Recipe_Products = new List<Recipe_Product>();
        }
    }
}
