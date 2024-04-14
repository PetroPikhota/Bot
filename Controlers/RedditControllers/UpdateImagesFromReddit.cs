using Bot_start.Interface;
using Bot_start.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Bot_start.Controlers.RedditControllers
{
    public class UpdateImagesFromReddit
    {
        private readonly IPrivateLogger _logger = MyLogger.GetLogger();
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string userAgent;
        private readonly string subredditName = "memes";
        private readonly int postCount = 10;
        private readonly List<Item> items = new();

        public UpdateImagesFromReddit()
        {
            clientId = LoginParameters.GetReddit().clientID;
            clientSecret = LoginParameters.GetReddit().clientSecret;
            userAgent = LoginParameters.GetReddit().userAgent;
        }

        public bool UpdateImages(ref int i)
        {
            bool result = true;
            try
            {
                _logger.LOG($"Start {nameof(UpdateImages)} Class");
                string accessToken = GetAccessToken(clientId, clientSecret, userAgent);
                _logger.LOG($"{nameof(UpdateImages)} After GetAccessToken");
                List<RedditPost> posts = GetRedditPosts(subredditName, postCount, userAgent, accessToken).Result;
                i = posts.Count;
                _ = DownloadAndSaveImages(posts);

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        private string GetAccessToken(string clientId, string clientSecret, string userAgent)
        {
            string authUrl = "https://www.reddit.com/api/v1/access_token";
            HttpClient httpClient = new();

            FormUrlEncodedContent authData = new(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });

            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

            Task<HttpResponseMessage> authResponse = httpClient.PostAsync(authUrl, authData);
            string authResponseContent = authResponse.Result.Content.ReadAsStringAsync().Result;

            RedditTokenResponse? tokenInfo = JsonConvert.DeserializeObject<RedditTokenResponse>(authResponseContent);
            return tokenInfo.AccessToken;
        }

        private async Task<List<RedditPost>> GetRedditPosts(string subredditName, int postCount, string userAgent, string accessToken)
        {
            string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/new.json?limit={postCount}";
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");

            string response = await httpClient.GetStringAsync(apiUrl);
            RedditResponse? responseData = JsonConvert.DeserializeObject<RedditResponse>(response);

            return responseData.Data.Children.ConvertAll(child => child.Data);
        }

        private async Task DownloadAndSaveImages(List<RedditPost> posts)
        {
            AddDataToDb();
            foreach (RedditPost post in posts)
            {
                if (post.Url.EndsWith(".jpg") || post.Url.EndsWith(".jpeg"))
                {
                    await DownloadAndSaveImage(post.Url);
                    items.Add(new Item() { Path = post.Url });
                }

            }
        }

        private async Task DownloadAndSaveImage(string imageUrl)
        {
            _logger.LOG($"Start {nameof(DownloadAndSaveImage)}");
            try
            {
                using HttpClient httpClient = new();
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                Regex regex = new("([a-zA-Z0-9]+)\\.(jpeg|jpg)(?=$|\\?)");
                string shorFileName = regex.Match(imageUrl).Value.Split('.')[0];
                string fileName = $"images/{shorFileName}.jpg";
                File.WriteAllBytes(fileName, imageBytes);
                _logger.LOG($"Saved: {imageUrl}");
            }
            catch (Exception ex)
            {
                _logger.LOG($"{nameof(DownloadAndSaveImage)}: {ex.Message}");
            }
        }

        private void AddDataToDb()
        {
            _logger.LOG("Start AddDataToDb");
            try
            {
                DbController dbController = new();
                AppDbContext DB = dbController.GetDb();
                _logger.LOG($"before remove items.Count = {items.Count}");
                _ = items.RemoveAll(DB.Items.ToList().Contains);
                _logger.LOG($"after remove items.Count = {items.Count}");
                if (items.Count > 0)
                {
                    DB.Items.AddRange(items);
                    int saved = DB.SaveChanges();
                    _logger.LOG($"{nameof(UpdateImagesFromReddit)}: Aded to db {saved} new records");
                }
            }
            catch (Exception ex)
            {
                _logger.LOG($"{nameof(UpdateImagesFromReddit)}: {ex.Message}");
            }
            _logger.LOG($"{nameof(UpdateImagesFromReddit)}: EOF");
        }
    }

    internal class RedditTokenResponse
    {
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }
    }

    internal class RedditResponse
    {
        public required RedditData Data { get; set; }
    }

    internal class RedditData
    {
        public required List<RedditPostData> Children { get; set; }
    }

    internal class RedditPostData
    {
        public required RedditPost Data { get; set; }
    }

    internal class RedditPost
    {
        public required string Title { get; set; }
        public required string Url { get; set; }
        public required string Id { get; set; }
    }
}