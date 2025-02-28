using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlashHack.Models
{
    [Table("User")]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string? Employer { get; set; } = string.Empty;
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^(\+46|0)[0-9]{7,10}$", ErrorMessage = "Please enter a valid Swedish phone number.")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
        [MaxLength(500)]
        public string? Bio { get; set; } = string.Empty;
        public List<Skill>? Skills { get; set; } = new List<Skill>();
        public string? ProfilePicURL { get; set; } = string.Empty;
        public int? Rating { get; set; } = 0;
        [MaxLength(20)]
        public string? Signature { get; set; } = string.Empty;

        public virtual List<Post>? Posts { get; set; } = new List<Post>();
        public virtual List<Post>? Favorites { get; set; } = new List<Post>();

        public bool IsPremium { get; set; }
        public bool IsAdmin { get; set; }
        public bool ShowEmail { get; set; } = true;
        public bool ShowPhoneNumber { get; set; } = true;
        public bool ShowEmployer { get; set; } = true;
        public bool ShowToRecruiter { get; set; } = true;
        public bool ShowRating { get; set; } = true;

    }
}
