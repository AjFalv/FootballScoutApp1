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
    }
}
