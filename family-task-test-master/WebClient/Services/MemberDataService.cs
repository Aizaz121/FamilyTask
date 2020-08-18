using Domain.Commands;
using Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebClient.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Text;
using Newtonsoft.Json;

namespace WebClient.Services
{
    public class MemberDataService : IMemberDataService
    {
        private HttpClient _httpClient;
        public MemberDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CreateMemberCommandResult> Create(CreateMemberCommand command)
        {            
            return await _httpClient.PostJsonAsync<CreateMemberCommandResult>("members", command);
        }

        public async Task<GetAllMembersQueryResult> GetAllMembers()
        {
            return await _httpClient.GetJsonAsync<GetAllMembersQueryResult>("members");
        }

        public async Task<UpdateMemberCommandResult> Update(UpdateMemberCommand command)
        {
            return await _httpClient.PutJsonAsync<UpdateMemberCommandResult>($"members/{command.Id}", command);
        }

        public async Task<DeleteMemberCommandResult> Delete(Guid id)
        {
           // var a= await HttpClientExtensions.DeleteAsJsonAsync<DeleteMemberCommandResult>(_httpClient, $"members/{id}",new DeleteMemberCommandResult());
            
          
            return await _httpClient.PostJsonAsync<DeleteMemberCommandResult>($"members/{id}",null);
        }
    }

    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
        => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}
