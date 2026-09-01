namespace InfoVaultWeb.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
