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

        /*public string? PreviousClubsJson { get; set; }*/ // JSON Serialization could not get to work. 
        public string? ProfilePhotoPath { get; set; }
        public string? YoutubeVideoUrlsJson { get; set; } // Store multiple YouTube video URLs as a JSON string

        public string? GalleryImagePathsJson { get; set; }

        // Scouting Profile Fields 
        public string? CurrentAffiliation { get; set; } // Current Club/Organization
        public int? ScoutingExperience { get; set; } // Years of experience
        public string? NotablePlayersScouted { get; set; } // Notable players scouted
        public string? SuccessfulTransfers { get; set; } // Successful transfers
        public string? PrimaryRegions { get; set; } // Primary regions covered
        public string? AchievementsAndAwards { get; set; } // Achievements and awards
        public string? ScoutingPhilosophy { get; set; } // Scouting philosophy
        public string? ContactEmail { get; set; } // Contact email

            // First Previous Club
            public string? PreviousClub1Name { get; set; }
            public int? PreviousClub1YearFrom { get; set; }
            public int? PreviousClub1YearTo { get; set; }
            public int? PreviousClub1Appearances { get; set; }
            public int? PreviousClub1Goals { get; set; }
            public int? PreviousClub1CleanSheets { get; set; }

            // Second Previous Club
            public string? PreviousClub2Name { get; set; }
            public int? PreviousClub2YearFrom { get; set; }
            public int? PreviousClub2YearTo { get; set; }
            public int? PreviousClub2Appearances { get; set; }
            public int? PreviousClub2Goals { get; set; }
            public int? PreviousClub2CleanSheets { get; set; }

            // Third Previous Club
            public string? PreviousClub3Name { get; set; }
            public int? PreviousClub3YearFrom { get; set; }
            public int? PreviousClub3YearTo { get; set; }
            public int? PreviousClub3Appearances { get; set; }
            public int? PreviousClub3Goals { get; set; }
            public int? PreviousClub3CleanSheets { get; set; }

            // Can add more clubs if necessary
        

        public IdentityUser User { get; set; } // Navigation property
    }

    
}

