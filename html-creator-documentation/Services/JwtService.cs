using html_creator_documentation.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace html_creator_documentation.Services
{
    public class JwtService
    {
        public string EncodeJwtToken(string login, string password)
        {
            var identity = GetIdentity(login, password);
            if (identity is null) return null;

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public ClaimsPrincipal DecodeJwtToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = AuthOptions.GetTokenValidationParameters();

            ClaimsPrincipal claimsPrincipal = null;
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out _);
            }
            catch (SecurityTokenException)
            {
                Console.WriteLine("Ошибка валидации токена");
            }

            return claimsPrincipal;
        }


        private ClaimsIdentity GetIdentity(string login, string password)
        {
            if (login == "admin" && password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim("access", AuthOptions.ACCESS_KEY)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }
            return null;
        }
    }
}
