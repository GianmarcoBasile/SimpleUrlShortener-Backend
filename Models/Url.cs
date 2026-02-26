namespace URLShortener.Models
{
    public class Url
    {
        public int Id { get; set; }
        public required string ShortURL { get; set; }
        public required string OriginalURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
