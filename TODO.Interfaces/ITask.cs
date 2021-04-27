using System;
using System.Collections.Generic;
using TODO.DTO;

namespace TODO.Interfaces
{
    public interface ITask
    {
        ResultModel<object> Add(TaskDTO model);
        ResultModel<object> Update(int id, TaskEditDTO model);
        ResultModel<object> Delete(int id);
        ResultModel<PagedResult<TaskDTO>> GetAll(int pageIndex, int pageSize, string search);
    }
}
