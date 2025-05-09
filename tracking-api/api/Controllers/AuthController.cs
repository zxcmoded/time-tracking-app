using Microsoft.AspNetCore.Mvc;
using tracking_app.Dto;
using tracking_app.Services;

namespace tracking_app.Controllers;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody] AuthRequestDto authRequestDto)
    {
        try
        {
            _logger.LogInformation("Authenticating user: {AuthRequestDto}", authRequestDto);

            var result = await _authService.Authenticate(authRequestDto.Username, authRequestDto.Password);

            // Invalid credentials return 401
            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}