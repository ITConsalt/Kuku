namespace Kuku.Models
{
    public class Recipe_Filter
    {
        public int itemId { get; set; }
        public string itemName { get; set; }
        public string itemType { get; set; }
        public int itemCount { get; set; }
        public int itemSort { get; set; }

        /*
        public string itemType { get; set; }
        public int itemSort { get; set; }
        public Filter[] items { get; set; }
        */
        Recipe_Filter(Filter item)
        {
            this.itemId = item.itemId;
        }
  
    }
}