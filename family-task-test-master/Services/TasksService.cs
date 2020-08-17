using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.DataModels;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class TasksService : ITasksService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;

        public TasksService(IMapper mapper, ITasksRepository taskRepository)
        {
            _mapper = mapper;
            _tasksRepository = taskRepository;
        }
        public async Task<CreateTasksCommandResult> CreateTasksCommandHandler(CreateTasksCommand command)
        {
            var member = _mapper.Map<Domain.DataModels.Task>(command);
            var persistedMember = await _tasksRepository.CreateRecordAsync(member);

            var vm = _mapper.Map<Domain.DataModels.Task>(persistedMember);

            return new CreateTasksCommandResult()
            {
                Payload = vm
            };
        }

        public async Task<UpdateTasksCommandResult> UpdateTasksCommandHandler(UpdateTasksCommand command)
        {
            var isSucceed = true;
            var member = await _tasksRepository.ByIdAsync(command.Id);

            _mapper.Map<UpdateTasksCommand, Domain.DataModels.Task>(command, member);

            var affectedRecordsCount = await _tasksRepository.UpdateRecordAsync(member);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new UpdateTasksCommandResult()
            {
                Succeed = isSucceed
            };
        }


        public async Task<DeleteTasksCommandResult> DeleteTasksCommandHandler(Guid command)
        {
            var isSucceed = true;

            var affectedRecordsCount = await _tasksRepository.DeleteRecordAsync(command);

            if (affectedRecordsCount < 1)
                isSucceed = false;

            return new DeleteTasksCommandResult()
            {
                Succeed = isSucceed
            };
        }
        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler()
        {
            IEnumerable<Domain.DataModels.Task> vm = new List<Domain.DataModels.Task>();

            var members = await _tasksRepository.Reset().ToListAsync();

            if (members != null && members.Any())
                vm = _mapper.Map<IEnumerable<Domain.DataModels.Task>>(members);

            return new GetAllTasksQueryResult()
            {
                Payload = vm
            };
        }

    
}
}
