using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO.DTO;

namespace TODO.Interfaces
{
    public interface IUser
    {
        Task<ResultModel<UserDTO>> Authenticate(string username, string password);
        Task<ResultModel<object>> SaveToken(TokenRefreshDTO model);
        Task<ResultModel<object>> UpdateToken(string token);
    }
}
