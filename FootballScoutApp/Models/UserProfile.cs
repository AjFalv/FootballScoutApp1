using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballScoutApp.Models
{
    public class UserProfile
    {
        public string Id { get; set; } // Primary key and foreign key to AspNetUsers
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Nationality { get; set; } // New Field: Nationality
        public string? Birthplace { get; set; } // New Field: Birthplace
        public string? Bio { get; set; }
        public string? Position { get; set; } // For players
        public int? Experience { get; set; } // For players
        public string? Expertise { get; set; } // For scouts
        public string? PreferredFoot { get; set; } // Preferred Foot
        public int? Height { get; set; } // Height in cm
        public int? Weight { get; set; } // Weight in kg
        public string? InjuryHistory { get; set; } // Injury History
        public List<PreviousClub> PreviousClubs { get; set; } // Previous Clubs

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
