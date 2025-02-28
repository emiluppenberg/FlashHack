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
        public virtual List<Vote> Votes { get; set; } = new List<Vote>();

        public List<Comment> Comments  { get; set; } = new List<Comment>();
        public DateTime TimeCreated { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User? User { get; set; } = null!;
        [Required]
        public int SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }

        public virtual List<User>? UserFavorites { get; set; } = new List<User>();
    }
}
