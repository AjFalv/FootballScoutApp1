#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballScoutApp.Models;
using FootballScoutApp.Data; // Add this namespace

namespace FootballScoutApp.Areas.Identity.Pages.Account.Manage
{
    public class ScoutProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ScoutProfileModel> _logger;
        private readonly ApplicationDBContext _context; // Add this line

        public ScoutProfileModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ScoutProfileModel> logger,
            ApplicationDBContext context) // Add this parameter
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context; // Add this line
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Expertise { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userProfile = await _context.UserProfiles.FindAsync(user.Id);
            Input = new InputModel
            {
                Expertise = userProfile?.Expertise
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userProfile = await _context.UserProfiles.FindAsync(user.Id);
            if (userProfile == null)
            {
                userProfile = new UserProfile { Id = user.Id };
                _context.UserProfiles.Add(userProfile);
            }

            userProfile.Expertise = Input.Expertise;

            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User updated their scout profile.");
            return RedirectToPage();
        }
    }
}
