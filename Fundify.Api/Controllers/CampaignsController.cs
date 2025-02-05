using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fundify.Api.Data;
using Fundify.Api.Models;

namespace Fundify.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly FundifyContext _context;

        public CampaignsController(FundifyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns()
        {
            return await _context.Campaigns
                .Include(c => c.Rewards)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.Rewards)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
            {
                return NotFound();
            }

            return campaign;
        }

        [HttpPost]
        public async Task<ActionResult<Campaign>> CreateCampaign(Campaign campaign)
        {
            campaign.CreatedAt = DateTime.UtcNow;
            campaign.RaisedAmount = 0;
            campaign.BackerCount = 0;

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, campaign);
        }
    }
} 