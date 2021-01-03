using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ChatApp.Controllers
{
    public class LoginCredentials
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private static readonly SigningCredentials SigningCreds = new (Startup.SecurityKey, SecurityAlgorithms.HmacSha256);
        private readonly JwtSecurityTokenHandler _tokenHandler = new ();

        [HttpGet("context")]
        public JsonResult Context()
        {
            return Json(new
            {
                name = User?.Identity.Name,
                email = User?.FindFirstValue(ClaimTypes.Email),
                role = User?.FindFirstValue(ClaimTypes.Role),
            });
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody]LoginCredentials creds)
        {
            if (!ValidateLogin(creds))
            {
                return Json(new
                {
                    error = "Login failed"
                });
            }

            var principal = GetPrincipal(creds, JwtBearerDefaults.AuthenticationScheme);

            var token = new JwtSecurityToken(
                "soSignalR",
                "soSignalR",
                principal.Claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: SigningCreds);

            return Json(new
            {
                token = _tokenHandler.WriteToken(token),
                name = principal.Identity.Name,
                email = principal.FindFirstValue(ClaimTypes.Email),
                role = principal.FindFirstValue(ClaimTypes.Role)
            });
        }
        
        private static bool ValidateLogin(LoginCredentials creds)
        {
            return true;
        }

        private static ClaimsPrincipal GetPrincipal(LoginCredentials creds, string authScheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, creds.Email),
                new Claim(ClaimTypes.Name, creds.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims, authScheme));
        }
    }
}