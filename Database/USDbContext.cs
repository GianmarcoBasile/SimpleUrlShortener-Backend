using Microsoft.EntityFrameworkCore;
using URLShortener.Models;

namespace URLShortener.Database
{
    public class USDbContext : DbContext
    {
        public USDbContext(DbContextOptions<USDbContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>()
                .HasIndex(u => u.Id)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();
            modelBuilder.Entity<Url>()
                .HasOne<User>(u => u.Creator)
                .WithMany(user => user.Urls)
                .HasForeignKey(u => u.CreatorId);
        }

        // User specific methods
        public async Task<bool> isUsernamePresent(string username)
        {
            try
            {
                return await this.Users.FirstOrDefaultAsync<User>(user => user.Username == username) != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<bool> isEmailPresent(string email)
        {
            try
            {
                return await this.Users.FirstOrDefaultAsync<User>(user => user.Email == email) != null ? true : false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        // URL Shortener specific methods
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
