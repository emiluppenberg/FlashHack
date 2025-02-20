using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Employer { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(5)]
        public string Password { get; set; }
        [MaxLength(500)]
        public string Bio { get; set; }
        public List<Skill> Skills { get; set; }
        public string ProfilePicURL { get; set; }
        public int Rating { get; set; }
        [MaxLength(20)]
        public string Signature { get; set; }

        public List<Post> Favorites { get; set; }

        public bool IsPremium { get; set; }
        public bool IsAdmin { get; set; }
        public bool ShowEmail { get; set; } = true;
        public bool ShowPhoneNumber { get; set; } = true;
        public bool ShowEmployer { get; set; } = true;
        public bool ShowToRecruiter { get; set; } = true;
        public bool ShowRating { get; set; } = true;

    }
}
