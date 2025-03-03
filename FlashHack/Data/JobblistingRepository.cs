using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class JobblistingRepository : IJobblistingRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public JobblistingRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Jobblisting jobblisting)
        {
            await applicationDbContext.AddAsync(jobblisting);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Jobblisting jobblisting)
        {
            applicationDbContext.Remove(jobblisting);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Jobblisting>> GetAllAsync()
        {
            return await applicationDbContext.Jobblisting
                .Include(j => j.Company)
                .ToListAsync();
        }

        public async Task<Jobblisting> GetByIdAsync(int id)
        {
            return await applicationDbContext.Jobblisting
                .Include(j => j.Company)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task Update(Jobblisting jobblisting)
        {
            applicationDbContext.Update(jobblisting);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Jobblisting>> FindAllWithTitle(string title)
        {
            return await applicationDbContext.Jobblisting
                .Include(j => j.Company)
                .Where(j => j.Title.ToLower()
                    .Contains(title.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Jobblisting>> FindAllWithLocation(string location)
        {
            return await applicationDbContext.Jobblisting
                .Include(j => j.Company)
                .Where(j => j.Company.Location.ToLower()
                    .Contains(location.ToLower()))
                .ToListAsync();
        }
    }
}
