using Microsoft.EntityFrameworkCore;
using NZWalks.API.Controllers.Models.Domain;
using NZWalks.API.Data;

namespace NZWalks.API.Repository
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly DbContextNZWalks _dbContextNZWalks;

        public WalkDifficultyRepository(DbContextNZWalks dbContextNZWalks)
        {
            _dbContextNZWalks = dbContextNZWalks;
        }

        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
        {
            //Add the walkDifficulty object
            walkDifficulty.Id = Guid.NewGuid();
            await _dbContextNZWalks.WalkDifficulty.AddAsync(walkDifficulty);
            await _dbContextNZWalks.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
        {
            //Find the record 
            var existingWalkDifficulty = await _dbContextNZWalks.WalkDifficulty.FindAsync(id);

            //If found, delete and save changes
            if (existingWalkDifficulty != null)
            {
                _dbContextNZWalks.WalkDifficulty.Remove(existingWalkDifficulty);
                await _dbContextNZWalks.SaveChangesAsync();
                return existingWalkDifficulty;
            }
            else { return null; }
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _dbContextNZWalks.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetWalkDifficutlyByIdAsync(Guid id)
        {
            //Pass Id and get the details from Database
            var walkDifficulty = await _dbContextNZWalks.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);

            //check null if not return object
            if (walkDifficulty != null)
            {
                return walkDifficulty;
            }
            else { return null; }
        }

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            //Find the matching resource
            var existingWalkDifficulty = await _dbContextNZWalks.WalkDifficulty.FindAsync(id);

            //check for null 
            if (existingWalkDifficulty != null)
            {
                existingWalkDifficulty.Code = walkDifficulty.Code; //Update new values existing record
                await _dbContextNZWalks.SaveChangesAsync(); //Save changes to Database
                return existingWalkDifficulty;  //return Model
            }
            else { return null; }
        }
    }
}
