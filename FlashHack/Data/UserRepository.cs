using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(User user)
        {
            await applicationDbContext.AddAsync(user);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            applicationDbContext.Remove(user);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await applicationDbContext.User.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await applicationDbContext.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task Update(User user)
        {
            applicationDbContext.Update(user);
            await applicationDbContext.SaveChangesAsync();

        }
    }
}
