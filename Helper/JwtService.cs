using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PostgresApplication.Helper
{
    public class JwtService
    {

        private static readonly string key = "qwertyuiopasdfghjklzxcvbnmqwertyuiopasdfghjklzxcvbnm";
        private static readonly SymmetricSecurityKey SymmetricSecuritySingingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        public string TokenGenerator(string Email, string FirstName, string LastName)
        {

            var credentials = new SigningCredentials(SymmetricSecuritySingingKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(credentials);

            DateTime Expire = DateTime.UtcNow.AddHours(1);
            int ts = (int)(Expire - new DateTime(1970, 1, 1)).TotalSeconds;

            var payload = new JwtPayload
            {
                {"sub", "MyService" },
                {"name", "somesh" },
                {"email", Email },
                {"exp", ts },
                {"iss", "https://localhost:44372" },
                {"aud", "https://localhost:44372" }
            };

            var securityToken = new JwtSecurityToken(header, payload);

            var handler = new JwtSecurityTokenHandler();

            var TokenString = handler.WriteToken(securityToken);

            return TokenString;
        }
    }
}
