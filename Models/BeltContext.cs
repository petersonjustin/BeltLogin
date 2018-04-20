using Microsoft.EntityFrameworkCore;

namespace BeltLogin.Models
{
    public class BeltContext : DbContext
    {
        // Other code

        public DbSet<User> Users { get; set; }
        public DbSet<Activitie> Activities { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }
    }
}
