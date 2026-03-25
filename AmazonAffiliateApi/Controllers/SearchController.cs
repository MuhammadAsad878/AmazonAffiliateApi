using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AmazonAffiliateApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SearchController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> SearchAmazon([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return BadRequest("Query is required.");

            // 1. Safely pull your credentials from appsettings.json
            var rapidApiKey = _configuration["RapidApi:Key"];
            var rapidApiHost = _configuration["RapidApi:Host"];
            var affiliateTag = _configuration["Amazon:AffiliateTag"];

            var client = _httpClientFactory.CreateClient();

            // 2. Format the exact URL for the "Real-Time Amazon Data" API
            // Note: I added &country=US to match your snippet's parameters
            var requestUri = $"https://{rapidApiHost}/search?query={Uri.EscapeDataString(query)}&page=1&country=US";

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri),
                Headers =
                {
                    { "x-rapidapi-key", rapidApiKey },
                    { "x-rapidapi-host", rapidApiHost },
                },
            };

            using var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500, "Error fetching data from Amazon Scraper.");
            }

            var body = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(body);

            // 3. This specific API usually puts the results inside a "data" then "products" array
            var products = jsonResponse["data"]?["products"] ?? new JArray();

            var resultList = new List<object>();

            foreach (var item in products)
            {
                var originalUrl = item["product_url"]?.ToString();

                // 4. Inject your affiliate tag
                var affiliateUrl = originalUrl != null
                    ? $"{originalUrl}{(originalUrl.Contains("?") ? "&" : "?")}tag={affiliateTag}"
                    : string.Empty;

                resultList.Add(new
                {
                    Title = item["product_title"]?.ToString(),
                    Price = item["product_price"]?.ToString(),
                    Image = item["product_photo"]?.ToString(),
                    Url = affiliateUrl
                });
            }

            return Ok(resultList);
        }
    }
}