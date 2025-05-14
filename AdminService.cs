using DüsseldorferSchülerinventar.Models;
using System.Text.Json;

namespace DüsseldorferSchülerinventar.Services
{
    public class AdminService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://ihredomain.de/api/";

        public AdminService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<NormTable>> GetNormTablesAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}readNormTable.php");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NormTable>>(json);
            }
            return new List<NormTable>();
        }

        public async Task<bool> UpdateNormTablesAsync(IEnumerable<NormTable> tables)
        {
            var json = JsonSerializer.Serialize(tables);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{BaseUrl}updateNormTable.php", content);
            return response.IsSuccessStatusCode;
        }
    }
}