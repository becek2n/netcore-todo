using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TODO.API.Helpers
{
    public class AuthorizationToken
    {
		public static string GenerateToken(int userId, string userName, string fullName, string tokenKey, string tokenIssuer, string tokenAudience)
		{
			//var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
			//var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			//var myIssuer = "http://mysite.com";
			//var myAudience = "http://myaudience.com";

			//var tokenHandler = new JwtSecurityToken();
			//var tokenDescriptor = new SecurityTokenDescriptor
			//{
			//	Subject = new ClaimsIdentity(new Claim[]
			//	{
			//		new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
			//	}),
			//	Expires = DateTime.UtcNow.AddDays(7),
			//	Issuer = myIssuer,
			//	Audience = myAudience,
			//	SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			//};
			//var token = tokenHandler.CreateToken(tokenDescriptor);

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

			var token = new JwtSecurityToken(
				issuer: tokenIssuer,
				audience: tokenAudience,
				claims: new[]
				{
					new Claim(ClaimTypes.Name, userName),
					new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
					new Claim(ClaimTypes.Surname, fullName ?? string.Empty),
				},
				expires: DateTime.UtcNow.AddHours(1),
				notBefore: DateTime.UtcNow,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
