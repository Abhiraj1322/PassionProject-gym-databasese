using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymDatabase.Models;
namespace GymDatabase.Data
{
    public class ApplicationDbContext : IdentityDbContext
    { 
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Memebrship_payment> Memberships_payment { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<MemeberTraining> MemberTrainings { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
