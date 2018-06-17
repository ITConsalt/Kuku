using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kuku.Models
{
    public class OriginalImageViewModel
    {
        public string FileName { get; set; }
        public IFormFile OriginalImageData { get; set; }
    }
}
