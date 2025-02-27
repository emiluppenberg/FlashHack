using AspNetCoreGeneratedDocument;
using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class PostsIndexViewModel
    {
        public IEnumerable<Post> Posts { get; set; } = null!;
        public IEnumerable<Post>? Favorites { get; set; }
    }
}
