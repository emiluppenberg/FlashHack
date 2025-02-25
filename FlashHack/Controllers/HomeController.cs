using System.Diagnostics;
using FlashHack.Data;
using FlashHack.Models;
using FlashHack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlashHack.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var headCategories = await _context.HeadCategory
                .Include(hc => hc.SubCategories)
                .ThenInclude(sc => sc.Posts)
                .ThenInclude(p => p.User)
                .ToListAsync();

            var viewModel = new HomeIndexViewModel
            {
                HeadCategories = headCategories.Select(hc => new HeadCategoryViewModel
                {
                    Id = hc.Id,
                    Name = hc.Name,
                    SubCategories = hc.SubCategories.Select(sc => new SubCategoryViewModel
                    {
                        Id = sc.Id,
                        Name = sc.Name,
                        PostCount = sc.Posts.Count,
                        MostRecentPostTitle = sc.Posts.OrderByDescending(p => p.TimeCreated).FirstOrDefault()?.Title,
                        MostRecentPostUser = sc.Posts.OrderByDescending(p => p.TimeCreated).FirstOrDefault()?.User?.FirstName,
                        HasPosts = sc.Posts.Any()
                    }).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
