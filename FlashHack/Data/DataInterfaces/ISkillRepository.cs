using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface ISkillRepository
    {
        Task<Skill> GetByIdAsync(int id);
        Task<IEnumerable<Skill>> GetAllAsync();

        Task AddAsync(Skill skill);
        Task Update(Skill skill);
        Task Delete(Skill skill);
    }
}
