using Kuku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.ViewModels
{
    public class RecipeViewModel
    {
        public IEnumerable<RecipeDetail> RecipesDetails { get; set; }
        public Recipe Recipes { get; set; }
    }

}
