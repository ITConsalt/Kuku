﻿using Kuku.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.RecipeViewModels
{
    public class Recipe_RecipeDetails
    {
        public Recipe Recipe { get; set; }
        public IEnumerable<RecipeDetails> RecipeDetails { get; set; }
    }
}
