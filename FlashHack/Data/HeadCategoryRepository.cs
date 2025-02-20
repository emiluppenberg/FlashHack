using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class HeadCategoryRepository : IHeadCategoryRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public HeadCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(HeadCategory headCategory)
        {
            await applicationDbContext.AddAsync(headCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(HeadCategory headCategory)
        {
            applicationDbContext.Remove(headCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<HeadCategory>> GetAllAsync()
        {
            return await applicationDbContext.HeadCategory.ToListAsync();
        }

        public async Task<HeadCategory> GetByIdAsync(int id)
        {
            return await applicationDbContext.HeadCategory.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task Update(HeadCategory headCategory)
        {
            applicationDbContext.Update(headCategory);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
