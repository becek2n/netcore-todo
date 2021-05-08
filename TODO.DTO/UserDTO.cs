using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODO.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
    }

    public class UserRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthResultDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }

    public class TokenRequestDTO {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class TokenRefreshDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; } // Map the token with jwtId
        public bool IsUsed { get; set; } // if its used we dont want generate a new Jwt token with the same refresh token
        public bool IsRevoked { get; set; } // if it has been revoke for security reasons
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; } // Refresh token is long lived it could last for months.

    }

    public class RequestResult {
        public string State { get; set; }
        public string Msg { get; set; }
    }
}
