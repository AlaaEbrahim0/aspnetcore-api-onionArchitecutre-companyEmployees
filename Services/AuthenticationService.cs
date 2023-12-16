using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DTOs;
using Shared.Options;

namespace Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly IMapper _mapper;

    public AuthenticationService(UserManager<AppUser> userManager, IMapper mapper, IOptions<JwtOptions> options)
    {
        _userManager = userManager;
        _mapper = mapper;
        _jwtOptions = options.Value;
    }

    public async Task<AuthDto> Register(RegisterationDto registerationDto)
    {
        var authDto = new AuthDto();

        var user = await _userManager.FindByEmailAsync(registerationDto.Email);
        if (user is not null)
        {
            authDto.Message = "Email Address is already used";
            return authDto;
        }

        user = _mapper.Map<AppUser>(registerationDto);
        var result = await _userManager.CreateAsync(user);

        var sb = new StringBuilder();

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                sb.AppendLine($"{error.Code}: {error.Description}");
            }

            authDto.Message = sb.ToString();
            authDto.IsAuthenticated = false;
            return authDto;
        }

        await _userManager.AddToRolesAsync(user, registerationDto.Roles);
        var token = await CreateToken(user);

        authDto.Message = $"User: [{user.Email}] has been created successfully";
        authDto.IsAuthenticated = true;
        authDto.ExpiresOn = token.ValidTo;
        authDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
        authDto.Roles = registerationDto.Roles!.ToList();

        return authDto;
    }

    private async Task<JwtSecurityToken> CreateToken(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roleClaims = new List<Claim>();

        foreach (var role in roles)
        {
            roleClaims.Add(new Claim("roles", role));
        }

        var jwtClaims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Iss, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Email, Guid.NewGuid().ToString()),
            new Claim("uid", user.Id)
        }
            .Union(userClaims)
            .Union(roleClaims);

        var x = _jwtOptions.Key;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
        (
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: jwtClaims.ToList(),
            expires: DateTime.Now.AddDays(_jwtOptions.Duration),
            signingCredentials: credentials
        );

        return token;
    }
}
