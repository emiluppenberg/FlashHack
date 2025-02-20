using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        
        

        public List<Comment> Comments  { get; set; }
        public DateTime TimeCreated { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
