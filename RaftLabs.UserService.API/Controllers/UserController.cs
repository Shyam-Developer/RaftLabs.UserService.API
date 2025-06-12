using Microsoft.AspNetCore.Mvc;
using UserServiceLibrary.Interfaces;
using UserServiceLibrary.Models;

namespace RaftLabs.UserService.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        #region Fields
        /// <summary>
        /// Service to interact with external user data source.
        /// </summary>
        private readonly IExternalUserService _userService;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IExternalUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Retrieves a user by their ID from the external user service.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserById{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all users from the external user service.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion
    }
}
