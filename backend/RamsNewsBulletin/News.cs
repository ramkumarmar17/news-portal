using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using Newtonsoft.Json;

namespace RamsNewsBulletin
{
    public static class News
    {
        const string DefaultCategory = "technology";
        const string NewAPIKey = "<API_KEY>";

        [FunctionName("News")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            
            log.Info("C# HTTP trigger function processed a request.");

            var queryParams = req.GetQueryNameValuePairs();

            var category = queryParams.FirstOrDefault(q => string.Compare(q.Key, "category", true) == 0).Value;

            if (string.IsNullOrWhiteSpace(category))
                category = DefaultCategory;

            Categories categoryEnum = News.GetCategoryEnum(category.Trim().ToLower());

            var articles = News.GetArticlesFromNewsApi(NewAPIKey, categoryEnum);

            HttpResponseMessage response = req.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(articles));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/json");

            if (req.Headers.Contains("Origin"))
            {
                var origin = req.Headers.GetValues("Origin").FirstOrDefault();

                response.Headers.Add("Access-Control-Allow-Credentials", "true");
                response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:8080");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            }

            return response;
        }

        private static Categories GetCategoryEnum(string categoryName)
        {
            if (categoryName == "technology")
            {
                return Categories.Technology;
            }
            if (categoryName == "business")
            {
                return Categories.Business;
            }
            if (categoryName == "science")
            {
                return Categories.Science;
            }
            if (categoryName == "sports")
            {
                return Categories.Sports;
            }
            if (!(categoryName == "entertainment"))
            {
                return Categories.Technology;
            }
            return Categories.Entertainment;
        }

        private static List<Article> GetArticlesFromNewsApi(string apiKey, Categories category)
        {
            NewsApiClient client = new NewsApiClient(apiKey);

            TopHeadlinesRequest request = new TopHeadlinesRequest();
            request.Country = new Countries?(Countries.IN);
            request.Language = new Languages?(Languages.EN);
            request.PageSize = 100;
            request.Category = new Categories?(category);

            ArticlesResult topHeadlines = client.GetTopHeadlines(request);
            if (topHeadlines.Status == Statuses.Ok)
            {
                return topHeadlines.Articles;
            }
            return new List<Article>();
        }
    }
}
