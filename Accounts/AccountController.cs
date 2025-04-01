using LabApi.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LabApi.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost("Login")]
        public IActionResult Login(LoginDTO userDTO)
        {
            if (userDTO.UserName=="nader11"&&userDTO.Password=="1234567")
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,userDTO.UserName),
                    new Claim(ClaimTypes.NameIdentifier,userDTO.Password),
                    new Claim(ClaimTypes.Role,"Admin"),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("201b499cb3cf7c611eb045aef9e0001d36caf72c5aa335612bba83013943197da43e4c40771c87eea1e5c8c640a19804563bebc67f17d18fb22b34a4d33f3ff3f793132a3d17b19e842eaa1239db2f0d7e37ff52f42241fbb21f2e8ea1eab84671bb9179b6bde36d83b9cc81a56d496cd0327f71a45ae017183a997ecd713cac7d9a64dab8d76ba19cf8d4399ba5e4886e5ff59356bc73ced9aca54acf79c4964647318b8778c6f953339d0a41cf11d9f28e2acb8754f7b9cbc509a3bf45ee15f6bfd8e9b93bd38872f13b22d0f0ee3b4a34bd16c1048216270939099075cef444e9e20e3ccf19b4525c86a7d5876d046fa95c6953cea3b93eb9952bd36b05d1"));
                SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                JwtSecurityToken myToken = new JwtSecurityToken(
                    issuer: "http://localhost:5272",
                    audience: "",
                    expires: DateTime.Now.AddDays(2),
                    claims: claims,
                    signingCredentials:signingCredentials
                    );
                return Ok(new
                {
                    TokenContext =new JwtSecurityTokenHandler().WriteToken(myToken),
                    expire=myToken.ValidTo
                });
            }
            ModelState.AddModelError("", "the username or password is invalid");
            return BadRequest(ModelState);
        }
    }
}
