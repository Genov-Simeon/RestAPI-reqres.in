using Models;
using RestSharp;

namespace Services
{
    public class ApiClient
    {
        private readonly RestClient _client;
        private readonly string _baseUrl;

        public ApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _client = new RestClient(_baseUrl);
        }

        public async Task<GetUsersListResponse> GetUsers(int page)
        {
            var request = new RestRequest($"users", Method.Get)
                .AddQueryParameter("page", page);
            
            var response = await _client.ExecuteAsync<GetUsersListResponse>(request);
            
            if (!response.IsSuccessful)
                throw new Exception($"Failed to get users: {response.ErrorMessage}");

            return response.Data;
        }

        public async Task<GetUserResponse> GetUserById(int userId)
        {
            var request = new RestRequest($"users/{userId}", Method.Get);
            var response = await _client.ExecuteAsync<GetUserResponse>(request);

            if (!response.IsSuccessful)
                throw new Exception($"Failed to get user: {response.ErrorMessage}");

            return response.Data;
        }

        public async Task<CreateUserResponse> CreateUser(UserRequest userRequest)
        {
            var request = new RestRequest("users", Method.Post)
                .AddJsonBody(userRequest);
            var response = await _client.ExecuteAsync<CreateUserResponse>(request);

            if (!response.IsSuccessful)
                throw new Exception($"Failed to create user: {response.ErrorMessage}");

            return response.Data;
        }

        public async Task DeleteUser(int userId)
        {
            var request = new RestRequest($"users/{userId}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            if (!response.IsSuccessful)
                throw new Exception($"Failed to delete user: {response.ErrorMessage}");
        }
    }
} 