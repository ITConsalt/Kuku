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

        public virtual ICollection<RecipeDetails> RecipeDetails { get; set; }
        public Recipe()
        {
            RecipeDetails = new List<RecipeDetails>();
        }
    }
}
