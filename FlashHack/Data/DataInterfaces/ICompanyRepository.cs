using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();

        Task AddAsync(Company company);
        Task Update(Company company);
        Task Delete(Company company);

    }
}
