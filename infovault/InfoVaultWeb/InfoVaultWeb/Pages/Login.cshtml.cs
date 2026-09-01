using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfoVaultWeb.Services;

namespace InfoVaultWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApiService _api;
        public LoginModel(ApiService api) => _api = api;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var token = await _api.LoginAsync(Username, Password);
                if (token == null)
                {
                    ErrorMessage = "Invalid username or password.";
                    return Page();
                }

                HttpContext.Session.SetString("Token", token);
                return RedirectToPage("Folders");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Could not connect to the API. Make sure the backend (INFOVUALT API) is running.";
                return Page();
            }
        }
    }
}