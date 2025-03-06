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

        public async Task<IActionResult> Index()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (string.IsNullOrEmpty(isAdmin) || isAdmin != "True")
            {
                return RedirectToAction("Index", "Home");
            }

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
                        Console.WriteLine($" {state.Key}: {error.ErrorMessage}");
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


        // GET: Users/Profile
        public async Task<IActionResult> Profile(int? id, string sortOrder)
        {
            if (id == null)
            {
                //  Hämta ID från session om det inte skickas
                id = HttpContext.Session.GetInt32("UserId");
                if (id == null)
                {
                    return RedirectToAction("Login");
                }
            }

            var user = await _context.User
                .Include(u => u.Skills)
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments) // Include the comments for each post
                .FirstOrDefaultAsync(u => u.Id == id.Value);

            if (user == null)
            {
                return NotFound();
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "recent":
                    user.Posts = user.Posts.OrderByDescending(p => p.TimeCreated).ToList();
                    break;
                case "oldest":
                    user.Posts = user.Posts.OrderBy(p => p.TimeCreated).ToList();
                    break;
                case "most_liked":
                    user.Posts = user.Posts.OrderByDescending(p => p.UpVotes).ToList();
                    break;
                case "most_comments":
                    user.Posts = user.Posts.OrderByDescending(p => p.Comments.Count).ToList();
                    break;
                default:
                    user.Posts = user.Posts.OrderByDescending(p => p.TimeCreated).ToList();
                    break;
            }

            return View(user);
        }




        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(User updatedUser, string? skillName, string? skillDescription, int? skillRating)
        {
            

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);

            if (user == null)
            {
                
                return NotFound();
            }

            if (string.IsNullOrEmpty(updatedUser.Password))
            {
                ModelState.Remove("Password"); // Ignorera lösenord vid validering
            }

            ModelState.Clear();
            TryValidateModel(updatedUser);

            if (ModelState.IsValid)
            {
                

                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.PhoneNumber = updatedUser.PhoneNumber;
                user.Employer = updatedUser.Employer ?? string.Empty;
                user.Bio = updatedUser.Bio ?? string.Empty;
                user.Signature = updatedUser.Signature ?? string.Empty;
                user.ProfilePicURL = updatedUser.ProfilePicURL ?? string.Empty;
                user.IsPremium = updatedUser.IsPremium;
                user.ShowEmail = updatedUser.ShowEmail;
                user.ShowPhoneNumber = updatedUser.ShowPhoneNumber;
                user.ShowEmployer = updatedUser.ShowEmployer;
                user.ShowBio = updatedUser.ShowBio;
                user.ShowRating = updatedUser.ShowRating;
                user.ShowSkills = updatedUser.ShowSkills;
                user.ShowToRecruiter = updatedUser.ShowToRecruiter;




                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    user.Password = updatedUser.Password;
                }


                // Lägg till ny färdighet (om det finns)
                if (!string.IsNullOrEmpty(skillName) && !string.IsNullOrEmpty(skillDescription) && skillRating.HasValue)
                {
                    user.Skills.Add(new Skill
                    {
                        UserId = user.Id,
                        SkillName = skillName,
                        SkillDescription = skillDescription,
                        SkillRating = skillRating.Value
                    });
                }

                await _userRepository.Update(user);
                HttpContext.Session.SetString("UserName", user.FirstName);

                return RedirectToAction("Profile", new { id = user.Id });
            }

            

            return View("UpdateProfile", user);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSkill(string skillName, string skillDescription, int skillRating)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            //  Lägg till ny färdighet
            if (!string.IsNullOrEmpty(skillName) && !string.IsNullOrEmpty(skillDescription) && skillRating > 0)
            {
                user.Skills.Add(new Skill
                {
                    UserId = user.Id,
                    SkillName = skillName,
                    SkillDescription = skillDescription,
                    SkillRating = skillRating
                });

                await _userRepository.Update(user);
                
            }

            return RedirectToAction("UpdateProfile");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSkill(int skillId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var user = await _userRepository.GetByIdAsync(userId.Value);
            if (user == null)
            {
                return NotFound();
            }

            // Hitta och ta bort färdigheten
            var skill = user.Skills?.FirstOrDefault(s => s.Id == skillId);
            if (skill != null)
            {
                user.Skills.Remove(skill);
                _context.Skill.Remove(skill); // Ta bort från databasen
                await _userRepository.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("UpdateProfile");
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
