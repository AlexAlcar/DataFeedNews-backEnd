using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private static readonly Dictionary<string, DateTime> LastRequests = new Dictionary<string, DateTime>();
        private static readonly object LockObject = new object();
        private const int RequestLimit = 100;
        private const int RequestLimitMinutes = 1;

        [HttpGet]
        public IActionResult GetNewsFromRSS(string rssUrl)
        {
            if (string.IsNullOrEmpty(rssUrl))
                return BadRequest("RSS URL cannot be empty");

            rssUrl = CheckURL(rssUrl);
            if (rssUrl == null)
                return BadRequest("Invalid RSS URL");

            try
            {
                // Límite de solicitudes
                if (!IsAllowedToRequest(rssUrl))
                    return StatusCode(429, "Too many requests. Please try again later.");

                var newsList = new List<object>();
                var oneWeekAgo = DateTime.Now.AddDays(-7);

                using (var reader = XmlReader.Create(rssUrl))
                {
                    var feed = SyndicationFeed.Load(reader);
                    if (feed == null)
                        return BadRequest("Invalid RSS URL or no feed found.");

                    foreach (var item in feed.Items.Where(feedItem => feedItem.PublishDate > oneWeekAgo))
                    {
                        var newsItem = new
                        {
                            Title = item.Title.Text,
                            Content = item.Summary?.Text,
                            Date = item.PublishDate.DateTime,
                            URL = item.Links.FirstOrDefault()?.Uri
                        };
                        newsList.Add(newsItem);
                    }
                }

                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving news: {ex.Message}");
            }
        }

        private static string CheckURL(string url)
        {
            var regex = new Regex(@"^(http|https):\/\/[^/\s]+\/.*$");
            if (regex.IsMatch(url))
            {
                return url;
            }
            else
            {
                return null;
            }
        }

        private bool IsAllowedToRequest(string rssUrl)
        {
            lock (LockObject)
            {
                // Obtener la hora de la última solicitud
                if (LastRequests.TryGetValue(rssUrl, out DateTime lastRequestTime))
                {
                    // Verificar límite de solicitudes dentro del período de tiempo especificado
                    if (DateTime.Now.Subtract(lastRequestTime).TotalSeconds < 5)
                    {
                        return false;
                    }
                }
                LastRequests[rssUrl] = DateTime.Now;
                return LastRequests.Count(req => req.Key == rssUrl && DateTime.Now.Subtract(req.Value).TotalMinutes < RequestLimitMinutes) < RequestLimit;
            }
        }
    }
}
