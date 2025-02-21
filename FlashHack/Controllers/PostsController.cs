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
            ViewData["CurrentSubCategoryId"] = subCategoryId;

            if (subCategoryId == null)
            {
                var applicationDbContext = _context.Post.Include(p => p.SubCategory).Include(p => p.User);
                return View(await applicationDbContext.ToListAsync());
            }
            else
            {
                TempData["PageId"] = subCategoryId;
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
            try
            {
                if (TempData["PageId"] != null /*&& HttpContext.Session.GetInt32("UserId") != null*/)
                {
                    var newPost = new Post { SubCategoryId = (int)TempData["PageId"], UserId = 7/*HttpContext.Session.GetInt32("UserId")*/ };
                    return View(newPost);
                }
                return View();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
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
                    return RedirectToAction(nameof(Index));
                }                
                return View(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
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
        public async Task<IActionResult> Delete(int id)
        {
            var post = await postRepository.GetByIdAndIncludeAsync(id);

            return View(post);
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
                    await postRepository.Delete(post);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
