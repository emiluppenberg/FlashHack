using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IPostRepository
    {
        Task<Post> GetByIdAsync(int id);
        Task<Post> GetByIdAndIncludeAsync(int id);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<IEnumerable<Post>> GetUserFavorites(int userId);
        Task AddAsync(Post post);
        Task Update(Post post);
        Task Delete(Post post);
    }
}
