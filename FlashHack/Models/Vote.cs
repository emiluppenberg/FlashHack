using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace FlashHack.Models
{
    public class Vote
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; } = null!;

        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }

        public int? CommentId { get; set; }
        public virtual Comment? Comment { get; set; }

        [Required]
        public bool IsUpVote { get; set; } = false;
        [Required]
        public bool IsDownVote { get; set; } = false;
    }
}
