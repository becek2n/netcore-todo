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
}
