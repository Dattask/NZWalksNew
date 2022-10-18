using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContextNZWalks _dbContextNZWalks;

        public UserRepository(DbContextNZWalks dbContextNZWalks)
        {
            _dbContextNZWalks = dbContextNZWalks;
        }
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user =  await _dbContextNZWalks.Users
                .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower() && x.Password == password);

            if (user == null)
            {
                return null;
            }

            var userRoles = await _dbContextNZWalks.Users_Roles.Where(x => x.UserId == user.Id).ToListAsync();

            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach (var userRole in userRoles)
                {
                    var role = await _dbContextNZWalks.Roles.FirstOrDefaultAsync(x=>x.Id == userRole.RoleId);
                    if (role != null)
                    user.Roles.Add(role.Name);
                }
            }

            user.Password = null;
            return user;
        }
    }
}
