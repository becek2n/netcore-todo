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

        public async Task<ResultModel<object>> SaveToken(TokenRefreshDTO model) {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                using (var transaction = _context.Database.BeginTransaction()) {
                    try
                    {
                        var token = _mapper.Map<TokenRefreshDTO, TokenRefresh>(model);

                        await _context.TokenRefreshes.AddAsync(token);
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        result.SetSuccess("success");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message);
                _logging.WriteErr(ex);
            }

            return result;
        }

        public async Task<ResultModel<object>> UpdateToken(string tokenRefresh)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var data = await _context.TokenRefreshes
                            .Where(x => x.Token == tokenRefresh && x.IsUsed == false)
                            .FirstOrDefaultAsync();

                        if (data != null) {

                            data.IsUsed = true;
                            _context.Entry(data).State = EntityState.Modified;
                            await _context.SaveChangesAsync();

                            transaction.Commit();
                            result.SetSuccess("success");
                        }
                        else {

                            result.SetFailed("Token not found!");
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
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
