using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.EntityFrameworkCore;

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
                    .FirstOrDefaultAsync(p => p.Id == vote.PostId);

                var userVote = post.Votes.Find(v => v.UserId == vote.UserId);

                if (userVote != null)
                {
                    if (userVote.IsUpVote && vote.IsUpVote) return null;

                    if (userVote.IsDownVote && vote.IsDownVote) return null;

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
                    .FirstOrDefaultAsync(c => c.Id == vote.PostId);

                var userVote = comment.Votes.Find(v => v.UserId == vote.UserId);

                if (userVote != null)
                {
                    if (userVote.IsUpVote && vote.IsUpVote) return null;

                    if (userVote.IsDownVote && vote.IsDownVote) return null;

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

                    comment.UpVotes += vote.IsUpVote ? 1 : 0;
                    comment.DownVotes += vote.IsDownVote ? 1 : 0;

                    await applicationDbContext.SaveChangesAsync();

                    return vote.IsUpVote ? "Upvote added" : "Downvote added";
                }
            }

            return null;
        }
    }
}
