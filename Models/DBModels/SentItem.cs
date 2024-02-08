using System.ComponentModel.DataAnnotations;

namespace Bot_start.Models
{
    public class SentItem
    {
        [Key]
        public long Id { get; set; }
        public string ItemName { get; set; }
    }
}
