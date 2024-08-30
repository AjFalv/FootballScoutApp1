using FootballScoutApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballScoutApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) 
        {
        }

        public DbSet<PlayerListEntity> PlayerList { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        //public DbSet<PreviousClub> PreviousClubs { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserProfile
            modelBuilder.Entity<UserProfile>()
                .HasKey(up => up.Id);

            modelBuilder.Entity<UserProfile>()
                .HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete to avoid multiple cascade paths

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete to avoid multiple cascade paths

            
        }
    }
}
