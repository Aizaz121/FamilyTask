using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Abstractions
{
    public interface ITasksDataService
    {
        public Task<CreateTasksCommandResult> Create(CreateTasksCommand command);
        public Task<UpdateTasksCommandResult> Update(UpdateTasksCommand command);

        public Task<DeleteTasksCommandResult> Delete(Guid command);
        public Task<GetAllTasksQueryResult> GetAllTasks();
    }
}
