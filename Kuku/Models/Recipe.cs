using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class Recipe
    {
        public int Recipeid { get; set; }
        public string Name { get; set; }
        public string Discriptions { get; set; }
        public DateTime GreateDate { get; set; }
        public int OriginalImageId { get; set; }
        public byte[] BigImageData { get; set; }
        public byte[] PreviewImageData { get; set; }
        public string UserId { get; set; }
    }
}
