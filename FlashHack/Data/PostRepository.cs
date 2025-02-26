﻿using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public PostRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Post post)
        {
            await applicationDbContext.AddAsync(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Post post)
        {
            applicationDbContext.Remove(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await applicationDbContext.Post.ToListAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await applicationDbContext.Post.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task Update(Post post)
        {
            applicationDbContext.Update(post);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<Post> GetByIdAndIncludeAsync(int id)
        {
            return await applicationDbContext.Post
                .Include(x => x.User)
                .Include(x => x.SubCategory)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Post>> GetUserFavorites(int userId)
        {
            return await applicationDbContext.Post
                .Where(p => p.UserFavorites.Any(u => u.Id == userId))
                .Include(p => p.User)
                .ToListAsync();
        }
    }
}
