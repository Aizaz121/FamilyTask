using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Services
{
    public interface ITasksService
    {
         Task<CreateTasksCommandResult> CreateTasksCommandHandler(CreateTasksCommand command);
        Task<UpdateTasksCommandResult> UpdateTasksCommandHandler(UpdateTasksCommand command);
        Task<DeleteTasksCommandResult> DeleteTasksCommandHandler(Guid command);
        Task<GetAllTasksQueryResult> GetAllTasksQueryHandler();
    }
}
