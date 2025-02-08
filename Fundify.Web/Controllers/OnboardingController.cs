using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Fundify.Web.Data;
using Fundify.Web.Models;
using Fundify.Web.ViewModels;

namespace Fundify.Web.Controllers
{
    [Authorize]
    public class OnboardingController : Controller
    {
        private readonly FundifyContext _context;
        private readonly ILogger<OnboardingController> _logger;

        public OnboardingController(FundifyContext context, ILogger<OnboardingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult AccountType()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = _context.Users.Find(userId);

            if (user?.HasCompletedOnboarding ?? false)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AccountType(Models.AccountType accountType)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.AccountType = accountType;
            await _context.SaveChangesAsync();

            return RedirectToAction(accountType switch
            {
                Fundify.Web.Models.AccountType.Backer => nameof(BackerOnboarding),
                Fundify.Web.Models.AccountType.Creator => nameof(CreatorOnboarding),
                Fundify.Web.Models.AccountType.Administrator => nameof(AdminOnboarding),
                _ => throw new ArgumentException("Invalid account type")
            });
        }

        [HttpGet]
        public IActionResult BackerOnboarding()
        {
            return View(new OnboardingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BackerOnboarding(OnboardingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.HasCompletedOnboarding = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult CreatorOnboarding()
        {
            return View(new OnboardingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatorOnboarding(OnboardingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.HasCompletedOnboarding = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AdminOnboarding()
        {
            return View(new OnboardingViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminOnboarding(OnboardingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            user.HasCompletedOnboarding = true;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
} 