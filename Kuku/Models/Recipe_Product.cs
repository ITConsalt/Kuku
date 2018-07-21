using System.ComponentModel.DataAnnotations;

namespace Kuku.Models
{
    public class Recipe_Product
    { 
        //public int Id { get; set; }              //not use, primary key required
        [Key]
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public Product Product { get; set; }
        public Recipe Recipe { get; set; }
    }
}
