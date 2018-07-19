using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class Recipe_NationalCuisene
    {
        [Key]
        public int RecipeId { get; set; }
        public int NationalCuisineId { get; set; }
    }
}
