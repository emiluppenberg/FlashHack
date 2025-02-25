using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Comment comment)
        {
            await applicationDbContext.AddAsync(comment);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Comment comment)
        {
            applicationDbContext.Remove(comment);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await applicationDbContext.Comment.ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetAllFromPostIdAsync(int? postId)
        {
            var comments = await applicationDbContext.Comment
                .Include(c => c.Post)
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .OrderBy(c => c.TimeCreated)
                .ToListAsync();
                                             
            return (comments);
        }

        public async Task<Comment> GetByIdAsync(int id)
        {
            return await applicationDbContext.Comment.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Comment comment)
        {
            applicationDbContext.Update(comment);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
