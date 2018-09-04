using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }//for adding products to recipe
        public int MeasuringSystemId { get; set; }
        public MeasuringSystem MeasuringSystem { get; set; }
        public int Count;

        //public Recipe_Product Recipe_Product { get; set; }

        public List<Recipe_Product> Recipe_Products { get; set; }
    }
}
