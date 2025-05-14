using DüsseldorferSchülerinventar.Models;
using System.Text.Json;

namespace DüsseldorferSchülerinventar.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://ihredomain.de/api/";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<User> Login(string username, string password)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}login.php?username={username}&password={password}");
            return await HandleResponse<User>(response);
        }

        public async Task<List<NormTableItem>> GetNormTable()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}readNormTable.php");
            return await HandleResponse<List<NormTableItem>>(response);
        }

        public async Task<bool> SaveProfile(Profile profile)
        {
            var json = JsonSerializer.Serialize(profile);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{BaseUrl}saveProfile.php", content);
            return response.IsSuccessStatusCode;
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiResponse<T>>(json);
            
            return result?.Status == "success" ? result.Data : default;
        }

        private class ApiResponse<T>
        {
            public string Status { get; set; }
            public T Data { get; set; }
        }
    }
}