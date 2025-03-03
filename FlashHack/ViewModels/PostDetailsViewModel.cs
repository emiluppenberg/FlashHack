using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; } = null!;
        public IEnumerable<Post>? Favorites { get; set; } = null!;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public string SortOrder { get; set; } = "newest";
    }
}
