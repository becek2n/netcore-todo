using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TODO.DTO;
using TODO.Repository.Models;

namespace TODO.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Task, TaskDTO>();
            CreateMap<TaskDTO, Task>();
            CreateMap<TaskEditDTO, Task>();
            CreateMap<User, UserDTO>();
        }
    }
}
