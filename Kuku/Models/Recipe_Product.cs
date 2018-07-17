using System.ComponentModel.DataAnnotations;

namespace Kuku.Models
{
    public class Recipe_Product
    { 
        [Key]
        public int RecipeId { get; set; }
        public int ProductId { get; set; }
    }
}
