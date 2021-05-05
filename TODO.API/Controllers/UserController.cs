using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TODO.API.Helpers;
using TODO.DTO;
using TODO.Interfaces;
using TODO.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TODO.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly IUser _user;
        private readonly tododbContext _context;
        private readonly TokenValidationParameters _tokenValidationParams;
        private readonly IConfiguration _configuration;
        public UserController(ILogger<TaskController> logger, IUser user, 
            tododbContext context, TokenValidationParameters tokenValidationParams, IConfiguration configuration) {
            _logger = logger;
            _user = user;
            _context = context;
            _tokenValidationParams = tokenValidationParams;
            _configuration = configuration;
        }

        [HttpPost, Route("Login")]
        public async Task<ActionResult> Post(UserRequestDTO model) {

            _logger.LogRequest("Requesting user login for user: {UserName}, password: {Password}", model?.Username, model?.Password);

            var user = await _user.Authenticate(model.Username, model.Password);

            if (user.ResponseCode != "200") {
                _logger.LogError("Error authentication for user: {UserName}, password: {Password}", model?.Username, model?.Password);
                return StatusCode(500, user.ResponseMessage);
            }

            if (user.ResponseData == null)
            {
                _logger.LogWarning("Not found for user: {UserName}, password: {Password}", model?.Username, model?.Password);
                return BadRequest(user);
            }

            _logger.LogResponse("Success authentication for user: {UserName}, password: {Password}", model?.Username, model?.Password);

            //generate token 

            _logger.LogRequest("Request generate token");
            var tokenJWT = await GenerateToken(user.ResponseData.Id, user.ResponseData.Username, user.ResponseData.Fullname);

            _logger.LogResponse("Retrieve generate token {Token}", tokenJWT.Token);

            return Ok(tokenJWT);

        }
        [HttpPost, Route("RefreshToken")]
        public async Task<ActionResult> Refresh([FromBody] TokenRequestDTO token) {
            var tokenJWT = await VerifyToken(token);
            return Ok(tokenJWT);
        }

        public async Task<AuthResultDTO> GenerateToken(int userId, string userName, string fullName)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Security:Tokens:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Security:Tokens:Issuer"],
                audience: _configuration["Security:Tokens:Audience"],
                claims: new[]
                {
                    new Claim(type: "Id", value: userId.ToString()),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Surname, fullName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                },
                expires: DateTime.UtcNow.AddMinutes(2),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var tokenRefresh = new TokenRefreshDTO()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = userId,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(1),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()
            };

            await _user.SaveToken(tokenRefresh);

            return new AuthResultDTO()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = tokenRefresh.Token
            };
        }

        public async Task<AuthResultDTO> VerifyToken(TokenRequestDTO tokenRequest)
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
                var storedToken = await _context.TokenRefreshes.FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);

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

                var data = await _user.UpdateToken(tokenRequest.RefreshToken);

                if (data.ResponseCode != "200")
                {
                    return new AuthResultDTO()
                    {
                        Success = false,
                        Errors = new List<string>() {
                                data.ResponseMessage
                            }
                    };
                }
                // Generate a new token
                var user = await _context.Users.FindAsync(storedToken.UserId);
                return await GenerateToken(user.Id, user.Username, user.Fullname);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Lifetime validation failed. The token is expired."))
                {

                    //validate token refresh
                    var storedToken = await _context.TokenRefreshes
                        .FirstOrDefaultAsync(x => x.Token == tokenRequest.RefreshToken);
                    
                    var data = await _user.UpdateToken(tokenRequest.RefreshToken);
                    
                    if (data.ResponseCode != "200") {
                        return new AuthResultDTO()
                        {
                            Success = false,
                            Errors = new List<string>() {
                                data.ResponseMessage
                            }
                        };
                    } 

                    var user = await _context.Users.FindAsync(storedToken.UserId);
                    return await GenerateToken(user.Id, user.Username, user.Fullname);
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


        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
