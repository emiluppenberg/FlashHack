using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlashHack.Data;
using FlashHack.Models;

namespace FlashHack.Controllers
{
    public class HeadCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HeadCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HeadCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.HeadCategory.ToListAsync());
        }

        // GET: HeadCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headCategory = await _context.HeadCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headCategory == null)
            {
                return NotFound();
            }

            return View(headCategory);
        }

        // GET: HeadCategories/Create
        [AdminAuthorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HeadCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public async Task<IActionResult> Create([Bind("Id,Name")] HeadCategory headCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(headCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(headCategory);
        }

        // GET: HeadCategories/Edit/5
        [AdminAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headCategory = await _context.HeadCategory.FindAsync(id);
            if (headCategory == null)
            {
                return NotFound();
            }
            return View(headCategory);
        }

        // POST: HeadCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] HeadCategory headCategory)
        {
            if (id != headCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeadCategoryExists(headCategory.Id))
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
            return View(headCategory);
        }

        // GET: HeadCategories/Delete/5
        [AdminAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var test = HttpContext.Session.GetString("isAdmin");
            if (id == null)
            {
                return NotFound();
            }

            var headCategory = await _context.HeadCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headCategory == null)
            {
                return NotFound();
            }

            return View(headCategory);
        }

        // POST: HeadCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdminAuthorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var headCategory = await _context.HeadCategory.FindAsync(id);
            if (headCategory != null)
            {
                _context.HeadCategory.Remove(headCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeadCategoryExists(int id)
        {
            return _context.HeadCategory.Any(e => e.Id == id);
        }
    }
}
