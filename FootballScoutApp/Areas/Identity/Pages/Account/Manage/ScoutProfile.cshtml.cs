#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballScoutApp.Models;
using Microsoft.EntityFrameworkCore;
using FootballScoutApp.Data; 
using Newtonsoft.Json;
using FootballScoutApp.Helpers;

namespace FootballScoutApp.Areas.Identity.Pages.Account.Manage
{
    public class ScoutProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<ScoutProfileModel> _logger;
        private readonly ApplicationDBContext _context; 

        public ScoutProfileModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<ScoutProfileModel> logger,
            ApplicationDBContext context) // Added this parameter
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context; // Added this line
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Nationality { get; set; }
            public string Birthplace { get; set; }
            public string Bio { get; set; }

            [Display(Name = "Profile Photo")]
            public IFormFile ProfilePhoto { get; set; }
            public string ProfilePhotoPath { get; set; } // Profile Photo
            public string CurrentAffiliation { get; set; } // Current Club/Organization
            public int? ScoutingExperience { get; set; } // Years of experience
            public string NotablePlayersScouted { get; set; } // Notable players scouted
            public string SuccessfulTransfers { get; set; } // Successful transfers
            public string PrimaryRegions { get; set; } // Primary regions covered
            public string AchievementsAndAwards { get; set; } // Achievements and awards
            public string ScoutingPhilosophy { get; set; } // Scouting philosophy
            public string ContactEmail { get; set; } // Contact email
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userProfile = await _context.UserProfiles.FindAsync(user.Id);
            Input = new InputModel
            {
                Firstname = userProfile.Firstname,
                Lastname = userProfile.Lastname,
                DateOfBirth = userProfile.DateOfBirth,
                Nationality = userProfile.Nationality,
                Birthplace = userProfile.Birthplace,
                Bio = userProfile.Bio,
                ProfilePhotoPath = userProfile.ProfilePhotoPath,
                CurrentAffiliation = userProfile.CurrentAffiliation,
                ScoutingExperience = userProfile.ScoutingExperience,
                NotablePlayersScouted = userProfile.NotablePlayersScouted,
                SuccessfulTransfers = userProfile.SuccessfulTransfers,
                PrimaryRegions = userProfile.PrimaryRegions,
                AchievementsAndAwards = userProfile.AchievementsAndAwards,
                ScoutingPhilosophy = userProfile.ScoutingPhilosophy,
                ContactEmail = userProfile.ContactEmail
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ViewData["Nationalities"] = ListHelpers.GetCountryList();

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

            // Handle file upload
            if (Input.ProfilePhoto != null)
            {
                // Create a directory for the user if it doesn't exist
                var userDirectory = Path.Combine("wwwroot/images/profiles", user.Id);
                if (!Directory.Exists(userDirectory))
                {
                    Directory.CreateDirectory(userDirectory);
                }

                // Create a unique filename
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(Input.ProfilePhoto.FileName)}";
                var filePath = Path.Combine(userDirectory, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ProfilePhoto.CopyToAsync(stream);
                }

                // Save the relative path to the database
                userProfile.ProfilePhotoPath = $"/images/profiles/{user.Id}/{fileName}";
            }

            // Update the scouting profile fields
            userProfile.Firstname = Input.Firstname;
            userProfile.Lastname = Input.Lastname;
            userProfile.DateOfBirth = Input.DateOfBirth;
            userProfile.Nationality = Input.Nationality;
            userProfile.Birthplace = Input.Birthplace;
            userProfile.Bio = Input.Bio;
            userProfile.ProfilePhotoPath = Input.ProfilePhotoPath;
            userProfile.CurrentAffiliation = Input.CurrentAffiliation;
            userProfile.ScoutingExperience = Input.ScoutingExperience;
            userProfile.NotablePlayersScouted = Input.NotablePlayersScouted;
            userProfile.SuccessfulTransfers = Input.SuccessfulTransfers;
            userProfile.PrimaryRegions = Input.PrimaryRegions;
            userProfile.AchievementsAndAwards = Input.AchievementsAndAwards;
            userProfile.ScoutingPhilosophy = Input.ScoutingPhilosophy;
            userProfile.ContactEmail = Input.ContactEmail;

            await _context.SaveChangesAsync();

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User updated their scout profile.");
            return RedirectToPage();
        }
    }
}
