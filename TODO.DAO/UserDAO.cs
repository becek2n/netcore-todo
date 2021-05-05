using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO.DTO;
using TODO.Interfaces;
using TODO.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace TODO.DAO
{
    public class UserDAO : IUser 
    {
        private readonly tododbContext _context;
        private readonly ILogging _logging;
        private readonly IMapper _mapper;
        public UserDAO(tododbContext context, ILogging logging, IMapper mapper) {
            _context = context;
            _logging = logging;
            _mapper = mapper;
        }

        public async Task<ResultModel<UserDTO>> Authenticate(string username, string password)
        {
            ResultModel<UserDTO> result = new ResultModel<UserDTO>(); 
            try
            {
                var data = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();

                if (data != null)
                {

                    //verify password
                    if (BCrypt.Net.BCrypt.Verify(password, data.Password))
                    {

                        var model = _mapper.Map<UserDTO>(data);
                        result.SetSuccess("success", model);
                    }
                    else
                    {
                        result.SetFailed("Incorrect username or password!");
                    }
                }
                else { 
                
                    result.SetFailed("User not found!");
                
                }

            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message);
                _logging.WriteErr(ex);
            }

            return result;
        }
    }
}
