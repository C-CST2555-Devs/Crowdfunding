using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fundify.Web.Data;
using Fundify.Web.Models;
using Fundify.Web.ViewModels;

namespace Fundify.Web.Controllers
{
    public class WalletController : Controller
    {
        private readonly FundifyContext _context;
        private readonly ILogger<WalletController> _logger;

        public WalletController(FundifyContext context, ILogger<WalletController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Checkout(int campaignId, decimal amount, int? rewardId = null)
        {
            try
            {
                var campaign = await _context.Campaigns
                    .Include(c => c.Rewards)
                    .FirstOrDefaultAsync(c => c.Id == campaignId);

                if (campaign == null)
                    return NotFound();

                var reward = rewardId.HasValue 
                    ? campaign.Rewards.FirstOrDefault(r => r.Id == rewardId)
                    : null;

                var viewModel = new CheckoutViewModel
                {
                    CampaignId = campaignId,
                    CampaignTitle = campaign.Title,
                    Amount = amount,
                    RewardId = rewardId,
                    RewardName = reward?.Name,
                    RewardDescription = reward?.Description,
                    AvailableBalance = 10000M // Example balance for testing
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading checkout");
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcessPayment(CheckoutViewModel model)
        {
            try
            {
                // In a real app, we would process actual payment here
                // For demo, we'll just record the pledge
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                
                var pledge = new Pledge
                {
                    CampaignId = model.CampaignId,
                    UserId = userId,
                    Amount = model.Amount,
                    RewardId = model.RewardId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Pledges.Add(pledge);

                // Update campaign stats
                var campaign = await _context.Campaigns.FindAsync(model.CampaignId);
                if (campaign != null)
                {
                    campaign.RaisedAmount += model.Amount;
                    campaign.BackerCount += 1;
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thank you for your support!";
                return RedirectToAction("Details", "Campaign", new { id = model.CampaignId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                ModelState.AddModelError("", "Error processing payment. Please try again.");
                return View("Checkout", model);
            }
        }
    }
} 