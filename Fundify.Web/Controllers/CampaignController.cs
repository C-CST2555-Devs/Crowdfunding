using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Fundify.Web.Data;
using Fundify.Web.Models;
using Fundify.Web.ViewModels;
using System.Linq;
using System.IO;

namespace Fundify.Web.Controllers
{
    public class CampaignController : Controller
    {
        private readonly FundifyContext _context;

        public CampaignController(FundifyContext context)
        {
            _context = context;
        }

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
            // Add logging to see what's happening
            Console.WriteLine($"Step: {step}, ModelState.IsValid: {ModelState.IsValid}");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Error: {error.ErrorMessage}");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Step"] = step;
                return View(model);
            }

            if (step < 4) // We have 4 steps total
            {
                if (model.ImageFile != null)
                {
                    // Save the image
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder); // Create if not exists
                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }
                    
                    model.ImageFileName = uniqueFileName;
                    TempData["ImagePath"] = "/uploads/" + uniqueFileName;
                }

                // Store the current step's data in TempData
                TempData[$"Step{step}Data"] = System.Text.Json.JsonSerializer.Serialize(model);
                return RedirectToAction(nameof(Create), new { step = step + 1 });
            }

            // This is the final step
            var campaign = new Campaign
            {
                Title = model.Title,
                Category = model.Category,
                Goal = model.Goal,
                Description = model.Description,
                DurationInDays = model.DurationInDays,
                CreatedAt = DateTime.UtcNow,
                RaisedAmount = 0,
                BackerCount = 0
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Rewards()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Preview()
        {
            return View();
        }
    }
} 