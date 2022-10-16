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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await _dbContextNZWalks.AddAsync(region);
            await _dbContextNZWalks.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid Id)
        {
            var regionId = await _dbContextNZWalks.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (regionId == null)
            {
                return null;
            }

            //Delete the Region
            _dbContextNZWalks.Regions.Remove(regionId);
            await _dbContextNZWalks.SaveChangesAsync();
            return regionId;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _dbContextNZWalks.Regions.ToListAsync();
        }

        public async Task<Region> GetRegionByIdAsync(Guid Id)
        {
            return await _dbContextNZWalks.Regions.FirstOrDefaultAsync(id => id.Id == Id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await _dbContextNZWalks.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;
            _dbContextNZWalks.SaveChanges();
            return existingRegion;
        }
    }
}
