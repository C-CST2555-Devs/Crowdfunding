using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/campaigns")]
[ApiController]
public class CampaignsController : ControllerBase
{
    private readonly AppDbContext _context;

    public CampaignsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Campaign>>> GetCampaigns()
    {
        return await _context.Campaigns.ToListAsync();
    }
} 