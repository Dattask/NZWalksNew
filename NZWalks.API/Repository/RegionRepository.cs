using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Data;

namespace NZWalks.API.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly DbContextNZWalks _dbContextNZWalks;

        public RegionRepository(DbContextNZWalks dbContextNZWalks)
        {
             _dbContextNZWalks = dbContextNZWalks;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _dbContextNZWalks.Regions.ToListAsync();
        }
    }
}
