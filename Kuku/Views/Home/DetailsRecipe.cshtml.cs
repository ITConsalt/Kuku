using Kuku.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Views.Home
{
    public class DetailModel : PageModel
    {
        private readonly EFContext db;

        public DetailModel(EFContext context)
        {
            db = context;
        }

        public Recipe Recipe_RecipeDetails { get; set; } = new Recipe();

        public class Recipe
        {
            public int RecipeId { get; set; }
            public string RecipeName { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public byte[] BigImageData { get; set; }
            public byte[] PreviewImageData { get; set; }
            public string UserId { get; set; }

            public List<RecipeDetails> RecipeDetails { get; set; } = new List<RecipeDetails>();
        }

        public class RecipeDetails
        {
            public int RecipeDetailsId { get; set; }
            public int RecipeId { get; set; }
            public string Description { get; set; }
            public DateTime CreatedDate { get; set; }
            public byte[] PreviewImageData { get; set; }
            public byte[] BigImageData { get; set; }
        }

        public async Task OnGet(int? id)
        {
            var recipe = await db.Recipe.FindAsync(id);
            Recipe_RecipeDetails = new Recipe()
            {
                CreatedDate = recipe.CreatedDate,
                RecipeId = recipe.RecipeId,
                RecipeName = recipe.RecipeName,
                Description = recipe.Description,
                BigImageData = recipe.BigImageData,
                PreviewImageData = recipe.PreviewImageData,
                UserId = recipe.UserId,

                RecipeDetails = recipe.RecipeDetails.Select(oi => new RecipeDetails()
                {
                    RecipeDetailsId = oi.RecipeDetailsId,
                    RecipeId = oi.RecipeId,
                    Description = oi.Description,
                    CreatedDate = oi.CreatedDate,
                    PreviewImageData = oi.PreviewImageData,
                    BigImageData = oi.BigImageData
                }).ToList()
            };
        }
    }
}
