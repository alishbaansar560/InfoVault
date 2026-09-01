namespace INFOVUALT.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public List<Folder> Folders { get; set; } = new();
    }
}
