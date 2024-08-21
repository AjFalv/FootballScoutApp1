#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using FootballScoutApp.Models;
using FootballScoutApp.Data; // Add this namespace
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using FootballScoutApp.Helpers;


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

            [Display(Name = "Profile Photo")]
            public IFormFile ProfilePhoto { get; set; }
            public string? ProfilePhotoPath { get; set; }

            [Display(Name = "YouTube Video URLs")]
            public List<string>? YoutubeVideoUrls { get; set; } // New field for the YouTube video URL

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Id == user.Id);

            if (userProfile != null)
            {
                List<PreviousClub> previousClubs = new List<PreviousClub>();
                List<string> youtubeVideoUrls = new List<string>();

                // Safely deserialize the PreviousClubsJson field
                if (!string.IsNullOrEmpty(userProfile.PreviousClubsJson))
                {
                    try
                    {
                        previousClubs = JsonConvert.DeserializeObject<List<PreviousClub>>(userProfile.PreviousClubsJson);
                    }
                    catch (JsonReaderException)
                    {
                        previousClubs = new List<PreviousClub>(); // Default to empty list on error
                    }
                }

                // Safely deserialize the YoutubeVideoUrlsJson field
                if (!string.IsNullOrEmpty(userProfile.YoutubeVideoUrlsJson))
                {
                    try
                    {
                        youtubeVideoUrls = JsonConvert.DeserializeObject<List<string>>(userProfile.YoutubeVideoUrlsJson);
                    }
                    catch (JsonReaderException)
                    {
                        youtubeVideoUrls = new List<string>(); // Default to empty list on error
                    }
                }

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
                    PreviousClubs = previousClubs, // Loaded from JSON
                    ProfilePhotoPath = userProfile.ProfilePhotoPath, // Load the profile photo path
                    YoutubeVideoUrls = youtubeVideoUrls // Loaded from JSON
                };
            }
            else
            {
                Input = new InputModel
                {
                    PreviousClubs = new List<PreviousClub>(), // Initialize with an empty list
                    YoutubeVideoUrls = new List<string>() // Initialize with an empty list
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

            // Fetch the user's profile
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Id == user.Id);

            if (userProfile == null)
            {
                userProfile = new UserProfile { Id = user.Id };
                _context.UserProfiles.Add(userProfile);
            }

            // Update the YouTube Video URL
            userProfile.YoutubeVideoUrlsJson = JsonConvert.SerializeObject(Input.YoutubeVideoUrls);

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



            // Serialize PreviousClubs to JSON and store it
            userProfile.PreviousClubsJson = JsonConvert.SerializeObject(Input.PreviousClubs);

            // Update other user profile fields
            userProfile.DateOfBirth = Input.DateOfBirth;
            userProfile.Nationality = Input.Nationality;
            userProfile.Birthplace = Input.Birthplace;
            userProfile.Bio = Input.Bio;
            userProfile.Position = Input.Position;
            userProfile.PreferredFoot = Input.PreferredFoot;
            userProfile.Height = Input.Height;
            userProfile.Weight = Input.Weight;
            userProfile.InjuryHistory = Input.InjuryHistory;
            /*userProfile.ProfilePhotoPath = Input.ProfilePhotoPath;*/ // Save the profile photo path

            // Convert the list of YouTube URLs to JSON and save it
            userProfile.YoutubeVideoUrlsJson = JsonConvert.SerializeObject(Input.YoutubeVideoUrls);

            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User updated their player profile.");
            return RedirectToPage();
        }




    }
}
