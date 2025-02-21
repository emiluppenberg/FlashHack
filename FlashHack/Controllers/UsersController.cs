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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _userRepository;

        public UsersController(ApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create() => View();

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Employer,PhoneNumber,Email,Password,Bio,ProfilePicURL,Rating,Signature,IsPremium,IsAdmin,ShowEmail,ShowPhoneNumber,ShowEmployer,ShowToRecruiter,ShowRating")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.User.Add(user); // Använder _context för att lägga till direkt
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Employer,PhoneNumber,Email,Bio,ProfilePicURL,Rating,Signature,IsPremium,IsAdmin,ShowEmail,ShowPhoneNumber,ShowEmployer,ShowToRecruiter,ShowRating")] User user)
        {
            if (id != user.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _userRepository.Update(user); // Uppdaterar via repository
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UserExists(user.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _userRepository.GetByIdAsync(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                _context.User.Remove(user); // Tar bort via _context
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserExists(int id)
        {
            return await _userRepository.GetByIdAsync(id) != null;
        }
    }
}
