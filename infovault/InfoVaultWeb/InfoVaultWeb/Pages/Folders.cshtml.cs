using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfoVaultWeb.Services;
using InfoVaultWeb.Models;

namespace InfoVaultWeb.Pages
{
    public class FoldersModel : PageModel
    {
        private readonly ApiService _api;
        public FoldersModel(ApiService api) => _api = api;

        public List<Folder> Folders { get; set; } = new();

        [BindProperty]
        public string NewFolderName { get; set; } = string.Empty;

        private string? Token => HttpContext.Session.GetString("Token");

        public async Task<IActionResult> OnGetAsync()
        {
            if (Token == null) return RedirectToPage("Login");
            Folders = await _api.GetFoldersAsync(Token);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (Token == null) return RedirectToPage("Login");
            await _api.CreateFolderAsync(Token, NewFolderName);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (Token == null) return RedirectToPage("Login");
            await _api.DeleteFolderAsync(Token, id);
            return RedirectToPage();
        }
    }
}