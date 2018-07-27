using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kuku.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.ImageSharp.Processing.Filters;
using Microsoft.AspNetCore.Hosting;
using SixLabors.Primitives;
using Kuku.ViewModels;

namespace Kuku.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        IHostingEnvironment _appEnvironment;

        private EFContext db;
        public HomeController(EFContext context, UserManager<User> userManager, IConfiguration configuration, IHostingEnvironment appEnvironment)

        {
            db = context;
            _userManager = userManager;
            Configuration = configuration;
            _appEnvironment = appEnvironment;
        }

        public IConfiguration Configuration { get; }

        public async Task<IActionResult> Index()
        {
            return View(await db.Recipes.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> DetailsRecipe(int? id)
        {
            Recipe recipe = await db.Recipes.FirstOrDefaultAsync(p => p.RecipeId == id);
            if (recipe == null)
            {
                return BadRequest("No such order found for this user.");
            }
            IQueryable<RecipeDetail> recipeDetails = db.RecipeDetails.Include(p => p.Recipe);
            if (id != null && id != 0)
            {
                recipeDetails = recipeDetails.Where(p => p.RecipeId == id);
            }
            IQueryable<Recipe_Product> recipe_Products = db.Recipe_Products.Include(p => p.Recipe);
            if (id != null && id != 0)
            {
                recipe_Products = recipe_Products.Where(p => p.RecipeId == id);
            }
            var products = db.Recipe_Products.Select(sc => sc.Product).ToList();
            IQueryable<Recipe_TypeOfDish> recipe_TypeOfDishes = db.Recipe_TypeOfDishes.Include(p => p.Recipe);
            if (id != null && id != 0)
            {
                recipe_TypeOfDishes = recipe_TypeOfDishes.Where(p => p.RecipeId == id);
            }
            var typeOfDishes = db.Recipe_TypeOfDishes.Select(sc => sc.TypeOfDish).ToList();
            IQueryable<Recipe_NationalCuisine> recipe_NationalCuisines = db.Recipe_NationalCuisines.Include(p => p.Recipe);
            if (id != null && id != 0)
            {
                recipe_NationalCuisines = recipe_NationalCuisines.Where(p => p.RecipeId == id);
            }
            var nationalCuisines = db.Recipe_NationalCuisines.Select(sc => sc.NationalCuisine).ToList();
            RecipeViewModel viewModel = new RecipeViewModel
            {
                Recipes = recipe,
                RecipesDetails = recipeDetails,
                Recipe_Products = recipe_Products,
                Products = products,
                Recipe_TypeOfDishes = recipe_TypeOfDishes,
                TypeOfDishes = typeOfDishes,
                Recipe_NationalCuisenes = recipe_NationalCuisines,
                NationalCuisines = nationalCuisines
            };
            return View(viewModel);
        }
    }
}
