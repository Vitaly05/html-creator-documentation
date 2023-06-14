using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace html_creator_documentation.Data
{
    public class AuthOptions
    {
        public const string ISSUER = "HTMLCreatorLibraryDocumentation";
        public const string AUDIENCE = "HTMLCreatorLibraryDocumentationClient";

        const string KEY = "JJVKSDJ2eiouet[sd)**@LKkdjfkekegfjg";
        public const string ACCESS_KEY = "ew349990KJJF[-wjhfj37jwfjzsh_+#WdfdfecvV";

        public const int LIFETIME = 30;

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = ISSUER,

                ValidateAudience = true,
                ValidAudience = AUDIENCE,

                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = true,

                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
