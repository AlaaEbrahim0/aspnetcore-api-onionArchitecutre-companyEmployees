using CompanyEmployees.Presentation.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DTOs;

namespace CompanyEmployees.Presentation.Controllers;
[ApiController]
[Route("api/auth/")]
public class AuthenticationController: ControllerBase
{
    private readonly IServiceManager _serviceManager;
    public AuthenticationController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("register")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Register(RegisterationDto registerationDto)
    {
        var result = await _serviceManager.AuthenticationService.Register(registerationDto);
        if (!result.IsAuthenticated)
        {
            return BadRequest(result.Message);
        }
        return StatusCode(201, result);
    }
}
