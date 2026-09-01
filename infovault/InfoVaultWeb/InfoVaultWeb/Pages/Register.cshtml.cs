using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InfoVaultWeb.Services;

namespace InfoVaultWeb.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ApiService _api;
        public RegisterModel(ApiService api) => _api = api;

        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? Message { get; set; }

        public void OnGet() { }
        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Please enter both a username and password.";
                return Page();
            }

            try
            {
                var (success, details) = await _api.RegisterAsync(Username, Password);
                Message = success ? "Registered! You can now log in." : details;
                if (success) return RedirectToPage("Login");
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Could not connect to the API. Make sure the backend (INFOVUALT API) is running.";
                return Page();
            }
        }
       
        }
    
}