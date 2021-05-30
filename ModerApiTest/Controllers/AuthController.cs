using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModerApiTest.Models;
using ModerApiTest.Utils;
using ModerApiTest.Managers;

namespace ModerApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthManager _authManager;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="authManager">built by dependency injection</param>
        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        /// <summary>
        /// Login http handler for endpoint POST 'api/auth/login'
        /// </summary>
        /// <param name="login">the login informations</param>
        /// <returns>the response</returns>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            var jwt = _authManager.Login(login);
            if (jwt.HasValue)
            {
                return Ok(_authManager.GetLoggedInResponse(jwt.Value));
            }
            return Unauthorized(new { message = "Wrong credentials" });
        }

        /// <summary>
        /// Logout http handler for endpoint GET 'api/auth/logout'
        /// </summary>
        /// <returns>the response</returns>
        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _authManager.Logout(HttpContext.GetBearerToken());
            return Ok("Logged out");
        }

        /// <summary>
        /// Register http handler for endpoint POST 'api/auth/register'
        /// </summary>
        /// <param name="user">the user informations</param>
        /// <returns>the response</returns>
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            var userDocument = _authManager.GetRegisteredUser(user);
            if (userDocument == null)
            {
                return Ok(_authManager.Register(user));
            }
            else
            {
                return BadRequest(_authManager.GetAlreadyRegisteredResponse(userDocument.UserId.ToString() , user));
            }
        }
    }
}
