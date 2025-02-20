using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SubCategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(SubCategory subCategory)
        {
            await applicationDbContext.AddAsync(subCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(SubCategory subCategory)
        {
            applicationDbContext.Remove(subCategory);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            return await applicationDbContext.SubCategory.ToListAsync();
        }

        public async Task<SubCategory> GetByIdAsync(int id)
        {
            return await applicationDbContext.SubCategory.FirstOrDefaultAsync( s => s.Id == id);
        }

        public async Task Update(SubCategory subCategory)
        {
            applicationDbContext.Update(subCategory);
            await applicationDbContext.SaveChangesAsync();

        }
    }
}
