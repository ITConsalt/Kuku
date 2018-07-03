using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class RecipeDetails
    {
        public int RecipeDetailsId { get; set; }
        public int RecipeId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public byte[] PreviewImageData { get; set; }
        public byte[] BigImageData { get; set; }

        //public virtual ICollection<Recipe> Recipe { get; set; }
        //public RecipeDetails()
        //{
        //    Recipe = new List<Recipe>();
        //}
    }

}
//CREATE TABLE RecipeDetails(
//RecipeDetailsId INT IDENTITY(1,1),
//RecipeId INT NOT NULL REFERENCES Recipe(RecipeId) ON DELETE CASCADE ON UPDATE CASCADE,
//Description NVARCHAR(4000) NOT NULL,
//[OriginalImageId] int NOT NULL,
//[BigImageData] [varbinary] (MAX) NOT NULL,
//[PreviewImageData] [varbinary] (MAX) NOT NULL,
//CreatedDate DATETIME NOT NULL DEFAULT(getdate()),
//CONSTRAINT PK_RecipeDetails_RecipeDetailsId PRIMARY KEY CLUSTERED(RecipeDetailsId)
//)
//GO