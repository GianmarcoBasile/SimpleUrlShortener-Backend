namespace URLShortener.Services
{
    public class UrlHelper
    {
        private static string availableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string generateShortCode(int length = 6)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(availableChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());

        }
    }
}
