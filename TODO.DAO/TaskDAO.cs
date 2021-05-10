using AutoMapper;
using System;
using System.Collections.Generic;
using TODO.DTO;
using TODO.Interfaces;
using TODO.Repository.Models;
using System.Linq;
using AutoMapper.QueryableExtensions;
using TODO.Helpers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TODO.DAO
{
    public class TaskDAO : ITask
    {
        private readonly tododbContext _context;
        private readonly ILogging _logging;
        private readonly IMapper _mapper;
        public TaskDAO(tododbContext context, ILogging logging, IMapper mapper) {
            _context = context;
            _logging = logging;
            _mapper = mapper;
        }
        public ResultModel<object> Add(TaskDTO model)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var task = _mapper.Map<TaskDTO, Repository.Models.Task>(model);

                        _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        _context.SaveChanges();

                        transaction.Commit();
                        result.SetSuccess("Data successfuly added");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.ToString());
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

        public ResultModel<object> Delete(int id)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var data = _context.Tasks
                            .Where(x => x.Id == id)
                            .FirstOrDefault();

                        if (data != null)
                        {
                            _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                            _context.SaveChanges();
                            transaction.Commit();
                            result.SetSuccess("Data successfully deleted");
                        }
                        else { 
                            result.SetSuccess("Data is not found!");
                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.ToString());
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

        public ResultModel<PagedResult<TaskDTO>> GetAll(int pageIndex, int pageSize, string search)
        {
            ResultModel<PagedResult<TaskDTO>> result = new ResultModel<PagedResult<TaskDTO>>();
            try
            {

                var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

                var data = _context.Tasks
                    .Where(x => x.Name.Contains(search ?? ""))
                    .ProjectTo<TaskDTO>(mapperConfig)
                    .GetPage(pageIndex, pageSize);
                
                result.SetSuccess("success", data);

            }
            catch (Exception ex)
            {
                result.SetFailed(ex.Message);
                _logging.WriteErr(ex);
            }

            return result;
        }

        public async Task<TaskDTO> GetId(int id)
        {
            //ResultModel<TaskDTO> result = new ResultModel<TaskDTO>();
            //try
            //{
                var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

                var data = await _context.Tasks
                    .Where(x => x.Id == id)
                    .ProjectTo<TaskDTO>(mapperConfig)
                    .FirstOrDefaultAsync();

                //result.SetSuccess("success", data);

                return data;
            //}
            //catch (Exception ex)
            //{
            //    result.SetFailed(ex.Message);
            //    _logging.WriteErr(ex);
            //}

            //return result;
        }

        public ResultModel<object> Update(int id, TaskEditDTO model)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var data = _context.Tasks.Where(x => x.Id == id).FirstOrDefault();
                        if (data != null)
                        {
                            var task = _mapper.Map(model, data);

                            _context.Entry(task).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();

                            transaction.Commit();
                            result.SetSuccess("Data successfuly updated");
                        }
                        else { 
                            result.SetSuccess("Data is not found!");

                        }

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.ToString());
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
