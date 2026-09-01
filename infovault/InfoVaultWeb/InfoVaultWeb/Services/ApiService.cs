using System.Net.Http.Json;
using System.Net.Http.Headers;
using InfoVaultWeb.Models;

namespace InfoVaultWeb.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;

        private const string BaseUrl = "https://infovault-a6d2h7dscpccf2gj.eastasia-01.azurewebsites.net";

        public ApiService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(BaseUrl);
        }

        public void SetToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("auth/login", new { username, password });
            if (!response.IsSuccessStatusCode) return null;

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result?.Token;
        }

        public async Task<(bool Success, string Details)> RegisterAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("auth/register", new { username, password });
            var body = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, $"Status: {response.StatusCode} | Body: {body}");
        }

        public async Task<List<Folder>> GetFoldersAsync(string token)
        {
            SetToken(token);
            return await _client.GetFromJsonAsync<List<Folder>>("folders") ?? new();
        }

        public async Task<bool> CreateFolderAsync(string token, string name)
        {
            SetToken(token);
            var response = await _client.PostAsJsonAsync("folders", new { name });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteFolderAsync(string token, int id)
        {
            SetToken(token);
            var response = await _client.DeleteAsync($"folders/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Note>> GetNotesAsync(string token, int folderId)
        {
            SetToken(token);
            return await _client.GetFromJsonAsync<List<Note>>($"notes/folder/{folderId}") ?? new();
        }

        public async Task<bool> CreateNoteAsync(string token, int folderId, string title, string content)
        {
            SetToken(token);
            var response = await _client.PostAsJsonAsync("notes", new { folderId, title, content });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteNoteAsync(string token, int id)
        {
            SetToken(token);
            var response = await _client.DeleteAsync($"notes/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    public class LoginResponse
    {
        public string? Token { get; set; }
    }
}