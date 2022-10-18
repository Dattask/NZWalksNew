using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public interface ITokenHandler
    {
        Task<string> CreateTokenAsync(User user);
    }
}
