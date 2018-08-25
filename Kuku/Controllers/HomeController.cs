using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kuku.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Kuku.ViewModels;
using System.Collections.Generic;
using System;
using System.Collections;

namespace Kuku.Controllers
{
    public class HomeController : Controller
    {
        private EFContext db;

        public HomeController(EFContext context)

        {
            db = context;
        }

        //www.site.ua/filter/cuisines:12,15/
        //www.site.ua/filter/cuisines:12,15/product:12,15/?filters=kjhfncdhg
        //www.site.ua/filter/product:12,15/
        [Route("filter")]
        public IActionResult Filters(string flp,string flc, string fld)
        {
            //IQueryable<Recipe_Product> recipe_Products = db.Recipe_Products.Include(p => p.Recipe);
            //if (id != null && id != 0)
            //{
            //    recipe_Products = recipe_Products.Where(p => p.RecipeId == id);
            //}
            //var products = db.Recipe_Products.Select(sc => sc.Product).ToList();

            string SqlFilterProduct = "join Recipe_Products frp on frp.RecipeId = r.RecipeId "; ;
            string SqlFilterNationalCuisines = "join Recipe_NationalCuisines frn on frn.RecipeId = r.RecipeId ";
            string SqlFilterTypeOfDishes = "join Recipe_TypeOfDishes frt on frt.RecipeId = r.RecipeId ";
            string[] up = { };
            string[] uc = { };
            string[] ud = { };
            if (flp != null)
            {
                SqlFilterProduct = "join Recipe_Products frp on frp.RecipeId = r.RecipeId and frp.ProductId in (" + flp + ") ";
                up = flp.Split(',');
            }
            if (flc != null)
            {
                SqlFilterNationalCuisines = "join Recipe_NationalCuisines frn on frn.RecipeId = r.RecipeId and frn.NationalCuisineId in (" + flc + ") ";
                uc = flc.Split(',');
            }
            if (fld != null)
            {
                SqlFilterTypeOfDishes = "join Recipe_TypeOfDishes frt on frt.RecipeId = r.RecipeId and frt.TypeOfDishId in (" + fld + ") ";
                ud = fld.Split(',');
            }

            
            string SqlFilter = "SELECT Distinct " +
                "Products.ProductId as itemId, 'Products' as itemType, Products.ProductName as itemName, " +
                "COUNT(Distinct Recipe_Products.RecipeId) AS itemCount, 1 as itemSort " +
                "FROM Products JOIN Recipe_Products ON Recipe_Products.ProductId = Products.ProductId " +
                "WHERE Recipe_Products.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY Products.ProductId,Products.ProductName " +
                "UNION " +
                "SELECT Distinct " +
                "NationalCuisines.NationalCuisineId as itemId, 'NationalCuisines' as itemType, NationalCuisines.NationalCuisineName as itemName, " +
                "COUNT(Distinct Recipe_NationalCuisines.RecipeId) AS itemCount, 2 as itemSort " +
                "FROM NationalCuisines JOIN Recipe_NationalCuisines ON Recipe_NationalCuisines.NationalCuisineId = NationalCuisines.NationalCuisineId " +
                "WHERE Recipe_NationalCuisines.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY NationalCuisines.NationalCuisineId,NationalCuisines.NationalCuisineName " +
                "UNION " +
                "SELECT Distinct " +
                "TypeOfDishes.TypeOfDishId as itemId, 'TypeOfDishes' as itemType, TypeOfDishes.TypeOfDishName as itemName, " +
                "COUNT(Distinct Recipe_TypeOfDishes.RecipeId) AS itemCount, 3 as itemSort " +
                "FROM TypeOfDishes JOIN Recipe_TypeOfDishes ON Recipe_TypeOfDishes.TypeOfDishId = TypeOfDishes.TypeOfDishId " +
                "WHERE Recipe_TypeOfDishes.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY TypeOfDishes.TypeOfDishId,TypeOfDishes.TypeOfDishName " +
                "ORDER BY itemSort, itemName;"

            ;
            List<Filter> Filters = db.Filters.FromSql(SqlFilter).ToList();
            List<Filter> Products = new List<Filter>();
            List<Filter> NationalCuisines = new List<Filter>();
            List<Filter> TypeOfDishes = new List<Filter>();
            int index;
            string[] u;
            bool t;
            foreach (Filter filter in Filters)
            {
                t = false;
                List<string> s = new List<string>();
                filter.itemLink = "/";
                switch (filter.itemType)
                {
                    case "Products":
                        u = up;
                        index = Array.IndexOf(u, filter.itemId + "");
                        if (index > -1)
                        {
                            filter.itemChecked = true;
                            Delete(ref u, index);
                            
                            //Array.Clear(u, index, 1);
                            if (u.Length > 0)
                            {
                                
                                t = true;
                                s.Add("flp=" + String.Join(",", u));
                            }
                        }
                        else
                        {
                            t = true;
                            if (u.Length > 0)
                            {
                                s.Add("flp=" + String.Join(",", u) + "," + filter.itemId);
                            }
                            else
                            {
                                s.Add("flp=" + filter.itemId);
                            }

                        };
                        if (uc.Length > 0)
                        {
                            t = true;
                            s.Add("flc=" + String.Join(",", uc));
                        }
                        if (ud.Length > 0)
                        {
                            t = true;
                            s.Add("fld=" + String.Join(",", ud));
                        }
                        if (t)
                        {
                            filter.itemLink = "/filter?" + String.Join("&", s);
                        }
                        Products.Add(filter);
                        break;
                    case "NationalCuisines":
                        u = uc;
                        index = Array.IndexOf(u, filter.itemId + "");
                        if (up.Length > 0)
                        {
                            t = true;
                            s.Add("flp=" + String.Join(",", up));
                        }
                        if (index > -1)
                        {
                            Delete(ref u, index);
                            filter.itemChecked = true;
                            if (u.Length > 0)
                            {
                                
                                t = true;
                                s.Add("flc=" + String.Join(",", u));
                            }
                        }
                        else
                        {
                            t = true;
                            if (u.Length > 0)
                            {
                                s.Add("flc=" + String.Join(",", u) + "," + filter.itemId);
                            }
                            else
                            {
                                s.Add("flc=" + filter.itemId);
                            }

                        };
                        if (ud.Length > 0)
                        {
                            t = true;
                            s.Add("fld=" + String.Join(",", ud));
                        }
                        if (t)
                        {
                            filter.itemLink = "/filter?" + String.Join("&", s);
                        }


                        NationalCuisines.Add(filter);
                        break;
                    case "TypeOfDishes":
                        u = ud;
                        index = Array.IndexOf(u, filter.itemId + "");
                        if (up.Length > 0)
                        {
                            t = true;
                            s.Add("flp=" + String.Join(",", up));
                        }
                        if (uc.Length > 0)
                        {
                            t = true;
                            s.Add("flc=" + String.Join(",", uc));
                        }
                        if (index > -1)
                        {
                            Delete(ref u, index);
                            filter.itemChecked = true;
                            if (u.Length > 0)
                            {
                                
                                t = true;
                                s.Add("fld=" + String.Join(",", u));
                            }
                        }
                        else
                        {
                            t = true;
                            if (u.Length > 0)
                            {
                                s.Add("fld=" + String.Join(",", u) + "," + filter.itemId);
                            }
                            else
                            {
                                s.Add("fld=" + filter.itemId);
                            }

                        };
                        if (t)
                        {
                            filter.itemLink = "/filter?" + String.Join("&", s);
                        }

                        TypeOfDishes.Add(filter);
                        break;
                }
            }
            List<Recipe> recipes = db.Recipes.ToList();
            FilterViewModel viewModel = new FilterViewModel
            {
                Recipes = recipes,
                Products = Products,
                TypeOfDishes = TypeOfDishes,
                NationalCuisines = NationalCuisines,
                //MeasuringSystems = measuringSystem
            };
            return View("Index",viewModel);
        }

        private void Delete(ref string[] u, int index)
        {
            string[] n = new string[u.Length - 1];
            for (int i = 0; i < index; i++)
            {
                n[i] = u[i];
            }
            for (int i = index; i < n.Length; i++)
            {
                n[i] = u[i + 1];
            }
            u = n;
            //throw new NotImplementedException();
        }

        [Route("/")]
        public IActionResult Index()
        {
            //IQueryable<Recipe_Product> recipe_Products = db.Recipe_Products.Include(p => p.Recipe);
            //if (id != null && id != 0)
            //{
            //    recipe_Products = recipe_Products.Where(p => p.RecipeId == id);
            //}
            //var products = db.Recipe_Products.Select(sc => sc.Product).ToList();

            //const string SqlFilterProduct = "join Recipe_Products frp on frp.RecipeId = r.RecipeId and frp.ProductId in (13, 15, 16, 17, 21) ";
            const string SqlFilterProduct = "join Recipe_Products frp on frp.RecipeId = r.RecipeId ";
            //const string SqlFilterNationalCuisines = "join Recipe_NationalCuisines frn on frn.RecipeId = r.RecipeId and frn.NationalCuisineId in (10) ";
            const string SqlFilterNationalCuisines = "join Recipe_NationalCuisines frn on frn.RecipeId = r.RecipeId ";
            //const string SqlFilterTypeOfDishes = "join Recipe_TypeOfDishes frt on frt.RecipeId = r.RecipeId and frt.TypeOfDishId in (4, 6) ";
            const string SqlFilterTypeOfDishes = "join Recipe_TypeOfDishes frt on frt.RecipeId = r.RecipeId ";
            const string SqlFilter = "SELECT Distinct " +
                "Products.ProductId as itemId, 'Products' as itemType, Products.ProductName as itemName, " +
                "COUNT(Distinct Recipe_Products.RecipeId) AS itemCount, 1 as itemSort " +
                "FROM Products JOIN Recipe_Products ON Recipe_Products.ProductId = Products.ProductId " +
                "WHERE Recipe_Products.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY Products.ProductId,Products.ProductName " +
                "UNION " +
                "SELECT Distinct " +
                "NationalCuisines.NationalCuisineId as itemId, 'NationalCuisines' as itemType, NationalCuisines.NationalCuisineName as itemName, " +
                "COUNT(Distinct Recipe_NationalCuisines.RecipeId) AS itemCount, 2 as itemSort " +
                "FROM NationalCuisines JOIN Recipe_NationalCuisines ON Recipe_NationalCuisines.NationalCuisineId = NationalCuisines.NationalCuisineId " +
                "WHERE Recipe_NationalCuisines.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY NationalCuisines.NationalCuisineId,NationalCuisines.NationalCuisineName " +
                "UNION " +
                "SELECT Distinct " +
                "TypeOfDishes.TypeOfDishId as itemId, 'TypeOfDishes' as itemType, TypeOfDishes.TypeOfDishName as itemName, " +
                "COUNT(Distinct Recipe_TypeOfDishes.RecipeId) AS itemCount, 3 as itemSort " +
                "FROM TypeOfDishes JOIN Recipe_TypeOfDishes ON Recipe_TypeOfDishes.TypeOfDishId = TypeOfDishes.TypeOfDishId " +
                "WHERE Recipe_TypeOfDishes.RecipeId in (SELECT Distinct r.RecipeId FROM Recipes r " +
                SqlFilterProduct +
                SqlFilterNationalCuisines +
                SqlFilterTypeOfDishes +
                ") GROUP BY TypeOfDishes.TypeOfDishId,TypeOfDishes.TypeOfDishName " +
                "ORDER BY itemSort, itemName;"

            ;
            List<Filter> Filters = db.Filters.FromSql(SqlFilter).ToList();
            List<Filter> Products = new List<Filter>();
            List<Filter> NationalCuisines = new List<Filter>();
            List<Filter> TypeOfDishes = new List<Filter>();
            foreach (Filter filter in Filters)
            {
                switch (filter.itemType)
                {
                    case "Products": filter.itemLink = "/filter?flp=" + filter.itemId; Products.Add(filter); break;
                    case "NationalCuisines": filter.itemLink = "/filter?flc=" + filter.itemId; NationalCuisines.Add(filter); break;
                    case "TypeOfDishes": filter.itemLink = "/filter?fld=" + filter.itemId; TypeOfDishes.Add(filter); break;
                }
            }
            List<Recipe> recipes = db.Recipes.ToList();
            FilterViewModel viewModel = new FilterViewModel
            {
                Recipes = recipes,
                Products = Products,
                TypeOfDishes = TypeOfDishes,
                NationalCuisines = NationalCuisines,
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
