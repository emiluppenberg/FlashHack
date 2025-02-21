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

namespace FlashHack.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPostRepository postRepository;
        private readonly ISubCategoryRepository subCategoryRepository;

        public PostsController(ApplicationDbContext context, IPostRepository postRepository, ISubCategoryRepository subCategoryRepository)
        {
            _context = context;
            this.postRepository = postRepository;
            this.subCategoryRepository = subCategoryRepository;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? subCategoryId)
        {
            if (subCategoryId == null)
            {
                var applicationDbContext = _context.Post.Include(p => p.SubCategory).Include(p => p.User);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                var posts = await _context.Post
                    .Where(p => p.SubCategoryId == subCategoryId)
                    .Include(p => p.SubCategory)
                    .Include(p => p.User)
                    .ToListAsync();
                return View(posts);
            }
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.SubCategory)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            Post testPod = new Post();
            testPod.SubCategoryId = 1;
            testPod.UserId = 7;
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View(testPod);
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,SubCategoryId,UserId")] Post post)
        {
            
            if (ModelState.IsValid)
            {                
                post.TimeCreated = DateTime.Now;
                await postRepository.AddAsync(post);
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategory, "Id", "Name", post.SubCategoryId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", post.UserId);
            return View(post);
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
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategory"] = new SelectList(await subCategoryRepository.GetAllAsync(), "Id", "Name", post.SubCategoryId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.SubCategory)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
