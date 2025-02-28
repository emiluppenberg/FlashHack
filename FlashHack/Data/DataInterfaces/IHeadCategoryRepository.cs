using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IHeadCategoryRepository
    {
        Task<HeadCategory> GetByIdAsync(int id);
        Task<IEnumerable<HeadCategory>> GetAllAsync();

        Task AddAsync(HeadCategory headCategory);
        Task Update(HeadCategory headCategory);
        Task Delete(HeadCategory headCategory);
    }
}
