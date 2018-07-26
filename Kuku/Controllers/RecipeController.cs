using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Kuku.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.Primitives;

namespace Kuku.Controllers
{
    [Authorize(Roles = "admin")]
    public class RecipeController : Controller
    {
        private readonly UserManager<User> _userManager;
        IHostingEnvironment _appEnvironment;

        private EFContext db;
        public RecipeController(EFContext context, UserManager<User> userManager, IConfiguration configuration, IHostingEnvironment appEnvironment)

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

        [HttpGet]
        public IActionResult AddProduct(int? recipeid, int? productid)
        {
            if (recipeid != null)
            {
                Recipe_Product recipe_Product = db.Recipe_Products.FirstOrDefault(p => p.RecipeId == recipeid);
                if (recipe_Product != null)
                    return View(recipe_Product);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Recipe_Product recipe_Product)
        {
            ///вот сюда чёта добавить, чтобы всё работало
            db.Recipe_Products.Add(recipe_Product);
            await db.SaveChangesAsync();
            return View();
        }

        public IActionResult CreateNationalCuisine()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNationalCuisine(NationalCuisine NationalCuisine)
        {
            db.NationalCuisines.Add(NationalCuisine);
            await db.SaveChangesAsync();
            return RedirectToAction("NationalCuisine");
        }

        [HttpGet]
        public ActionResult CreateProduct()
        {
            // Формируем список команд для передачи в представление
            SelectList productTypes = new SelectList(db.ProductTypes, "ProductTypeId", "ProductTypeName");
            ViewBag.ProductTypes = productTypes;
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            //Добавляем игрока в таблицу
            db.Products.Add(product);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("Product");
        }

        public IActionResult CreateProductType()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductType(ProductType ProductType)
        {
            db.ProductTypes.Add(ProductType);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductType");
        }

        public ActionResult CreateRecipe()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRecipe(IFormFile uploadedFile, SP_Recipe sp_Recipe)
        {
            // string connectionString = Configuration.GetConnectionString("DefaultConnection");
            // название процедуры
            //string sqlExpression = "SP_Product";

            using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
            {
                // Sp_recipe file = new Sp_recipe { FileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1) };
                string shortFileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                SP_Recipe file = new SP_Recipe { FileName = shortFileName };

                Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Temp/");
                // путь к папке Temp
                string path = _appEnvironment.WebRootPath + "/Temp/";

                if (uploadedFile != null)
                {
                    // сохраняем файл в папку Temp в каталоге wwwroot
                    using (var fileStream = new FileStream(path + shortFileName, FileMode.Create))
                    {
                        uploadedFile.CopyTo(fileStream);
                    }

                    using (var img = Image.Load(path + shortFileName))
                    {
                        // as generate returns a new IImage make sure we dispose of it
                        using (Image<Rgba32> destRound = img.Clone(x => x.Resize(new Size(590, 0))))
                        {
                            destRound.Save(path + "bigImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                        }

                        using (Image<Rgba32> destRound = img.Clone(x => x.Resize(new Size(320, 0))))
                        {
                            destRound.Save(path + "previewImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                        }
                    }

                    byte[] bigImageData = System.IO.File.ReadAllBytes(path + "bigImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                    file.BigImageData = bigImageData;

                    byte[] previewImageData = System.IO.File.ReadAllBytes(path + "previewImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                    file.PreviewImageData = previewImageData;

                    byte[] originalImageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                    {
                        originalImageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                    }
                    // установка массива байтов
                    file.OriginalImageData = originalImageData;

                    Directory.Delete(path, true);
                }

                connection.Open();
                SqlCommand command = new SqlCommand("SP_Recipe", connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                //string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);
                SqlParameter fileNameParam = new SqlParameter
                {
                    ParameterName = "@FileName",
                    Value = file.FileName
                };
                // добавляем параметр
                command.Parameters.Add(fileNameParam);
                // параметр для ввода возраста
                SqlParameter originalImageDataParam = new SqlParameter
                {
                    ParameterName = "@OriginalImageData",
                    Value = file.OriginalImageData
                };
                // добавляем параметр
                command.Parameters.Add(originalImageDataParam);

                SqlParameter recipeNameParam = new SqlParameter
                {
                    ParameterName = "@RecipeName",
                    Value = sp_Recipe.RecipeName
                };
                // добавляем параметр
                command.Parameters.Add(recipeNameParam);

                SqlParameter DescriptionParam = new SqlParameter
                {
                    ParameterName = "@Description",
                    Value = sp_Recipe.Description
                };
                // добавляем параметр
                command.Parameters.Add(DescriptionParam);

                SqlParameter bigImageDataParam = new SqlParameter
                {
                    ParameterName = "@BigImageData",
                    Value = file.BigImageData
                };
                // добавляем параметр
                command.Parameters.Add(bigImageDataParam);

                SqlParameter previewImageDataParam = new SqlParameter
                {
                    ParameterName = "@PreviewImageData",
                    Value = file.PreviewImageData
                };
                // добавляем параметр
                command.Parameters.Add(previewImageDataParam);

                SqlParameter userIdParam = new SqlParameter
                {
                    ParameterName = "@UserId",
                    Value = _userManager.GetUserId(HttpContext.User)
                };
                // добавляем параметр
                command.Parameters.Add(userIdParam);

                //var result = command.ExecuteScalar();
                // если нам не надо возвращать id
                command.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToAction("Index");
        }

        public ActionResult CreateRecipeDetail()
        {
            return View(/*await db.Recipe.ToListAsync()*/);
        }

        [HttpPost]
        public IActionResult CreateRecipeDetail(IFormFile uploadedFile, SP_RecipeDetails sp_RecipeDetails, int? id)
        {
            if (id != null)
            {
                // string connectionString = Configuration.GetConnectionString("DefaultConnection");
                // название процедуры
                //string sqlExpression = "SP_Product";

                using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    // Sp_recipe file = new Sp_recipe { FileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1) };
                    string shortFileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                    SP_RecipeDetails file = new SP_RecipeDetails { FileName = shortFileName };

                    Directory.CreateDirectory(_appEnvironment.WebRootPath + "/Temp/");
                    // путь к папке Temp
                    string path = _appEnvironment.WebRootPath + "/Temp/";

                    if (uploadedFile != null)
                    {
                        // сохраняем файл в папку Temp в каталоге wwwroot
                        using (var fileStream = new FileStream(path + shortFileName, FileMode.Create))
                        {
                            uploadedFile.CopyTo(fileStream);
                        }

                        using (var img = Image.Load(path + shortFileName))
                        {
                            // as generate returns a new IImage make sure we dispose of it
                            using (Image<Rgba32> destRound = img.Clone(x => x.Resize(new Size(590, 0))))
                            {
                                destRound.Save(path + "bigImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                            }

                            using (Image<Rgba32> destRound = img.Clone(x => x.Resize(new Size(320, 0))))
                            {
                                destRound.Save(path + "previewImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                            }
                        }

                        byte[] bigImageData = System.IO.File.ReadAllBytes(path + "bigImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                        file.BigImageData = bigImageData;

                        byte[] previewImageData = System.IO.File.ReadAllBytes(path + "previewImage_" + _userManager.GetUserName(HttpContext.User) + "_" + shortFileName);
                        file.PreviewImageData = previewImageData;

                        byte[] originalImageData = null;
                        // считываем переданный файл в массив байтов
                        using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                        {
                            originalImageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                        }
                        // установка массива байтов
                        file.OriginalImageData = originalImageData;

                        Directory.Delete(path, true);
                    }
                    connection.Open();
                    SqlCommand command = new SqlCommand("SP_RecipeDetails", connection)
                    {
                        // указываем, что команда представляет хранимую процедуру
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    // параметр для ввода имени
                    //string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);
                    SqlParameter fileNameParam = new SqlParameter
                    {
                        ParameterName = "@FileName",
                        Value = file.FileName
                    };
                    // добавляем параметр
                    command.Parameters.Add(fileNameParam);
                    // параметр для ввода возраста
                    SqlParameter originalImageDataParam = new SqlParameter
                    {
                        ParameterName = "@OriginalImageData",
                        Value = file.OriginalImageData
                    };
                    // добавляем параметр
                    command.Parameters.Add(originalImageDataParam);
                    SqlParameter DescriptionParam = new SqlParameter
                    {
                        ParameterName = "@DescriptionRD",
                        Value = sp_RecipeDetails.DescriptionRD
                    };
                    // добавляем параметр
                    command.Parameters.Add(DescriptionParam);
                    SqlParameter bigImageDataParam = new SqlParameter
                    {
                        ParameterName = "@BigImageData",
                        Value = file.BigImageData
                    };
                    // добавляем параметр
                    command.Parameters.Add(bigImageDataParam);
                    SqlParameter previewImageDataParam = new SqlParameter
                    {
                        ParameterName = "@PreviewImageData",
                        Value = file.PreviewImageData
                    };
                    // добавляем параметр
                    command.Parameters.Add(previewImageDataParam);
                    SqlParameter RecipeIdParam = new SqlParameter
                    {
                        ParameterName = "@RecipeId",
                        Value = id

                    };
                    // добавляем параметр
                    command.Parameters.Add(RecipeIdParam);
                    //var result = command.ExecuteScalar();
                    // если нам не надо возвращать id
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return Ok("Recipe details added");
            }
            return NotFound();
        }

        public IActionResult CreateTypeOfDish()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTypeOfDish(TypeOfDish TypeOfDish)
        {
            db.TypeOfDishes.Add(TypeOfDish);
            await db.SaveChangesAsync();
            return RedirectToAction("TypeOfDish");
        }


    }
}