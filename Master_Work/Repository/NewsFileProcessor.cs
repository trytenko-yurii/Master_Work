using CsvHelper;
using CsvHelper.Configuration;
using Master_Work.Entities;
using System.Formats.Asn1;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Master_Work.Repository
{
    public class NewsFileProcessor
    {
        public static List<NewsArticle> LoadNewsFromFile(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<NewsArticle>().ToList();

                // Remove links from all string fields
                foreach (var record in records)
                {
                    record.title = RemoveLinks(record.title);
                    record.text = RemoveLinks(record.text);
                    record.subject = RemoveLinks(record.subject);
                }

                return records;
            }
        }

        // Helper method to remove links from a string
        private static string RemoveLinks(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Regular expression to match URLs
            string urlPattern = @"https?://[^\s]+";
            return Regex.Replace(input, urlPattern, string.Empty).Trim();
        }
    }
}
