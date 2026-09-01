namespace INFOVUALT.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        
        [System.ComponentModel.DataAnnotations.MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}