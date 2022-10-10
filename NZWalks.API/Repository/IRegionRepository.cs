using NZWalks.API.Controllers.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
