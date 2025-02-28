using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
