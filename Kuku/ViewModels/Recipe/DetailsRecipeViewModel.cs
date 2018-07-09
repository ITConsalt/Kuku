using Kuku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.ViewModels.Recipe
{
    public class DetailsRecipeViewModel
    {
        public Models.Recipe Recipe { get; set; }
        public ICollection<RecipeDetails> RecipeDetails { get; set; }
    }
}
