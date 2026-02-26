using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Database
{
    public class UrlDbContext : DbContext
    {
        public UrlDbContext(DbContextOptions<UrlDbContext> options) : base(options) { }
        public DbSet<Url> Urls { get; set; }
        public async Task<bool> isShortCodePresent(string shortCode)
        {
            try
            {
                return await this.Urls.FirstOrDefaultAsync<Url>(url => url.ShortURL == shortCode) != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Url?> GetOriginalUrl(string shortCode)
        {
            try
            {
                return await this.Urls.FirstOrDefaultAsync<Url>(url => url.ShortURL == shortCode);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
