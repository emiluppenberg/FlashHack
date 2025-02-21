using FlashHack.Models;

namespace FlashHack.ViewModels
{
    public class NavBarViewModel
    {
        public List<HeadCategory> HeadCategories { get; set; }
        public int? CurrentSubCategoryId { get; set; }
    }
}
