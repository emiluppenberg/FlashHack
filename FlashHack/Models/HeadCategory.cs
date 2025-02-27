using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class HeadCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<SubCategory>? SubCategories { get; set; }
    }
}
