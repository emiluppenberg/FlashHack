namespace FlashHack.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<HeadCategoryViewModel> HeadCategories { get; set; }
    }

    public class HeadCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryViewModel> SubCategories { get; set; }
    }

    public class SubCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PostCount { get; set; }
        public string? MostRecentPostTitle { get; set; }
        public string? MostRecentPostUser { get; set; }
        public bool HasPosts { get; set; }
        public int TotalComments { get; set; }
    }
}
