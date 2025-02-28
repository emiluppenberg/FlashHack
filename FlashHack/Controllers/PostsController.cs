using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashHack.Data;
using FlashHack.Models;
using FlashHack.Data.DataInterfaces;
using FlashHack.ViewModels;

namespace FlashHack.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository postRepository;
        private readonly ISubCategoryRepository subCategoryRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IVoteRepository voteRepository;

        public PostsController(ApplicationDbContext context, IPostRepository postRepository, ISubCategoryRepository subCategoryRepository, IUserRepository userRepository, ICommentRepository commentRepository, IVoteRepository voteRepository)
        {
            _context = context;
            this.postRepository = postRepository;
            this.subCategoryRepository = subCategoryRepository;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.voteRepository = voteRepository;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? subCategoryId, string sortOrder)
        {
            IQueryable<Post> postsQuery = _context.Post
                .Include(p => p.SubCategory)
                .Include(p => p.User)
                .Include(p => p.Comments);

            if (subCategoryId != null)
            {
                postsQuery = postsQuery.Where(p => p.SubCategoryId == subCategoryId);
                var subCategory = await _context.SubCategory.FindAsync(subCategoryId);
                if (subCategory == null)
                {
                    return NotFound();
                }
                ViewData["SubCategoryName"] = subCategory.Name;
            }
            else
            {
                ViewData["SubCategoryName"] = "All Posts";
            }

            ViewData["CurrentSortOrder"] = sortOrder;

            switch (sortOrder)
            {
                case "newest":
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
                case "oldest":
                    postsQuery = postsQuery.OrderBy(p => p.TimeCreated);
                    break;
                case "mostLiked":
                    postsQuery = postsQuery.OrderByDescending(p => p.UpVotes);
                    break;
                case "leastLiked":
                    postsQuery = postsQuery.OrderBy(p => p.UpVotes);
                    break;
                case "mostCommented":
                    postsQuery = postsQuery.OrderByDescending(p => p.Comments.Count);
                    break;
                case "leastCommented":
                    postsQuery = postsQuery.OrderBy(p => p.Comments.Count);
                    break;
                default:
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
            }

            var vm = new PostsIndexViewModel()
            {
                Posts = await postsQuery.ToListAsync()
            };

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                vm.Favorites = await postRepository.GetUserFavorites(Convert.ToInt32(userId));
            }

            return View(vm);
        }


        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var vm = new PostDetailsViewModel();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                vm.Favorites = await postRepository.GetUserFavorites(Convert.ToInt32(userId));
            }

            vm.Post = await postRepository.GetByIdAsync((int)id);

            return View(vm);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") == null)
                    return RedirectToAction("Login", "Users");

                if (TempData["PageId"] != null)
                {
                    var newPost = new Post { SubCategoryId = (int)TempData["PageId"], UserId = (int)HttpContext.Session.GetInt32("UserId") };
                    return View(newPost);
                }
                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,SubCategoryId,UserId")] Post post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    post.TimeCreated = DateTime.Now;
                    await postRepository.AddAsync(post);
                    var getMadePost = _context.Post.Where(t => t.Title == post.Title).OrderByDescending(t => t.TimeCreated).FirstOrDefault();

                    return RedirectToAction("Details", new { id = getMadePost.Id });
                }
                return View(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error", "Home");
            }
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var post = await postRepository.GetByIdAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            ViewData["SubCategory"] = new SelectList(await subCategoryRepository.GetAllAsync(), "Id", "Name", post.SubCategoryId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,UpVotes,DownVotes,TimeCreated,UserId,SubCategoryId,Comments")] Post post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await postRepository.Update(post);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategory"] = new SelectList(await subCategoryRepository.GetAllAsync(), "Id", "Name", post.SubCategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? postId, int? userId)
        {
            if (postId == null)
            {
                RedirectToAction("Error", "Home");
            }

            var post = await postRepository.GetByIdAndIncludeAsync((int)postId);
            if (userId == post.UserId)
            {
                return View(post);
            }
            return View("Error", "Home");

        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await postRepository.GetByIdAndIncludeAsync(id);

            if (post != null)
            {
                try
                {
                    var relatedVotes = _context.Vote.Where(v => v.PostId == id);
                    _context.Vote.RemoveRange(relatedVotes);
                    await postRepository.Delete(post);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("Posts/AddToFavorites/{postId}")]
        public async Task<IActionResult> AddToFavorites(int postId)
        {
            var post = await postRepository.GetByIdAsync(postId);

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return new JsonResult(new { result = "Login required" });
            }

            if (post == null)
            {
                return new JsonResult(new { result = "Post does not exist" });
            }

            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var user = await userRepository.GetByIdAsync(userId);

            user.Favorites.Add(post);

            await userRepository.UpdateAsync(user);

            return new JsonResult(new { result = "Post added to favorites" });
        }

        [HttpPost("Posts/RemoveFromFavorites/{postId}")]
        public async Task<IActionResult> RemoveFromFavorites(int postId)
        {
            var post = await postRepository.GetByIdAsync(postId);

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return new JsonResult(new { result = "Login required" });
            }

            if (post == null)
            {
                return new JsonResult(new { result = "Post does not exist" });
            }

            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var user = await userRepository.GetByIdAsync(userId);

            user.Favorites.Remove(post);

            await userRepository.UpdateAsync(user);

            return new JsonResult(new { result = "Post removed from favorites" });
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var favorites = await postRepository.GetUserFavorites(Convert.ToInt32(userId));

            return View(favorites);
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        public async Task<IActionResult> IndexByHeadCategory(int? headCategoryId, string sortOrder)
        {
            if (headCategoryId == null)
            {
                return NotFound();
            }

            IQueryable<Post> postsQuery = _context.Post
                .Where(p => p.SubCategory.HeadCategoryId == headCategoryId)
                .Include(p => p.SubCategory)
                .Include(p => p.User)
                .Include(p => p.Comments);

            ViewData["CurrentSortOrder"] = sortOrder;

            switch (sortOrder)
            {
                case "newest":
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
                case "oldest":
                    postsQuery = postsQuery.OrderBy(p => p.TimeCreated);
                    break;
                case "mostLiked":
                    postsQuery = postsQuery.OrderByDescending(p => p.UpVotes);
                    break;
                case "leastLiked":
                    postsQuery = postsQuery.OrderBy(p => p.UpVotes);
                    break;
                case "mostCommented":
                    postsQuery = postsQuery.OrderByDescending(p => p.Comments.Count);
                    break;
                case "leastCommented":
                    postsQuery = postsQuery.OrderBy(p => p.Comments.Count);
                    break;
                default:
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
            }

            var headCategory = await _context.HeadCategory.FindAsync(headCategoryId);
            if (headCategory == null)
            {
                return NotFound();
            }

            var vm = new PostsIndexViewModel()
            {
                Posts = await postsQuery.ToListAsync()
            };

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                vm.Favorites = await postRepository.GetUserFavorites(Convert.ToInt32(userId));
            }

            ViewData["HeadCategoryName"] = headCategory.Name;
            ViewData["HeadCategoryId"] = headCategoryId;
            return View("IndexByHeadCategory", vm);
        }


        public async Task<IActionResult> IndexBySubCategory(int? subCategoryId, string sortOrder)
        {
            if (subCategoryId == null)
            {
                return NotFound();
            }

            IQueryable<Post> postsQuery = _context.Post
                .Where(p => p.SubCategoryId == subCategoryId)
                .Include(p => p.SubCategory)
                .Include(p => p.User)
                .Include(p => p.Comments);

            TempData["PageId"] = subCategoryId;
            ViewData["CurrentSortOrder"] = sortOrder;

            switch (sortOrder)
            {
                case "newest":
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
                case "oldest":
                    postsQuery = postsQuery.OrderBy(p => p.TimeCreated);
                    break;
                case "mostLiked":
                    postsQuery = postsQuery.OrderByDescending(p => p.UpVotes);
                    break;
                case "leastLiked":
                    postsQuery = postsQuery.OrderBy(p => p.UpVotes);
                    break;
                case "mostCommented":
                    postsQuery = postsQuery.OrderByDescending(p => p.Comments.Count);
                    break;
                case "leastCommented":
                    postsQuery = postsQuery.OrderBy(p => p.Comments.Count);
                    break;
                default:
                    postsQuery = postsQuery.OrderByDescending(p => p.TimeCreated);
                    break;
            }

            var subCategory = await _context.SubCategory.FindAsync(subCategoryId);
            if (subCategory == null)
            {
                return NotFound();
            }

            var vm = new PostsIndexViewModel()
            {
                Posts = await postsQuery.ToListAsync()
            };

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId != null)
            {
                vm.Favorites = await postRepository.GetUserFavorites(Convert.ToInt32(userId));
            }

            ViewData["SubCategoryName"] = subCategory.Name;
            ViewData["SubCategoryId"] = subCategoryId;
            return View("IndexBySubCategory", vm);
        }


        [HttpPost("Posts/Vote/{postId}/{isUpDown}")]
        public async Task<IActionResult> Vote(int postId, bool isUpDown)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return new JsonResult(new { result = "Login required", value = 0 });
            }

            var vote = new Vote()
            {
                UserId = Convert.ToInt32(userId),
                PostId = postId
            };

            if (isUpDown) vote.IsUpVote = true;
            if (!isUpDown) vote.IsDownVote = true;

            var result = await voteRepository.AddAsync(vote);

            if (result == null)
            {
                return new JsonResult(new { result = "Vote removed", value = 0 });
            }

            return new JsonResult(new { result = result, value = 1 });
        }

        [HttpGet("Posts/CountVotes/{postId}")]
        public async Task<IActionResult> CountVotes(int postId)
        {
            var post = await postRepository.GetByIdAsync(postId);

            var upVotes = post.UpVotes;
            var downVotes = post.DownVotes;

            return new JsonResult(new { upVotes = upVotes, downVotes = downVotes });
        }
    }
}
