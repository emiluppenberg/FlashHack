using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; } = null!;
        public IEnumerable<Post>? Favorites { get; set; } = null!;
    }
}
