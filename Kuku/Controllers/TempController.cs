/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Kuku.Controllers
{
    public class TempController : Controller
    {
        [HttpPost]
        public IActionResult CreateRecipeDetails(IFormFile uploadedFile, SP_RecipeDetails sp_RecipeDetails, int? id)
        {
            if (id != null)
            {
                // string connectionString = Configuration.GetConnectionString("DefaultConnection");
                // название процедуры
                //string sqlExpression = "SP_Product";

                using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection")))
                {
                    SP_RecipeDetails file = new SP_RecipeDetails { FileName = uploadedFile.FileName.Substring(uploadedFile.FileName.LastIndexOf('\\') + 1) };
                    //string shortFileName = uploadFile.FileName.Substring(uploadFile.FileName.LastIndexOf('\\') + 1);
                    //SP_Recipe file = new SP_Recipe { FileName = shortFileName };
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
                        ParameterName = "@Description",
                        Value = sp_RecipeDetails.DescriptionRD
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
                    //public async task<iactionresult> edittypeofdish(int? id)
                    //    if (id != null)
                    //{
                    //    {
                    //        typeofdish typeofdish = await db.typeofdish.firstordefaultasync(p => p.typeofdishid == id);
                    //        if (typeofdish != null)
                    //            return view(typeofdish);
                    //    }
                    //    return notfound();
                    ///
                    SqlParameter RecipeIdParam = new SqlParameter
                    {
                        ParameterName = "@RecipeId",
                        Value = _userManager.GetUserId(HttpContext.User)

                    };
                    // добавляем параметр
                    command.Parameters.Add(RecipeIdParam);

                    //var result = command.ExecuteScalar();
                    // если нам не надо возвращать id
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}*/