using AppliationService.Contracts;
using DataTransferObjects.DTOs.User;
using DomainModel;
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
            if (userDTO == null)
                return BadRequest();

            Token? token = await _userService.ValidateCredentials(userDTO).ConfigureAwait(false);

            if (token == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(token);
            }
        }
    }
}