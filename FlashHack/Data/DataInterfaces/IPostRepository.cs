using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();

        Task AddAsync(Post post);
        Task Update(Post post);
        Task Delete(Post post);
    }
}
