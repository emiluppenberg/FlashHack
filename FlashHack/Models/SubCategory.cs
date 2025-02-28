using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Post>? Posts { get; set; }

        public int HeadCategoryId { get; set; }
        public HeadCategory? HeadCategory { get; set; }

    }
}
