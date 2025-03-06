using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FlashHack.Data
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public VoteRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<string?> AddAsync(Vote vote)
        {
            if (vote.PostId != null)
            {
                var post = await applicationDbContext.Post
                    .Include(p => p.Votes)
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == vote.PostId);

                var userVote = post.Votes.Find(v => v.UserId == vote.UserId);

                if (userVote != null)
                {
                    if (userVote.IsUpVote && vote.IsUpVote)
                    {
                        post.UpVotes--;
                        post.User.Rating--;
                        applicationDbContext.Remove(userVote);
                        await applicationDbContext.SaveChangesAsync();
                        return null;
                    }

                    if (userVote.IsDownVote && vote.IsDownVote)
                    {
                        post.DownVotes--;
                        post.User.Rating++;
                        applicationDbContext.Remove(userVote);
                        await applicationDbContext.SaveChangesAsync();
                        return null;
                    }

                    post.User.Rating += vote.IsUpVote ? 1 : -1;

                    userVote.IsUpVote = vote.IsUpVote;
                    userVote.IsDownVote = vote.IsDownVote;

                    post.UpVotes = post.Votes.Count(v => v.IsUpVote);
                    post.DownVotes = post.Votes.Count(v => v.IsDownVote);

                    await applicationDbContext.SaveChangesAsync();

                    return userVote.IsUpVote ? "Changed to upvote" : "Changed to downvote";
                }

                if (userVote == null)
                {
                    post.Votes.Add(vote);

                    post.User.Rating += vote.IsUpVote ? 1 : -1;

                    post.UpVotes += vote.IsUpVote ? 1 : 0;
                    post.DownVotes += vote.IsDownVote ? 1 : 0;

                    await applicationDbContext.SaveChangesAsync();

                    return vote.IsUpVote ? "Upvote added" : "Downvote added";
                }
            }

            if (vote.CommentId != null)
            {
                var comment = await applicationDbContext.Comment
                    .Include(c => c.Votes)
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == vote.CommentId);

                var userVote = comment.Votes.Find(v => v.UserId == vote.UserId);

                if (userVote != null)
                {
                    if (userVote.IsUpVote && vote.IsUpVote)
                    {
                        comment.UpVotes--;
                        comment.User.Rating--;
                        applicationDbContext.Remove(userVote);
                        await applicationDbContext.SaveChangesAsync();
                        return null;
                    }

                    if (userVote.IsDownVote && vote.IsDownVote)
                    {
                        comment.DownVotes--;
                        comment.User.Rating++;
                        applicationDbContext.Remove(userVote);
                        await applicationDbContext.SaveChangesAsync();
                        return null;
                    }

                    comment.User.Rating += vote.IsUpVote ? 1 : -1;

                    userVote.IsUpVote = vote.IsUpVote;
                    userVote.IsDownVote = vote.IsDownVote;

                    comment.UpVotes = comment.Votes.Count(v => v.IsUpVote);
                    comment.DownVotes = comment.Votes.Count(v => v.IsDownVote);

                    await applicationDbContext.SaveChangesAsync();

                    return userVote.IsUpVote ? "Changed to upvote" : "Changed to downvote";
                }

                if (userVote == null)
                {
                    comment.Votes.Add(vote);

                    comment.User.Rating += vote.IsUpVote ? 1 : -1;

                    comment.UpVotes += vote.IsUpVote ? 1 : 0;
                    comment.DownVotes += vote.IsDownVote ? 1 : 0;

                    await applicationDbContext.SaveChangesAsync();

                    return vote.IsUpVote ? "Upvote added" : "Downvote added";
                }
            }

            return null;
        }

        //For testing purposes
        public async Task UpdateUserRatings()
        {
            var users = await applicationDbContext.User.ToListAsync();

            foreach (var u in users)
            {
                u.Rating = 0;
            }

            var votes = await applicationDbContext.Vote
                .Include(v => v.User)
                .Include(v => v.Post)
                .Include(v => v.Comment)
                .ToListAsync();

            foreach (var v in votes)
            {
                if (v.Post != null)
                {
                    v.Post.User.Rating += v.IsUpVote ? 1 : -1;
                }

                if (v.Comment != null)
                {
                    v.Comment.User.Rating += v.IsUpVote ? 1 : -1;
                }
            }

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
