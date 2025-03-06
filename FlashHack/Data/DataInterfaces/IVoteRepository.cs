using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface IVoteRepository
    {
        Task<string?> AddAsync(Vote vote);
        Task UpdateUserRatings();
    }
}
