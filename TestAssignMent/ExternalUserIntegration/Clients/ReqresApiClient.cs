using ExternalUserIntegration.Configuration;
using ExternalUserIntegration.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUserIntegration.Clients
{
public class ReqresApiClient : IReqresApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ReqresApiOptions _options;

    public ReqresApiClient(HttpClient httpClient, IOptions<ReqresApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<ReqresUserResponse.UserData> GetUserByIdAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"{_options.BaseUrl}/users/{userId}");

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Failed to fetch user. StatusCode: {response.StatusCode}");

        var userResponse = await response.Content.ReadFromJsonAsync<ReqresUserResponse>();

        if (userResponse?.Data == null)
            throw new Exception("Invalid user response format");

        return userResponse.Data;
    }

    public async Task<List<ReqresUserResponse.UserData>> GetAllUsersAsync()
    {
        var users = new List<ReqresUserResponse.UserData>();
        int page = 1;
        ReqresListUsersResponse? pageResponse;
                                                                
        do
        {
            var response = await _httpClient.GetAsync($"{_options.BaseUrl}/users?page={page}");

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Failed to fetch users. StatusCode: {response.StatusCode}");

            pageResponse = await response.Content.ReadFromJsonAsync<ReqresListUsersResponse>();

            if (pageResponse?.Data != null)
                users.AddRange(pageResponse.Data);

            page++;
        } while (pageResponse != null && page <= pageResponse.Total_Pages);


        return users;
    }
}
}
