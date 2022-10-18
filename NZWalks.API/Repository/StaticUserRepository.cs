using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class StaticUserRepository : IUserRepository
    {
        //Create list of users

        private List<User> Users = new List<User>()
        {
            //new User{ Username ="Datta", Password="Datta", FirstName ="Datta", LastName ="K", Email="Dk@gmail.com", Id= new Guid(),
            //    Roles = new List<string>(){ "reader"  }
            //},
            //new User{
            //    Username ="DattaK", Password="DattaK", FirstName ="Dattak", LastName ="K", Email="Dkk@gmail.com", Id= new Guid(),
            //    Roles = new List<string>(){ "reader", "writer" }
            //}
        };

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
           x.Password == password);

            return user;
        }
    }
}
