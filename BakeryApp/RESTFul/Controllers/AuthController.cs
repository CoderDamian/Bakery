using AppliationService.Contracts;
using DataTransferObjects.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace RESTFul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userervice)
        {
            this._userService = userervice;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            var userExists = await _userService.ValidateUserExistence(userDTO).ConfigureAwait(false);

            if (userExists)
            {
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}