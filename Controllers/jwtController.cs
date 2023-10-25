using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Models;

namespace WebApplication2.Controllers;


public class jwtController : Controller
{
    public string jwtKey = "mysupersecret_secretkey!123";

    [HttpPost("/token")]
    public async Task<IActionResult> token([FromBody] TokenRequest request)
    {
        if (jwtKey != request.jwtKey)
        {
            return BadRequest(new { errorText = "Invalid jwt Key" + request.jwtKey + jwtKey });
        }

        var TimeNow = DateTime.UtcNow;
        
        var jwt = new JwtSecurityToken(
            issuer: jwtModel.ISSUER,
            audience: jwtModel.AUDIENCE,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(jwtModel.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        
        Response.Cookies.Append("token", encodedJwt, new CookieOptions
        {
            HttpOnly = true
        });
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity()));

        
        var response = new
        {
            access_token = encodedJwt,
        };
 
        return Json(response);
    }
}