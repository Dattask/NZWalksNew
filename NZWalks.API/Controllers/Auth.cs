using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Auth : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHandler _token;

        public Auth(IUserRepository userRepository, ITokenHandler token)
        {
            _userRepository = userRepository;
            _token = token;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginAsync(API.Models.DTO.LoginRequest loginRequest)
        {
            //Validate the incoming request - you can use Fluent Validation
            //Check user is authenticated or not // username and password

            var user = await _userRepository.AuthenticateAsync(loginRequest.Username, loginRequest.Password);
            if (user !=null)
            {
                //Generate JWT token
                var token =  await _token.CreateTokenAsync(user);
                return Ok(token);
            }

            return BadRequest("username or password is invalid");

        }
    }
}
