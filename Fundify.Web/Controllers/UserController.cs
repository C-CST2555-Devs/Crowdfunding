using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Fundify.Web.Data;
using Fundify.Web.Models;
using Fundify.Web.ViewModels;

namespace Fundify.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly FundifyContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(FundifyContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new UserProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AccountType = user.AccountType,
                CreatedAt = user.CreatedAt,
                HasCompletedOnboarding = user.HasCompletedOnboarding
                // Add other stats based on account type
            };

            return View(viewModel);
        }
    }
} 