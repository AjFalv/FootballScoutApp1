using FootballScoutApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballScoutApp.Data
{
    public class ApplicationDBContext : IdentityDbContext 
    {
        public ApplicationDBContext(DbContextOptions options) : base(options) 
        {
        }

        public DbSet<PlayerListEntity> PlayerList { get; set; }
    }
}
