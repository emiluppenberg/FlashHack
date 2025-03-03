using FlashHack.Data.DataInterfaces;
using FlashHack.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlashHack.Controllers
{
    public class JobbListingController : Controller
    {
        private readonly IJobblistingRepository jobblistingRepository;
        private readonly IUserRepository userRepository;
        public JobbListingController(IJobblistingRepository jobblistingRepository, IUserRepository userRepository)
        {
            this.jobblistingRepository = jobblistingRepository;
            this.userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string? title, string? location)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await userRepository.GetByIdAsync(userId.Value);

            if (!user.IsPremium)
            {
                return RedirectToAction("Error", "Home");
            }

            var jobblistings = new List<Jobblisting>();

            if (title != null || location != null)
            {
                if (title != null)
                {
                    jobblistings.AddRange(await jobblistingRepository.FindAllWithTitle(title));
                }
                if (location != null)
                {
                    jobblistings.AddRange(await jobblistingRepository.FindAllWithLocation(location));
                }

                if (title != null && location != null)
                {
                    jobblistings.RemoveAll(j =>
                        !j.Title.ToLower()
                        .Contains(title.ToLower()) ||
                        !j.Company.Location.ToLower()
                        .Contains(location.ToLower()));

                    jobblistings = jobblistings.Distinct().ToList();
                }
            }
            else
            {
                jobblistings.AddRange(await jobblistingRepository.GetAllAsync());
            }

            return View(jobblistings);
        }
    }
}
