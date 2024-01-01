using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Test.Controllers
{
    [Route("api")]
    public class AuthController : Controller
    {
        private readonly Dictionary<string, string> userCredentials = new();
        private readonly WebSocket _socket;

        public AuthController(WebSocket socket)
        {
            userCredentials.Add("anuj", "asdf1234");
            _socket = socket;
        }

        [HttpPost("auth")]
        public async Task<ActionResult<string>> Authenticate(string username, string password)
        {
            if (userCredentials.ContainsKey(username) && userCredentials[username] == password)
            {
                var token = GenerateJwtToken(username);
                return  new OkObjectResult(token);
            }

            return Unauthorized("Invalid username or password");
        }

        [HttpPost("add/{token}")]
        public ActionResult<bool> RedirectWithToken(string token)
        {
            if (!string.IsNullOrEmpty(token) && VerifyToken(token))
            {
                _socket.AddToken(token);
                return true;
            }
            else
            {
                return false;
            }

        }

        private static bool VerifyToken(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            return jwtToken.ValidTo > DateTime.UtcNow;
        }

        private static string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("oLOMxbL81N3ouIWWAb6AvkU6V88l0hOF")); 

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user"),
            };

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            

            var access_token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddYears(5),
                signingCredentials: cred
            );

            return new JwtSecurityTokenHandler().WriteToken(access_token);
        }
    }
}
