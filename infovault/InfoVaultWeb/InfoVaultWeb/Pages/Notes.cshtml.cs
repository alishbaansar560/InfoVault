using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfoVaultWeb.Services;
using InfoVaultWeb.Models;

namespace InfoVaultWeb.Pages
{
    public class NotesModel : PageModel
    {
        private readonly ApiService _api;
        public NotesModel(ApiService api) => _api = api;

        public List<Note> Notes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int FolderId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FolderName { get; set; } = string.Empty;

        [BindProperty]
        public string NewTitle { get; set; } = string.Empty;

        [BindProperty]
        public string NewContent { get; set; } = string.Empty;

        private string? Token => HttpContext.Session.GetString("Token");

        public async Task<IActionResult> OnGetAsync()
        {
            if (Token == null) return RedirectToPage("Login");
            Notes = await _api.GetNotesAsync(Token, FolderId);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (Token == null) return RedirectToPage("Login");
            await _api.CreateNoteAsync(Token, FolderId, NewTitle, NewContent);
            return RedirectToPage(new { FolderId, FolderName });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id, int folderId, string folderName)
        {
            if (Token == null) return RedirectToPage("Login");
            await _api.DeleteNoteAsync(Token, id);
            return RedirectToPage(new { FolderId = folderId, FolderName = folderName });
        }
    }
}