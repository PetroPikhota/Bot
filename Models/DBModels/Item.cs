using System.ComponentModel.DataAnnotations;

namespace Bot_start.Models
{
    public class Item
    {
        [Key]
        public string Path { get; set; }
        public Item(string path)
        {
            Path = path;
        }
    }
}
