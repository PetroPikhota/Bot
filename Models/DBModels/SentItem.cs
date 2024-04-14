using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bot_start.Models
{
    public class SentItem : CoreDbModels
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string ItemName { get; set; }
        public SentItem()
        {
            lastUpdate = DateTime.Now;
        }
    }
}
