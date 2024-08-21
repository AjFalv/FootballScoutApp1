using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballScoutApp.Models
{
    public class UserProfile
    {
        public string Id { get; set; } // Primary key and foreign key to AspNetUsers
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public int? Age { get; set; }
        public string? Nationality { get; set; } // New Field: Nationality
        public string? Birthplace { get; set; } // New Field: Birthplace
        public string? Bio { get; set; }
        public string? Position { get; set; } // For players
        public int? Experience { get; set; } // For players
        public string? PreferredFoot { get; set; } // Preferred Foot
        public int? Height { get; set; } // Height in cm
        public int? Weight { get; set; } // Weight in kg
        public string? InjuryHistory { get; set; } // Injury History
        public string? PreviousClubsJson { get; set; } // Previous Clubs
        public string? ProfilePhotoPath { get; set; }
        public string? YoutubeVideoUrlsJson { get; set; } // Store multiple YouTube video URLs as a JSON string

        // Scouting Profile Fields 
        public string? CurrentAffiliation { get; set; } // Current Club/Organization
        public int? ScoutingExperience { get; set; } // Years of experience
        public string? NotablePlayersScouted { get; set; } // Notable players scouted
        public string? SuccessfulTransfers { get; set; } // Successful transfers
        public string? PrimaryRegions { get; set; } // Primary regions covered
        public string? AchievementsAndAwards { get; set; } // Achievements and awards
        public string? ScoutingPhilosophy { get; set; } // Scouting philosophy
        public string? ContactEmail { get; set; } // Contact email

        public IdentityUser User { get; set; } // Navigation property
    }

    public class PreviousClub
    {
        public int Id { get; set; }
        // Explicitly define this as a foreign key to the UserProfile
        [ForeignKey("UserProfile")]
        public string? UserProfileId { get; set; } // Foreign key to UserProfile
        public string? ClubName { get; set; } // New Field: Club Name
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string? Appearances { get; set; }
        public string? Goals { get; set; }
        public string? CleanSheets { get; set; }

        public UserProfile? UserProfile { get; set; } // Navigation property
    }
}
