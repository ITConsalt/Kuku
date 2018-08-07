using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.ViewModels
{
    public class FilterProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } // продукты (яблоки, картофель, сахар, свенина...)
        public int Count;
    }
}
