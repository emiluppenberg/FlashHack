using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Location { get; set; }
        public string WebbPage { get; set; }

        public List<Jobblisting> Jobblistings { get; set; }
    }
}
