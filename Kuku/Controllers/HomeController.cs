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

namespace Kuku.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;

        private EFContext db;
        public HomeController(EFContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            db = context;
            _userManager = userManager;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task<IActionResult> Index()
        {
            return View(await db.Recipe.ToListAsync());
        }

     /*   public ActionResult Index()
        {
            return View();
        }
     */
        public async Task<IActionResult> NationalityCuisine()
        {
            return View(await db.NationalityCuisine.ToListAsync());
        }

        public IActionResult CreateNationalityCuisine()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateNationalityCuisine(NationalityCuisine NationalityCuisine)
        {
            db.NationalityCuisine.Add(NationalityCuisine);
            await db.SaveChangesAsync();
            return RedirectToAction("NationalityCuisine");
        }
        public async Task<IActionResult> DetailsNationalityCuisine(int? id)
        {
            if (id != null)
            {
                NationalityCuisine nationalityCuisine = await db.NationalityCuisine.FirstOrDefaultAsync(p => p.NationalityCuisineId == id);
                if (nationalityCuisine != null)
                    return View(nationalityCuisine);
            }
            return NotFound();
        }
        [HttpGet]
        [ActionName("DeleteNationalityCuisine")]
        public async Task<IActionResult> ConfirmDeleteNationalityCuisine(int? id)
        {
            if (id != null)
            {
                NationalityCuisine nationalityCuisine = await db.NationalityCuisine.FirstOrDefaultAsync(p => p.NationalityCuisineId == id);
                if (nationalityCuisine != null)
                    return View(nationalityCuisine);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNationalityCuisine(int? id)
        {
            if (id != null)
            {
                /*NationalityCuisine nationalityCuisine = await db.NationalityCuisine.FirstOrDefaultAsync(p => p.NationalityCuisineId == id);
                if (nationalityCuisine != null)
                {
                    db.NationalityCuisine.Remove(nationalityCuisine);
                    await db.SaveChangesAsync();
                    return RedirectToAction("NationalityCuisine");
                }*/
                NationalityCuisine nationalityCuisine = new NationalityCuisine { NationalityCuisineId = id.Value };
                db.Entry(nationalityCuisine).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("NationalityCuisine");//в отличии от предыдущего, этот метод - оптимизированный и с проверкой на существование записи в БД
            }
            return NotFound();
        }
        public async Task<IActionResult> EditNationalityCuisine(int? id)
        {
            if (id != null)
            {
                NationalityCuisine nationalityCuisine = await db.NationalityCuisine.FirstOrDefaultAsync(p => p.NationalityCuisineId == id);
                if (nationalityCuisine != null)
                    return View(nationalityCuisine);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditNationalityCuisine(NationalityCuisine cuisine)
        {
            db.NationalityCuisine.Update(cuisine);
            await db.SaveChangesAsync();
            return RedirectToAction("NationalityCuisine");
        }

        public async Task<IActionResult> ProductType()
        {
            return View(await db.ProductType.ToListAsync());
        }

        public IActionResult CreateProductType()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductType(ProductType ProductType)
        {
            db.ProductType.Add(ProductType);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductType");
        }
        public async Task<IActionResult> DetailsProductType(int? id)
        {
            if (id != null)
            {
                ProductType productType = await db.ProductType.FirstOrDefaultAsync(p => p.ProductTypeId == id);
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
                ProductType productType = await db.ProductType.FirstOrDefaultAsync(p => p.ProductTypeId == id);
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
                ProductType productType = await db.ProductType.FirstOrDefaultAsync(p => p.ProductTypeId == id);
                if (productType != null)
                    return View(productType);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditProductType(ProductType type)
        {
            db.ProductType.Update(type);
            await db.SaveChangesAsync();
            return RedirectToAction("ProductType");
        }

        public async Task<IActionResult> Product()
        {
            return View(await db.Product.ToListAsync());
        }
        [HttpGet]
        public ActionResult CreateProduct()
        {
            // Формируем список команд для передачи в представление
            SelectList productTypes = new SelectList(db.ProductType, "ProductTypeId", "ProductTypeName");
            ViewBag.ProductTypes = productTypes;
            return View();
        }
        [HttpPost]
        public ActionResult CreateProduct(Product product)
        {
            //Добавляем игрока в таблицу
            db.Product.Add(product);
            db.SaveChanges();
            // перенаправляем на главную страницу
            return RedirectToAction("Product");
        }
        public async Task<IActionResult> DetailsProduct(int? id)
        {
            if (id != null)
            {
                Product product = await db.Product.FirstOrDefaultAsync(p => p.ProductId == id);
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
                Product product = await db.Product.FirstOrDefaultAsync(p => p.ProductId == id);
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
            return View(await db.TypeOfDish.ToListAsync());
        }
        public IActionResult CreateTypeOfDish()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTypeOfDish(TypeOfDish TypeOfDish)
        {
            db.TypeOfDish.Add(TypeOfDish);
            await db.SaveChangesAsync();
            return RedirectToAction("TypeOfDish");
        }
        public async Task<IActionResult> DetailsTypeOfDish(int? id)
        {
            if (id != null)
            {
                TypeOfDish typeOfDish = await db.TypeOfDish.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
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
                TypeOfDish typeOfDish = await db.TypeOfDish.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
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
                TypeOfDish typeOfDish = await db.TypeOfDish.FirstOrDefaultAsync(p => p.TypeOfDishId == id);
                if (typeOfDish != null)
                    return View(typeOfDish);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditTypeOfDish(TypeOfDish dish)
        {
            db.TypeOfDish.Update(dish);
            await db.SaveChangesAsync();
            return RedirectToAction("TypeOfDish");
        }

    // Add Image: (https://www.metanit.com/sharp/aspnet5/21.3.php)
        public async Task<IActionResult> AddImage()
        {
            return View(await db.OriginalImage.ToListAsync());
        }

        [HttpPost]
        public IActionResult AddImage(IFormFile uploadedFile)
        {
            OriginalImage originalImage = new OriginalImage
            {
                //string shortFileName = filename.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1);
                FileName = uploadedFile.FileName
            };
            if (uploadedFile != null)
            {
                byte[] imageData = null;
                //считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                }
                //установка массива байтов
                originalImage.OriginalImageData = imageData;
            }
            db.OriginalImage.Add(originalImage);
            db.SaveChanges();

            return RedirectToAction("AddImage");
        }

        [HttpGet]
        [ActionName("DeleteOriginalImage")]
        public async Task<IActionResult> ConfirmDeleteOriginalImage(int? id)
        {
            if (id != null)
            {
                OriginalImage originalImage = await db.OriginalImage.FirstOrDefaultAsync(p => p.OriginalImageId == id);
                if (originalImage != null)
                    return View(originalImage);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOriginalImage(int? id)
        {
            if (id != null)
            {

                OriginalImage originalImage = new OriginalImage { OriginalImageId = id.Value };
                db.Entry(originalImage).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("AddImage");
            }
            return NotFound();
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
                SP_Recipe uploadFile = new SP_Recipe { FileName = uploadedFile.FileName };
                string shortFileName = uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf('\\') + 1);
                SP_Recipe file = new SP_Recipe { FileName = shortFileName };
                if (uploadedFile != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(uploadedFile.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)uploadedFile.Length);
                    }
                    // установка массива байтов
                    file.OriginalImageData = imageData;
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
                    Value = file.OriginalImageData
                };
                // добавляем параметр
                command.Parameters.Add(bigImageDataParam);

                SqlParameter previewImageDataParam = new SqlParameter
                {
                    ParameterName = "@PreviewImageData",
                    Value = file.OriginalImageData
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
