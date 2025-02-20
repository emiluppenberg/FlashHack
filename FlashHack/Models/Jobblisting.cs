using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class Jobblisting
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }



    }
}
