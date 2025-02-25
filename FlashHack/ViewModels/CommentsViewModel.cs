using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class CommentsViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
