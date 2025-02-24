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

        // GET: Users/Register – Visar registreringssidan för besökare
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("FirstName,LastName,PhoneNumber,Email,Password,Employer,Bio,ProfilePicURL,Signature,Rating")] User user)

        {
            
            if (!ModelState.IsValid)
            {
               
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"❌ {state.Key}: {error.ErrorMessage}");
                    }
                }
                return View(user);
            }

            // Kontrollera om e-post redan finns
            var existingUser = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Email == user.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
                
                return View(user);
            }

            // Sätt standardvärden för valfria fält
            user.Employer ??= string.Empty;
            user.Bio ??= string.Empty;
            user.ProfilePicURL ??= string.Empty;
            user.Signature ??= string.Empty;
            user.Rating ??= 0;

            // Standardvärden
            user.IsAdmin = false;
            user.IsPremium = false;
            user.Rating = user.Rating > 0 ? user.Rating : 0;
            user.ProfilePicURL ??= string.Empty;
            user.Employer ??= string.Empty;
            user.Bio ??= string.Empty;
            user.Signature ??= string.Empty;

            // Spara användaren i databasen
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            

            return RedirectToAction("Login");
        }


        // GET: Users/Login (Visar inloggningssidan)
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login (Hanterar inloggningsförsöket)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "E-post och lösenord krävs.");
                return View();
            }

            // Hämta användaren baserat på e-post och lösenord
            var user = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Spara användarens session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.FirstName);
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Fel e-post eller lösenord.");
            return View();
        }

        // GET: Users/Logout (Loggar ut användaren)
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Tar bort all sessionsdata
            return RedirectToAction("Login");
        }

        // GET: Users/Create – Endast för admin
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        // POST: Users/Create – Endast för admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Password,IsAdmin")] User user)
        {
            if (ModelState.IsValid)
            {
                // Kontrollera om e-post redan finns
                var existingUser = (await _userRepository.GetAllAsync())
                    .FirstOrDefault(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "E-postadressen är redan registrerad.");
                    return View(user);
                }

                // Lägg till användaren som admin eller vanlig användare
                _context.User.Add(user);
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
