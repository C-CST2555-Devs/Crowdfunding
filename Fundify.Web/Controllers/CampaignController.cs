using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fundify.Web.Data;
using Fundify.Web.Models;
using Fundify.Web.ViewModels;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Fundify.Web.Controllers
{
    public class CampaignController : Controller
    {
        private readonly FundifyContext _context;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(FundifyContext context, ILogger<CampaignController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(int step = 1)
        {
            ViewData["Step"] = step;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CampaignViewModel model, int step = 1)
        {
            // Only validate if form was actually submitted with the Continue/Submit button
            if (Request.Method == "POST" && !Request.Headers["Referer"].ToString().Contains($"step={step}"))
            {
                if (step == 1)
                {
                    // Validate only basic fields when continuing to next step
                    if (string.IsNullOrEmpty(model.Title) || 
                        string.IsNullOrEmpty(model.Category) || 
                        model.Goal < 1000 || 
                        model.DurationInDays < 30 || 
                        model.DurationInDays > 60)
                    {
                        ModelState.AddModelError("", "Please fill in all required fields correctly.");
                        ViewData["Step"] = step;
                        return View(model);
                    }
                }
                else if (step == 2)
                {
                    // Only validate description if "Continue to Rewards" was clicked
                    if (string.IsNullOrEmpty(model.Description))
                    {
                        ModelState.AddModelError("Description", "Please provide a project description");
                        ViewData["Step"] = step;
                        return View(model);
                    }
                }
            }

            // Clear ModelState when first loading the form
            if (Request.Method == "GET")
            {
                ModelState.Clear();
            }

            try
            {
                if (step == 1)
                {
                    TempData["Title"] = model.Title;
                    TempData["Category"] = model.Category;
                    TempData["Goal"] = model.Goal.ToString("F2");
                    TempData["DurationInDays"] = model.DurationInDays.ToString();
                }
                else if (step == 2)
                {
                    TempData["Description"] = model.Description;
                    if (model.ImageFile != null)
                    {
                        // Create uploads directory if it doesn't exist
                        var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                        Directory.CreateDirectory(uploadsDir);

                        // Generate unique filename
                        var uniqueFileName = $"{Guid.NewGuid()}_{model.ImageFile.FileName}";
                        var filePath = Path.Combine(uploadsDir, uniqueFileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }

                        // Store the relative path
                        TempData["ImageUrl"] = $"/images/{uniqueFileName}";
                    }
                }

                if (step < 4) // If not the final step
                {
                    ViewData["Step"] = step + 1;
                    return View(model);
                }

                // Final step - save the campaign
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                
                var campaign = new Campaign
                {
                    Title = TempData["Title"]?.ToString() ?? string.Empty,
                    Description = TempData["Description"]?.ToString() ?? string.Empty,
                    Goal = decimal.Parse(TempData["Goal"]?.ToString() ?? "0"),
                    Category = TempData["Category"]?.ToString() ?? string.Empty,
                    DurationInDays = int.Parse(TempData["DurationInDays"]?.ToString() ?? "30"),
                    ImageUrl = TempData["ImageUrl"]?.ToString() ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CreatorId = userId,
                    RaisedAmount = 0,
                    BackerCount = 0
                };

                _context.Campaigns.Add(campaign);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Campaign created successfully!";
                return RedirectToAction("Details", new { id = campaign.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating campaign");
                ModelState.AddModelError("", "Error creating campaign. Please try again.");
                ViewData["Step"] = step;
                return View(model);
            }
        }

        [Authorize]
        [HttpGet("Campaign/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try 
            {
                var campaign = await _context.Campaigns
                    .Include(c => c.Rewards)
                    .Include(c => c.Creator)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (campaign == null)
                {
                    return NotFound();
                }

                var viewModel = new CampaignDetailsViewModel
                {
                    Id = campaign.Id,
                    Title = campaign.Title,
                    Category = campaign.Category,
                    Description = campaign.Description,
                    ImageUrl = campaign.ImageUrl,
                    Goal = campaign.Goal,
                    RaisedAmount = campaign.RaisedAmount,
                    BackerCount = campaign.BackerCount,
                    DurationInDays = campaign.DurationInDays,
                    CreatedAt = campaign.CreatedAt,
                    CreatorName = $"{campaign.Creator.FirstName} {campaign.Creator.LastName}",
                    Location = "Glasgow, United Kingdom",
                    Rewards = campaign.Rewards.Select(r => new RewardViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Amount = r.Amount,
                        Description = r.Description,
                        Limit = r.Limit,
                        EstimatedDelivery = r.EstimatedDelivery
                    }).ToList()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving campaign details");
                return RedirectToAction("Error", "Home", new { message = "Unable to load campaign details" });
            }
        }

        [HttpGet]
        public IActionResult Rewards()
        {
            ViewData["Step"] = 3;
            return View();
        }

        [HttpGet]
        public IActionResult Preview()
        {
            ViewData["Step"] = 4;
            return View();
        }
    }
} 