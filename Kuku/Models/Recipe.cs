using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string RecipeName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] BigImageData { get; set; }
        public byte[] PreviewImageData { get; set; }
        public string UserId { get; set; }

        public IEnumerable<RecipeDetail> RecipesDetails { get; set; }
        public List<Recipe_Product> Recipe_Products { get; set; }
        public List<Recipe_TypeOfDish> Recipe_TypeOfDishes { get; set; }
        public List<Recipe_NationalCuisine> Recipe_NationalCuisenes { get; set; }

        public Recipe()
        {
            RecipesDetails = new List<RecipeDetail>();
            Recipe_Products = new List<Recipe_Product>();
            Recipe_TypeOfDishes = new List<Recipe_TypeOfDish>();
            Recipe_NationalCuisenes = new List<Recipe_NationalCuisine>();
        }
        // так тоже работает:
        // public List<RecipeDetails> RecipesDetails { get; set; } = new List<RecipeDetails>();
    }
}
