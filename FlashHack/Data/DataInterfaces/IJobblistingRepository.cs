using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IJobblistingRepository
    {
        Task<Jobblisting> GetByIdAsync(int id);
        Task<IEnumerable<Jobblisting>> GetAllAsync();

        Task AddAsync(Jobblisting jobblisting);
        Task Update(Jobblisting jobblisting);
        Task Delete(Jobblisting jobblisting);
    }
}
