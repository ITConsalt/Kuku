using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; } // тип продукта (овощи, фрукты, напитки, мясо...)
        public List<Product> Products { get; set; }//for adding products to recipe
        public ProductType()                       //for adding products to recipe
        {
            Products = new List<Product>();        //for adding products to recipe
        }

        //public IEnumerable<Product> Products { get; set; }
    }
}
