using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CompanyRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Company company)
        {
            await applicationDbContext.AddAsync(company);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Company company)
        {
            applicationDbContext.Remove(company);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await applicationDbContext.Company.ToListAsync();
        }

        public async Task<Company> GetByIdAsync(int id)
        {
            return await applicationDbContext.Company.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Company company)
        {
            applicationDbContext.Update(company);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
