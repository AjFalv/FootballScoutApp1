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

            //public List<PreviousClub> PreviousClubs { get; set; } = new List<PreviousClub>();

            [Display(Name = "Profile Photo")]
            public IFormFile ProfilePhoto { get; set; }
            public string? ProfilePhotoPath { get; set; }

            [Display(Name = "YouTube Video URLs")]
            public List<string>? YoutubeVideoUrls { get; set; } // New field for the YouTube video URL

            [Display(Name = "Gallery Images")]
            public List<IFormFile> GalleryImages { get; set; } // New field for multiple gallery images

            public List<string>? GalleryImagePaths { get; set; } 

            #region PreviousClubsProperties
            // Properties for the first previous club
            public string? PreviousClub1Name { get; set; }
            public int? PreviousClub1YearFrom { get; set; }
            public int? PreviousClub1YearTo { get; set; }
            public int? PreviousClub1Appearances { get; set; }
            public int? PreviousClub1Goals { get; set; }
            public int? PreviousClub1CleanSheets { get; set; }

            // Properties for the second previous club
            public string? PreviousClub2Name { get; set; }
            public int? PreviousClub2YearFrom { get; set; }
            public int? PreviousClub2YearTo { get; set; }
            public int? PreviousClub2Appearances { get; set; }
            public int? PreviousClub2Goals { get; set; }
            public int? PreviousClub2CleanSheets { get; set; }

            // Properties for the third previous club
            public string? PreviousClub3Name { get; set; }
            public int? PreviousClub3YearFrom { get; set; }
            public int? PreviousClub3YearTo { get; set; }
            public int? PreviousClub3Appearances { get; set; }
            public int? PreviousClub3Goals { get; set; }
            public int? PreviousClub3CleanSheets { get; set; }
            #endregion

        }

        private async Task LoadAsync(IdentityUser user)
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.Id == user.Id);

            if (userProfile != null)
            {
                
                List<string> youtubeVideoUrls = new List<string>();
                List<string> galleryImagePaths = new List<string>();

                #region NonWorkingJSONSerializationforPreviousClubs
                // Safely deserialize the PreviousClubsJson field
                //if (!string.IsNullOrEmpty(userProfile.PreviousClubsJson))
                //{
                //    try
                //    {
                //        previousClubs = JsonConvert.DeserializeObject<List<PreviousClub>>(userProfile.PreviousClubsJson) ?? new List<PreviousClub>();
                //    }
                //    catch (JsonReaderException)
                //    {
                //        previousClubs = new List<PreviousClub>(); // Default to empty list on error
                //    }
                //}
                #endregion

                // Safely deserialize the YoutubeVideoUrlsJson field
                if (!string.IsNullOrEmpty(userProfile.YoutubeVideoUrlsJson))
                {
                    try
                    {
                        youtubeVideoUrls = JsonConvert.DeserializeObject<List<string>>(userProfile.YoutubeVideoUrlsJson) ?? new List<string>();
                    }
                    catch (JsonReaderException)
                    {
                        youtubeVideoUrls = new List<string>(); // Default to empty list on error
                    }
                }

                if (!string.IsNullOrEmpty(userProfile.GalleryImagePathsJson))
                {
                    try
                    {
                        galleryImagePaths = JsonConvert.DeserializeObject<List<string>>(userProfile.GalleryImagePathsJson) ?? new List<string>();
                    }
                    catch (JsonReaderException)
                    {
                        galleryImagePaths = new List<string>(); // Default to empty list on error
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
                    /*PreviousClubs = previousClubs,*/ // NonWorkingPreviousClubsCode
                    ProfilePhotoPath = userProfile.ProfilePhotoPath, // Load the profile photo path
                    YoutubeVideoUrls = youtubeVideoUrls, // Loaded from JSON
                    GalleryImagePaths = galleryImagePaths,


                    PreviousClub1Name = userProfile.PreviousClub1Name,
                    PreviousClub1YearFrom = userProfile.PreviousClub1YearFrom,
                    PreviousClub1YearTo = userProfile.PreviousClub1YearTo,
                    PreviousClub1Appearances = userProfile.PreviousClub1Appearances,
                    PreviousClub1Goals = userProfile.PreviousClub1Goals,
                    PreviousClub1CleanSheets = userProfile.PreviousClub1CleanSheets,

                    PreviousClub2Name = userProfile.PreviousClub2Name,
                    PreviousClub2YearFrom = userProfile.PreviousClub2YearFrom,
                    PreviousClub2YearTo = userProfile.PreviousClub2YearTo,
                    PreviousClub2Appearances = userProfile.PreviousClub2Appearances,
                    PreviousClub2Goals = userProfile.PreviousClub2Goals,
                    PreviousClub2CleanSheets = userProfile.PreviousClub2CleanSheets,

                    PreviousClub3Name = userProfile.PreviousClub3Name,
                    PreviousClub3YearFrom = userProfile.PreviousClub3YearFrom,
                    PreviousClub3YearTo = userProfile.PreviousClub3YearTo,
                    PreviousClub3Appearances = userProfile.PreviousClub3Appearances,
                    PreviousClub3Goals = userProfile.PreviousClub3Goals,
                    PreviousClub3CleanSheets = userProfile.PreviousClub3CleanSheets

                };
            }
            else
            {
                Input = new InputModel
                {
                    /*PreviousClubs = new List<PreviousClub>(),*/ // NonWorkingPreviousClubsCode
                    YoutubeVideoUrls = new List<string>(), // Initialize with an empty list
                    GalleryImagePaths = new List<string>()
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

            // FAILED CODE: Serialize PreviousClubs to JSON and store it
            //userProfile.PreviousClubsJson = JsonConvert.SerializeObject(Input.PreviousClubs);

            #region PreviousClubsProfileFields
            userProfile.PreviousClub1Name = Input.PreviousClub1Name;
            userProfile.PreviousClub1YearFrom = Input.PreviousClub1YearFrom;
            userProfile.PreviousClub1YearTo = Input.PreviousClub1YearTo;
            userProfile.PreviousClub1Appearances = Input.PreviousClub1Appearances;
            userProfile.PreviousClub1Goals = Input.PreviousClub1Goals;
            userProfile.PreviousClub1CleanSheets = Input.PreviousClub1CleanSheets;

            userProfile.PreviousClub2Name = Input.PreviousClub2Name;
            userProfile.PreviousClub2YearFrom = Input.PreviousClub2YearFrom;
            userProfile.PreviousClub2YearTo = Input.PreviousClub2YearTo;
            userProfile.PreviousClub2Appearances = Input.PreviousClub2Appearances;
            userProfile.PreviousClub2Goals = Input.PreviousClub2Goals;
            userProfile.PreviousClub2CleanSheets = Input.PreviousClub2CleanSheets;

            userProfile.PreviousClub3Name = Input.PreviousClub3Name;
            userProfile.PreviousClub3YearFrom = Input.PreviousClub3YearFrom;
            userProfile.PreviousClub3YearTo = Input.PreviousClub3YearTo;
            userProfile.PreviousClub3Appearances = Input.PreviousClub3Appearances;
            userProfile.PreviousClub3Goals = Input.PreviousClub3Goals;
            userProfile.PreviousClub3CleanSheets = Input.PreviousClub3CleanSheets;
            #endregion

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

            // Handle gallery images upload
            if (Input.GalleryImages != null && Input.GalleryImages.Count > 0)
            {
                var galleryDirectory = Path.Combine("wwwroot/images/profiles", user.Id, "gallery");
                if (!Directory.Exists(galleryDirectory))
                {
                    Directory.CreateDirectory(galleryDirectory);
                }

                var galleryImagePaths = new List<string>();

                foreach (var image in Input.GalleryImages)
                {
                    var galleryFileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
                    var galleryFilePath = Path.Combine(galleryDirectory, galleryFileName);

                    using (var stream = new FileStream(galleryFilePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    galleryImagePaths.Add($"/images/profiles/{user.Id}/gallery/{galleryFileName}");
                }

                // Save the relative paths to the database as JSON
                userProfile.GalleryImagePathsJson = JsonConvert.SerializeObject(galleryImagePaths);
            }



            // Convert the list of YouTube URLs to JSON and save it
            userProfile.YoutubeVideoUrlsJson = JsonConvert.SerializeObject(Input.YoutubeVideoUrls);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Refresh the sign-in to reflect any changes in the user's profile
            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User updated their player profile.");

            return RedirectToPage();
        }




    }
}
