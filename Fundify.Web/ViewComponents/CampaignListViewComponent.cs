using Microsoft.AspNetCore.Mvc;
using Fundify.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace Fundify.Web.ViewComponents
{
    public class CampaignListViewComponent : ViewComponent
    {
        private readonly FundifyContext _context;

        public CampaignListViewComponent(FundifyContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var campaigns = await _context.Campaigns
                .Include(c => c.Rewards)
                .ToListAsync();
            return View(campaigns);
        }
    }
} 