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
    public class HomeController : Controller
    {
        private EFContext db;

        public HomeController(EFContext context)

        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Recipes.ToListAsync());
        }



        [HttpGet]
        public ActionResult SelectProduct(int? recipeid, int? productType, string name)
        {
            Recipe recipeidcontext = db.Recipes.FirstOrDefault(p => p.RecipeId == recipeid);
            if (recipeidcontext == null)
            {
                return BadRequest("No such order found for this user.");
            }
            IQueryable<Product> products = db.Products.Include(p => p.ProductType);
            if (productType != null && productType != 0)
            {
                products = products.Where(p => p.ProductTypeId == productType);
            }
            if (!String.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.ProductName.Contains(name));
            }

            List<ProductType> productTypes = db.ProductTypes.ToList();
            // устанавливаем начальный элемент, который позволит выбрать всех
            productTypes.Insert(0, new ProductType { ProductTypeName = "All type", ProductTypeId = 0 });

            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = products.ToList(),
                ProductTypes = new SelectList(productTypes, "ProductTypeId", "ProductTypeName"),
                Name = name,
                Recipe = recipeidcontext
            };
            return View(viewModel);
            //return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SelectProduct([FromQuery] Recipe recipe, [FromQuery] Product product)
        {
            int productId = product.ProductId;
            int recipeId = recipe.RecipeId;
            Recipe_Product recipe_Product = new Recipe_Product
            {
                ProductId = productId,
                RecipeId = recipeId
            };
            db.Recipe_Products.Add(recipe_Product);
            await db.SaveChangesAsync();
            return Ok("Product added to recipe");
        }

        [HttpGet]
        public ActionResult SelectTypeOfDish(int? recipeid, string name)
        {
            Recipe recipeidcontext = db.Recipes.FirstOrDefault(p => p.RecipeId == recipeid);
            if (recipeidcontext == null)
            {
                return BadRequest("No such order found for this user.");
            }
            IQueryable<TypeOfDish> typeOfDishes = db.TypeOfDishes;
            if (!String.IsNullOrEmpty(name))
            {
                typeOfDishes = typeOfDishes.Where(p => p.TypeOfDishName.Contains(name));
            }

            TypeOfDishesListViewModel viewModel = new TypeOfDishesListViewModel
            {
                TypeOfDishes = typeOfDishes.ToList(),
                //ProductTypes = new SelectList(productTypes, "ProductTypeId", "ProductTypeName"),
                Name = name,
                Recipe = recipeidcontext
            };
            return View(viewModel);
            //return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SelectTypeOfDish([FromQuery] Recipe recipe, [FromQuery] TypeOfDish typeOfDish)
        {
            int typeOfDishId = typeOfDish.TypeOfDishId;
            int recipeId = recipe.RecipeId;
            Recipe_TypeOfDish recipe_TypeOfDish = new Recipe_TypeOfDish
            {
                TypeOfDishId = typeOfDishId,
                RecipeId = recipeId
            };
            db.Recipe_TypeOfDishes.Add(recipe_TypeOfDish);
            await db.SaveChangesAsync();
            return Ok("Type of dish added to recipe");
        }

        [HttpGet]
        public ActionResult SelectNationalCuisine(int? recipeid, string name)
        {
            Recipe recipeidcontext = db.Recipes.FirstOrDefault(p => p.RecipeId == recipeid);
            if (recipeidcontext == null)
            {
                return BadRequest("No such order found for this user.");
            }
            IQueryable<NationalCuisine> nationalCuisines = db.NationalCuisines;
            if (!String.IsNullOrEmpty(name))
            {
                nationalCuisines = nationalCuisines.Where(p => p.NationalCuisineName.Contains(name));
            }

            NationalCuisineListViewModel viewModel = new NationalCuisineListViewModel
            {
                NationalCuisines = nationalCuisines.ToList(),
                Name = name,
                Recipe = recipeidcontext
            };
            return View(viewModel);
            //return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> SelectNationalCuisine([FromQuery] Recipe recipe, [FromQuery] NationalCuisine nationalCuisine)
        {
            int nationalCuisineId = nationalCuisine.NationalCuisineId;
            int recipeId = recipe.RecipeId;
            Recipe_NationalCuisine recipe_NationalCuisine = new Recipe_NationalCuisine
            {
                NationalCuisineId = nationalCuisineId,
                RecipeId = recipeId
            };
            db.Recipe_NationalCuisines.Add(recipe_NationalCuisine);
            await db.SaveChangesAsync();
            return Ok("National cuisine added to recipe");
        }


        public async Task<IActionResult> NationalCuisine()
        {
            return View(await db.NationalCuisines.ToListAsync());
        }

        public async Task<IActionResult> DetailsNationalCuisine(int? id)
        {
            if (id != null)
            {
                NationalCuisine nationalCuisine = await db.NationalCuisines.FirstOrDefaultAsync(p => p.NationalCuisineId == id);
                if (nationalCuisine != null)
                    return View(nationalCuisine);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteNationalCuisine")]
        public async Task<IActionResult> ConfirmDeleteNationalCuisine(int? id)
        {
            if (id != null)
            {
                NationalCuisine nationalCuisine = await db.NationalCuisines.FirstOrDefaultAsync(p => p.NationalCuisineId == id);
                if (nationalCuisine != null)
                    return View(nationalCuisine);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteNationalCuisine(int? id)
        {
            if (id != null)
            {
                /*NationalCuisine nationalCuisine = await db.NationalCuisine.FirstOrDefaultAsync(p => p.NationalCuisineId == id);
                if (nationalCuisine != null)
                {
                    db.NationalCuisine.Remove(nationalCuisine);
                    await db.SaveChangesAsync();
                    return RedirectToAction("NationalCuisine");
                }*/
                NationalCuisine nationalCuisine = new NationalCuisine { NationalCuisineId = id.Value };
                db.Entry(nationalCuisine).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("NationalCuisine");//в отличии от предыдущего, этот метод - оптимизированный и с проверкой на существование записи в БД
            }
            return NotFound();
        }
        public async Task<IActionResult> EditNationalCuisine(int? id)
        {
            if (id != null)
            {
                NationalCuisine nationalCuisine = await db.NationalCuisines.FirstOrDefaultAsync(p => p.NationalCuisineId == id);
                if (nationalCuisine != null)
                    return View(nationalCuisine);//////////
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditNationalCuisine(NationalCuisine cuisine)
        {
            db.NationalCuisines.Update(cuisine);
            await db.SaveChangesAsync();
            return RedirectToAction("NationalCuisine");
        }

        public async Task<IActionResult> ProductType()
        {
            return View(await db.ProductTypes.ToListAsync());
        }

        public async Task<IActionResult> DetailsProductType(int? id)
        {
            if (id != null)
            {
                ProductType productType = await db.ProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == id);
                if (productType != null)
                    return View(productType);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteProductType")]
        public async Task<IActionResult> ConfirmDeleteProductType(int? id)
        {
            if (id != null)
            {
                ProductType productType = await db.ProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == id);
                if (productType != null)
                    return View(productType);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductType(int? id)
        {
            if (id != null)
            {

                ProductType productType = new ProductType { ProductTypeId = id.Value };
                db.Entry(productType).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("ProductType");
            }
            return NotFound();
        }
        public async Task<IActionResult> EditProductType(int? id)
        {
            if (id != null)
            {
                ProductType productType = await db.ProductTypes.FirstOrDefaultAsync(p => p.ProductTypeId == id);
                if (productType != null)
                    return View(productType);
            }
            return NotFound();
        }
        [HttpPost]//////////////////////
        public async Task<IActionResult> EditProductType(ProductType type)
        {
            db.ProductTypes.Update(type);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductType");
        }

        public async Task<IActionResult> Product()
        {
            return View(await db.Products.ToListAsync());
        }
        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (product != null)
                    return View(product);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteProduct")]
        public async Task<IActionResult> ConfirmDeleteProduct(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (product != null)
                    return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id != null)
            {

                Product product = new Product { ProductId = id.Value };
                db.Entry(product).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Product");
            }
            return NotFound();
        }

        public async Task<IActionResult> TypeOfDish()
        {
            return View(await db.TypeOfDishes.ToListAsync());
        }
        public async Task<IActionResult> DetailsTypeOfDish(int? id)
        {
            if (id != null)
            {
                TypeOfDish typeOfDish = await db.TypeOfDishes.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
                if (typeOfDish != null)
                    return View(typeOfDish);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteTypeOfDish")]
        public async Task<IActionResult> ConfirmDeleteTypeOfDish(int? id)
        {
            if (id != null)
            {
                TypeOfDish typeOfDish = await db.TypeOfDishes.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
                if (typeOfDish != null)
                    return View(typeOfDish);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTypeOfDish(int? id)
        {
            if (id != null)
            {

                TypeOfDish typeOfDish = new TypeOfDish { TypeOfDishId = id.Value };
                db.Entry(typeOfDish).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("TypeOfDish");
            }
            return NotFound();
        }
        public async Task<IActionResult> EditTypeOfDish(int? id)
        {
            if (id != null)
            {
                TypeOfDish typeOfDish = await db.TypeOfDishes.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
                if (typeOfDish != null)
                    return View(typeOfDish);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditTypeOfDish(TypeOfDish dish)
        {
            db.TypeOfDishes.Update(dish);
            await db.SaveChangesAsync();
            return RedirectToAction("TypeOfDish");
        }


        [HttpGet]
        [ActionName("DeleteRecipe")]
        public async Task<IActionResult> ConfirmDeleteRecipe(int? id)
        {
            if (id != null)
            {
                Recipe recipe = await db.Recipes.FirstOrDefaultAsync(p => p.RecipeId == id);
                if (recipe != null)
                    return View(recipe);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int? id)
        {
            if (id != null)
            {
                /*NationalCuisine nationalCuisine = await db.NationalCuisine.FirstOrDefaultAsync(p => p.NationalCuisineId == id);
                if (nationalCuisine != null)
                {
                    db.NationalCuisine.Remove(nationalCuisine);
                    await db.SaveChangesAsync();
                    return RedirectToAction("NationalCuisine");
                }*/
                Recipe recipe = new Recipe { RecipeId = id.Value };
                db.Entry(recipe).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");//в отличии от предыдущего, этот метод - оптимизированный и с проверкой на существование записи в БД
            }
            return NotFound();
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

        public async Task<IActionResult> RecipeDetail()
        {
            return View(await db.RecipeDetails.ToListAsync());
        }


        [HttpGet]
        [ActionName("DeleteRecipeDetail")]
        public async Task<IActionResult> ConfirmDeleteRecipeDetail(int? id)
        {
            if (id != null)
            {
                RecipeDetail recipeDetail = await db.RecipeDetails.FirstOrDefaultAsync(p => p.RecipeDetailId == id);
                if (recipeDetail != null)
                    return View(recipeDetail);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRecipeDetail(int? id)
        {
            if (id != null)
            {

                RecipeDetail recipeDetail = new RecipeDetail { RecipeDetailId = id.Value };
                db.Entry(recipeDetail).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("RecipeDetail");
            }
            return NotFound();
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
    }
}