using Kuku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.ViewModels
{
    public class RecipeViewModel
    {
        public Recipe Recipes { get; set; }
        public IEnumerable<RecipeDetail> RecipesDetails { get; set; }
        public IEnumerable<Recipe_Product> Recipe_Products { get; set; }
    }

}
