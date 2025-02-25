using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashHack.Models
{
    [Table("Posts")]
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
        
        

        public List<Comment> Comments  { get; set; } = new List<Comment>();
        public DateTime TimeCreated { get; set; }

        [Required]
        public int UserId { get; set; }       
        public User? User { get; set; }
        [Required]
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }

        public virtual List<User>? UserFavorites { get; set; } = new List<User>();
    }
}
