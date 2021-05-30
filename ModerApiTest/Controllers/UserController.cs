using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModerApiTest.Managers;
using ModerApiTest.Models;
using ModerApiTest.Utils;

namespace ModerApiTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserManager _userManager;

        /// <summary>
        /// Constructor called only by the framework
        /// </summary>
        /// <param name="userManager">built by dependency injection</param>
        public UserController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// GetUserInfo http handler for endpoint GET 'api/user/{id}
        /// </summary>
        /// <param name="id">the user id</param>
        /// <returns>the response</returns>
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUserInfo(string id)
        {
            var userDocument = _userManager.GetUserInfo(id);
            if (userDocument == null)
            {
                return NotFound(new { message = "User not found" });
            }
            else
            {
                return Ok(_userManager.GetUserResponse("User found", userDocument));
            }
        }

        /// <summary>
        /// UpdateUser http handler for endpoint PUT 'api/user/{id}'
        /// </summary>
        /// <param name="id">the user id</param>
        /// <param name="userModel">the user informations</param>
        /// <returns>the response</returns>
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UserModel userModel)
        {
            if (!id.Equals(HttpContext.GetCurrentUserId()))
            {
                // a user can update only its own informations
                return Unauthorized(new { message = "Access denied" });
            }

            var userDocument = _userManager.UpdateUser(id, userModel);
            if (userDocument == null)
            {
                return NotFound(new { message = "User not found" });
            }
            else
            {
                return Ok(_userManager.GetUserResponse("User updated", userDocument));
            }
        }

        /// <summary>
        /// DeleteUser http handler for endpoint DELETE 'api/user/{id}'
        /// </summary>
        /// <param name="id">the user id</param>
        /// <returns>the response</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            if (!id.Equals(HttpContext.GetCurrentUserId()))
            {
                // a user can update only its own informations
                return Unauthorized(new { message = "Access denied" });
            }

            var userDocument = _userManager.FindUser(id);
            if (userDocument == null)
            {
                return NotFound(new { message = "User not found" });
            }
            else if (_userManager.DeleteUser(userDocument))
            {
                return Ok(_userManager.GetUserResponse("User deleted", userDocument));
            }
            else
            {
                return Problem();
            }
        }
    }
}
