using Domain.Commands;
using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Domain.Commands;
using Domain.DataModels;
using Domain.ViewModel;

namespace WebApi.AutoMapper
{
    public class TaskMapper : Profile
    {
        public TaskMapper()
        {
            CreateMap<CreateTasksCommand, Task>();
            CreateMap<UpdateTasksCommand, Task>();
            CreateMap<Task, Task>();
        }
    }
}
