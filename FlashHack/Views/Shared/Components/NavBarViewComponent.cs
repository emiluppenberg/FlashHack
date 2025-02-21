using FlashHack.Data;
using FlashHack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FlashHack.Views.Shared.Components
{
    public class NavBarViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NavBarViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headCategories = await _context.HeadCategory
                .Include(hc => hc.SubCategories)
                .ToListAsync();

            var viewModel = new NavBarViewModel
            {
                HeadCategories = headCategories
            };

            return View(viewModel);
        }
    }
}