using Microsoft.EntityFrameworkCore;
using FlashHack.Models;

namespace FlashHack.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()  
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments) 
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Favorites)
                .WithMany(f => f.UserFavorites)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPostFavorites",
                    j => j.HasOne<Post>().WithMany().HasForeignKey("PostId").OnDelete(DeleteBehavior.Restrict),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict));

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<FlashHack.Models.User> User { get; set; } = default!;
        public DbSet<FlashHack.Models.HeadCategory> HeadCategory { get; set; } = default!;
        public DbSet<FlashHack.Models.SubCategory> SubCategory { get; set; } = default!;
        public DbSet<FlashHack.Models.Skill> Skill { get; set; } = default!;
        public DbSet<FlashHack.Models.Post> Post { get; set; } = default!;
        public DbSet<FlashHack.Models.Comment> Comment { get; set; } = default!;
        public DbSet<FlashHack.Models.Company> Company{ get; set; } = default!;
        public DbSet<FlashHack.Models.Jobblisting> Jobblisting { get; set; } = default!;

    }
}

