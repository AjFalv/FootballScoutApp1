#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballScoutApp.Models;
using FootballScoutApp.Data; // Add this namespace
using Microsoft.EntityFrameworkCore;

namespace FootballScoutApp.Areas.Identity.Pages.Account.Manage
{
    public class PlayerProfileModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<PlayerProfileModel> _logger;
        private readonly ApplicationDBContext _context; // Add this line

        public PlayerProfileModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<PlayerProfileModel> logger,
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
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public DateTime? DateOfBirth { get; set; }
            public string Nationality { get; set; }
            public string Birthplace { get; set; }
            public string Bio { get; set; }
            public string Position { get; set; }
            public int? Experience { get; set; }
            public string PreferredFoot { get; set; }
            public int? Height { get; set; }
            public int? Weight { get; set; }
            public string? InjuryHistory { get; set; }
            public List<PreviousClub> PreviousClubs { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userProfile = await _context.UserProfiles
                                             .Include(up => up.PreviousClubs)
                                             .FirstOrDefaultAsync(up => up.Id == user.Id);

            if (userProfile != null)
            {
                Input = new InputModel
                {
                    Firstname = userProfile.Firstname,
                    Lastname = userProfile.Lastname,
                    DateOfBirth = userProfile.DateOfBirth,
                    Nationality = userProfile.Nationality,
                    Birthplace = userProfile.Birthplace,
                    Bio = userProfile.Bio,
                    Position = userProfile.Position,
                    PreferredFoot = userProfile.PreferredFoot,
                    Height = userProfile.Height,
                    Weight = userProfile.Weight,
                    InjuryHistory = userProfile.InjuryHistory,
                    PreviousClubs = userProfile.PreviousClubs.Select(pc => new PreviousClub
                    {
                        Id = pc.Id,
                        ClubName = pc.ClubName,
                        YearFrom = pc.YearFrom,
                        YearTo = pc.YearTo,
                        Appearances = pc.Appearances,
                        Goals = pc.Goals,
                        CleanSheets = pc.CleanSheets,
                        UserProfileId = pc.UserProfileId // Ensure the UserProfileId is mapped
                    }).ToList()
                };
            }
            else
            {
                Input = new InputModel
                {
                    PreviousClubs = new List<PreviousClub>() // Initialize with an empty list
                };
            }
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
                _logger.LogError($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is invalid.");
                await LoadAsync(user);
                return Page();
            }

            // Fetch the user's profile
            var userProfile = await _context.UserProfiles
                                             .Include(up => up.PreviousClubs)
                                             .FirstOrDefaultAsync(up => up.Id == user.Id);

            if (userProfile == null)
            {
                _logger.LogInformation("Creating a new user profile.");
                userProfile = new UserProfile { Id = user.Id };
                _context.UserProfiles.Add(userProfile);
            }
            else
            {
                _logger.LogInformation("Updating existing user profile.");
            }

            // Remove only the PreviousClubs that have not been resubmitted
            if (userProfile.PreviousClubs != null)
            {
                var clubsToRemove = userProfile.PreviousClubs
                                               .Where(existingClub => !Input.PreviousClubs
                                                                        .Any(inputClub => inputClub.Id == existingClub.Id))
                                               .ToList();

                _logger.LogInformation($"Removing {clubsToRemove.Count} previous clubs.");

                _context.PreviousClubs.RemoveRange(clubsToRemove);
            }

            // Update or add the resubmitted clubs
            foreach (var club in Input.PreviousClubs)
            {
                var existingClub = userProfile.PreviousClubs
                                              .FirstOrDefault(c => c.Id == club.Id);

                if (existingClub != null)
                {
                    _logger.LogInformation($"Updating club with ID {existingClub.Id}.");
                    // Update existing club
                    existingClub.ClubName = club.ClubName;
                    existingClub.YearFrom = club.YearFrom;
                    existingClub.YearTo = club.YearTo;
                    existingClub.Appearances = club.Appearances;
                    existingClub.Goals = club.Goals;
                    existingClub.CleanSheets = club.CleanSheets;
                }
                else
                {
                    _logger.LogInformation("Adding new club.");
                    // Add new club
                    club.UserProfileId = userProfile.Id;
                    _context.PreviousClubs.Add(club);
                }
            }

            // Update user profile fields
            userProfile.DateOfBirth = Input.DateOfBirth;
            userProfile.Nationality = Input.Nationality;
            userProfile.Birthplace = Input.Birthplace;
            userProfile.Bio = Input.Bio;
            userProfile.Position = Input.Position;
            userProfile.PreferredFoot = Input.PreferredFoot;
            userProfile.Height = Input.Height;
            userProfile.Weight = Input.Weight;
            userProfile.InjuryHistory = Input.InjuryHistory;

            _logger.LogInformation("Saving changes to the database.");

            // Save changes to the database
            await _context.SaveChangesAsync();

            _logger.LogInformation("User updated their player profile.");

            // Refresh the user's sign-in session to apply changes
            await _signInManager.RefreshSignInAsync(user);

            return RedirectToPage();
        }




    }
}
