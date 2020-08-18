using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;

namespace WebClient.Services
{
    public class TasksDataService : ITasksDataService
    {
        private HttpClient _httpClient;
        public TasksDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateTasksCommandResult> Create(CreateTasksCommand command)
        {
            return await _httpClient.PostJsonAsync<CreateTasksCommandResult>("Tasks", command);
        }

        public async Task<GetAllTasksQueryResult> GetAllTasks()
        {
            return await _httpClient.GetJsonAsync<GetAllTasksQueryResult>("Tasks");
        }

        public async Task<UpdateTasksCommandResult> Update(UpdateTasksCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateTasksCommandResult>($"Tasks/{command.Id}", command);
        }

        public async Task<DeleteTasksCommandResult> Delete(Guid id)
        {
            // var a= await HttpClientExtensions.DeleteAsJsonAsync<DeleteTasksCommandResult>(_httpClient, $"Taskss/{id}",new DeleteTasksCommandResult());


            return await _httpClient.PostJsonAsync<DeleteTasksCommandResult>($"Tasks/{id}", null);
        }
    }
}
