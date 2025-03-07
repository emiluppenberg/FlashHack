﻿using FlashHack.Models;

namespace FlashHack.Data.DataInterfaces
{
    public interface ICommentRepository
    {
        Task<Comment> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetAllAsync();

        Task<IEnumerable<Comment>> GetAllFromPostIdAsync(int? postId);
        Task AddAsync(Comment comment);
        Task Update(Comment comment);
        Task Delete(Comment comment);
    }
}
