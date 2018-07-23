using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuku.Models
{
    public class Recipe_Product
    {
        //public int Id { get; set; }              //not use, primary key required
        //[Key, ForeignKey("FK_Recipe_Products_Recipes_RecipeId")]
        //[Column("RecipeId", Order = 1)]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        //[Key, ForeignKey("FK_Recipe_Products_Products_ProductId")]
        //[Column("ProductId", Order = 0)]
        //[Key]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        //public string ProductName { get; set; }
    }
}
