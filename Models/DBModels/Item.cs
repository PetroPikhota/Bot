using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot_start.Models
{
    public class Item : CoreDbModels
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        public string Path { get; set; }
        public Item(string path)
        {
            Path = path;
        }
        public Item()
        {
            lastUpdate = DateTime.UtcNow;
        }
    }
}
