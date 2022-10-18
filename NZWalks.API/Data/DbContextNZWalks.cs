using Microsoft.EntityFrameworkCore;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class DbContextNZWalks : DbContext
    {
        public DbContextNZWalks(DbContextOptions<DbContextNZWalks> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x=>x.Role)
                .WithMany(y=>y.UsersRoles)
                .HasForeignKey(x=>x.RoleId);

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.UsersRoles)
                .HasForeignKey(x => x.UserId);
        }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> Users_Roles { get; set; }
}
}
