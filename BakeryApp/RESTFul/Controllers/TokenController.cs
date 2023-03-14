using AppliationService.Contracts;
using DomainModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace RESTFul.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public TokenController(ITokenService tokenService, IUserService userService)
        {
            this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            this._userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody]Token token)
        {
            if (token is null)
                return BadRequest("Invalid client request");

            Token newToken = await _tokenService.GetNewRefreshToken(token.Value, token.Refresh)
                .ConfigureAwait(false);

            return Ok(newToken);
        }
    }
}
