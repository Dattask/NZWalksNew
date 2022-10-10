using Microsoft.EntityFrameworkCore;
using NZWalks.API.Controllers.Models.Domain;

namespace NZWalks.API.Data
{
    public class DbContextNZWalks : DbContext
    {
        public DbContextNZWalks(DbContextOptions<DbContextNZWalks> options) :base(options)
        {

        }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }

}
}
