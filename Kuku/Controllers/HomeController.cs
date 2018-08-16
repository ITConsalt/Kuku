﻿using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kuku.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Kuku.ViewModels;
using System.Collections.Generic;

namespace Kuku.Controllers
{
    public class HomeController : Controller
    {
        private EFContext db;
        public HomeController(EFContext context)

        {
            db = context;
        }

        public IActionResult Index()
        {
            //IQueryable<Recipe_Product> recipe_Products = db.Recipe_Products.Include(p => p.Recipe);
            //if (id != null && id != 0)
            //{
            //    recipe_Products = recipe_Products.Where(p => p.RecipeId == id);
            //}
            //var products = db.Recipe_Products.Select(sc => sc.Product).ToList();
            //List<AspNetUser> aspNetUsers = db.AspNetUsers.ToList();
            List<Product> products = db.Recipe_Products.Select(rp => rp.Product).ToList();
            List<FilterProduct> filterProducts = new List<FilterProduct>();
            HashSet<int> productId = new HashSet<int>();
            foreach (Product product in products.ToList())
            {
                if (!productId.Add(product.ProductId))
                {
                    FilterProduct filterProduct = filterProducts.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (filterProduct != null)
                    {
                        filterProduct.Count++;
                    }
                }
                else
                {
                    filterProducts.Add(new FilterProduct() { ProductId = product.ProductId, ProductName = product.ProductName, Count = 1});
                }
            }

            List<NationalCuisine> nationalCuisines = db.Recipe_NationalCuisines.Select(rn => rn.NationalCuisine).ToList();
            List<FilterNationalCuisine> filterNationalCuisines = new List<FilterNationalCuisine>();
            HashSet<int> nationalCuisineId = new HashSet<int>();
            foreach (NationalCuisine nationalCuisine in nationalCuisines.ToList())
            {
                if (!nationalCuisineId.Add(nationalCuisine.NationalCuisineId))
                {
                    FilterNationalCuisine filterNationalCuisine = filterNationalCuisines.FirstOrDefault(p => p.NationalCuisineId == nationalCuisine.NationalCuisineId);
                    if (filterNationalCuisine != null)
                    {
                        filterNationalCuisine.Count++;
                    }
                }
                else
                {
                    filterNationalCuisines.Add(new FilterNationalCuisine() { NationalCuisineId = nationalCuisine.NationalCuisineId, NationalCuisineName = nationalCuisine.NationalCuisineName, Count = 1 });
                }
            }
            List<TypeOfDish> typeOfDishes = db.Recipe_TypeOfDishes.Select(rt => rt.TypeOfDish).ToList();
            List<FilterTypeOfDish> filterTypeOfDishes = new List<FilterTypeOfDish>();
            HashSet<int> typeOfDishId = new HashSet<int>();
            foreach (TypeOfDish typeOfDish in typeOfDishes.ToList())
            {
                if (!typeOfDishId.Add(typeOfDish.TypeOfDishId))
                {
                    FilterTypeOfDish filterTypeOfDish = filterTypeOfDishes.FirstOrDefault(p => p.TypeOfDishId == typeOfDish.TypeOfDishId);
                    if (filterTypeOfDish != null)
                    {
                        filterTypeOfDish.Count++;
                    }
                }
                else
                {
                    filterTypeOfDishes.Add(new FilterTypeOfDish() { TypeOfDishId = typeOfDish.TypeOfDishId, TypeOfDishName = typeOfDish.TypeOfDishName, Count = 1 });
                }
            }
            List<Recipe> recipes = db.Recipes.ToList();
            FilterViewModel viewModel = new FilterViewModel
            {
                Recipes = recipes,
                //Recipe_Products = recipe_Products,
                Products = products,
                FilterProducts = filterProducts,
                FilterNationalCuisines = filterNationalCuisines,
                FilterTypeOfDishes = filterTypeOfDishes,
                //Recipe_TypeOfDishes = recipe_TypeOfDishes,
                TypeOfDishes = typeOfDishes,
                //Recipe_NationalCuisenes = recipe_NationalCuisines,
                NationalCuisines = nationalCuisines,
                //MeasuringSystems = measuringSystem
            };
            return View(viewModel);
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
            List<MeasuringSystem> measuringSystems = await db.MeasuringSystems.ToListAsync();
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
                NationalCuisines = nationalCuisines,
                //MeasuringSystems = measuringSystem
            };
            return View(viewModel);
        }
    }
}
