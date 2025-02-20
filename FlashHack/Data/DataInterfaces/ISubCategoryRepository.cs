using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface ISubCategoryRepository
    {
        Task<SubCategory> GetByIdAsync(int id);
        Task<IEnumerable<SubCategory>> GetAllAsync();

        Task AddAsync(SubCategory subCategory);
        Task Update(SubCategory subCategory);
        Task Delete(SubCategory subCategory);
    }
}
