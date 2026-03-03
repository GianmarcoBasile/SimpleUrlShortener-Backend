using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace URLShortener.Models
{
    public class Url
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public User Creator { get; set; } = null!;
        public required string ShortURL { get; set; }
        public required string OriginalURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
