using System.Collections.Generic;

namespace Kuku.Models
{
    public class Recipe_Filter
    {
        public string itemType { get; set; }

        public string itemMD5 { get; set; }
        public string itemClass { get; set; }
        

        public int itemCount { get; set; }
        public int itemSort { get; set; }
        public List<Filter> items { get; set; }
 
    }
}