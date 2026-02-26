using Microsoft.AspNetCore.Mvc;
using URLShortener.Database;
using URLShortener.Models;
using URLShortener.Services;

namespace URLShortener.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class URLController : ControllerBase
    {
        private readonly UrlDbContext _context;
        public URLController(UrlDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrlRequest request)
        {
            string shortCode;
            do
            {
                shortCode = UrlHelper.generateShortCode();
            } while (await _context.isShortCodePresent(shortCode));

            string originalUrl = request.OriginalUrl;

            Url shortUrl = new Url
            {
                OriginalURL = originalUrl.StartsWith("http") ? $"http://{originalUrl}" : $"https://{originalUrl}",
                ShortURL = shortCode
            };

            _context.Urls.Add(shortUrl);
            await _context.SaveChangesAsync();

            return Ok(shortUrl);
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> GetOriginalUrl(string shortCode)
        {
            Url? newUrl = await _context.GetOriginalUrl(shortCode);

            if (newUrl == null)
            {
                return NotFound();
            }

            Console.WriteLine($"Redirecting to {newUrl.OriginalURL}");

            return Redirect(newUrl.OriginalURL);
        }
    }
}
