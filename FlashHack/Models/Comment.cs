﻿using System.ComponentModel.DataAnnotations;

namespace FlashHack.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }
        public DateTime TimeCreated { get; set; }

        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
