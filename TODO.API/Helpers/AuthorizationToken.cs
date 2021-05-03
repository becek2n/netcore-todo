using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODO.DTO;
using TODO.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TODO.API.Helpers
{
    public class AuthorizationToken
    {
		private readonly tododbContext _context;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IConfiguration _configuration;

        public AuthorizationToken(tododbContext context, TokenValidationParameters tokenValidationParams, IConfiguration configuration) {
			_context = context;
            _tokenValidationParams = tokenValidationParams;
            _configuration = configuration;
		}
		public async Task<AuthResultDTO> GenerateToken(int userId, string userName, string fullName)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Tokens:Key"]));

			var token = new JwtSecurityToken(
				issuer: _configuration["Security:Tokens:Issuer"],
				audience: _configuration["Security:Tokens:Audience"],
				claims: new[]
				{
					new Claim(ClaimTypes.Name, userName),
					new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
					new Claim(ClaimTypes.Surname, fullName ?? string.Empty),
				},
				expires: DateTime.UtcNow.AddSeconds(30),
				notBefore: DateTime.UtcNow,
				signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
			);

			var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

			var tokenRefresh = new TokenRefresh() {
				JwtId = token.Id,
				IsUsed = false,
				UserId = userId,
				AddedDate = DateTime.UtcNow,
				ExpiryDate = DateTime.UtcNow.AddYears(1),
				IsRevoked = false,
				Token = RandomString(25) + Guid.NewGuid()
			};

			await _context.TokenRefreshes.AddAsync(tokenRefresh);
			await _context.SaveChangesAsync();

			return new AuthResultDTO() {
				Token = jwtToken,
				Success = true,
				RefreshToken = tokenRefresh.Token
			};
		}

        private async Task<AuthResultDTO> VerifyToken(TokenRequestDTO tokenRequest)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validation 1 - Validation JWT token format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out var validatedToken);

                // Validation 2 - Validate encryption alg
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Validation 3 - validate expiry date
                var utcExpiryDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has not yet expired"
                        }
                    };
                }

                // validation 4 - validate existence of the token
                var storedToken = await _context.TokenRefreshes.FirstOrDefaultAsync(x => x.Token == tokenRequest.Token);

                if (storedToken == null)
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token does not exist"
                        }
                    };
                }

                // Validation 5 - validate if used
                if (storedToken.IsUsed)
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been used"
                        }
                    };
                }

                // Validation 6 - validate if revoked
                if (storedToken.IsRevoked)
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has been revoked"
                        }
                    };
                }

                // Validation 7 - validate the id
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token doesn't match"
                        }
                    };
                }

                // update current token 

                storedToken.IsUsed = true;
                _context.TokenRefreshes.Update(storedToken);
                await _context.SaveChangesAsync();

                // Generate a new token
                var user = await _context.Users.FindAsync(storedToken.UserId);
                return await GenerateToken(user.Id, user.Username, user.Fullname);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Token has expired please re-login"
                        }
                    };

                }
                else
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                            "Something went wrong."
                        }
                    };
                }
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            return dtDateTime;
        }


        public string RandomString(int length)
		{
			var random = new Random();
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());
		}
	}
}
