using Core.Abstractions.Repositories;
using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class TasksRepository : BaseRepository<Guid, Task, TasksRepository>, ITasksRepository
    {
        public TasksRepository(FamilyTaskContext context) : base(context)
        { }



        ITasksRepository IBaseRepository<Guid, Task, ITasksRepository>.NoTrack()
        {
            return base.NoTrack();
        }

        ITasksRepository IBaseRepository<Guid, Task, ITasksRepository>.Reset()
        {
            return base.Reset();
        }

    }
}
