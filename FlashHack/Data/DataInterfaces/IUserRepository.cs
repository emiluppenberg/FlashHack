using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();

        Task AddAsync(User user);
        Task Update(User user);
        Task Delete(User user);
        Task RemoveSkillAsync(Skill skill);

    }
}
