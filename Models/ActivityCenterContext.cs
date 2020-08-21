using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class ActivityCenterContext : DbContext
    {
        public ActivityCenterContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Plan> Plans {get;set;}
        public DbSet<Participant> Participants {get;set;}
    }
}