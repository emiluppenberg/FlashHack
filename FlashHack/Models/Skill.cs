using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(40)]
        public string SkillName { get; set; }
        [Required]
        [MaxLength(255)]
        public string SkillDescription { get; set; }

        [Required]
        [Range(1,5)]
        public int SkillRating { get; set; }

    }
}
