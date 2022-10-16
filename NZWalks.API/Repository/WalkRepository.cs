using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Data;

namespace NZWalks.API.Repository
{
    public class WalkRepository : IWalkRepository
    {
        private readonly DbContextNZWalks _dbContextNZWalks;

        public WalkRepository(DbContextNZWalks dbContextNZWalks)
        {
            _dbContextNZWalks = dbContextNZWalks;
        }

        public async Task<Walk> AddWalkAsync(Walk walk)
        {
            //Create Unique ID and assign it.
            walk.Id = Guid.NewGuid();
            await _dbContextNZWalks.Walks.AddAsync(walk);
            _dbContextNZWalks.SaveChanges();
            return walk;
        }

        public async Task<Walk> DeleteWalkByIdAsync(Guid id)
        {
            var existingWalk = await _dbContextNZWalks.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                _dbContextNZWalks.Walks.Remove(existingWalk);
                await _dbContextNZWalks.SaveChangesAsync();
                return existingWalk;
            }
            else
            { return null; }
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await _dbContextNZWalks.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetWalkByIdAsync(Guid id)
        {
            return await _dbContextNZWalks.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);


        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            //check the given Id/Data is available in DB.
            var existingWalk = await _dbContextNZWalks.Walks.FindAsync(id);

            //If exists, update the record else null.
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Length = walk.Length;
            existingWalk.Name = walk.Name;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.RegionId = walk.RegionId;
            await _dbContextNZWalks.SaveChangesAsync();

            //return the response
            return existingWalk;
        }
    }
}
