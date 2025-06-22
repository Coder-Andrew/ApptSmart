using System.Text.RegularExpressions;

namespace ApptSmartBackend.Helpers
{
    /// <summary>
    /// A Slugify helper class designed to convert string into url-friendly representations.
    /// </summary>
    public static class SlugHelper
    {
        /// <summary>
        /// Converts a string into a url safe representation
        /// 
        /// Example: "John's Pizzeria-Cafe & Subs" -> "johns-pizzeria-cafe-subs"
        /// </summary>
        /// <param name="phrase">The string to be slugified</param>
        /// <returns>The slugified string</returns>
        public static string Slugify(string phrase)
        {

            string str = phrase.ToLowerInvariant();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", "-").Trim();
            str = Regex.Replace(str, @"-+", "-");
            return str;
        }
    }
}
