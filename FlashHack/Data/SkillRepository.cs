using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Data
{
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public SkillRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task AddAsync(Skill skill)
        {
            await applicationDbContext.AddAsync(skill);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task Delete(Skill skill)
        {
            applicationDbContext.Remove(skill);
            await applicationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            return await applicationDbContext.Skill.ToListAsync();
        }

        public async Task<Skill> GetByIdAsync(int id)
        {
            return await applicationDbContext.Skill.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task Update(Skill skill)
        {
            applicationDbContext.Update(skill);
            await applicationDbContext.SaveChangesAsync();
        }
    }
}
