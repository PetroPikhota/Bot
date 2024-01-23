using Newtonsoft.Json;

namespace Bot_start.Controlers.RedditControllers
{
    public class UpdateImagesFromReddit
    {
        readonly string clientId;
        readonly string clientSecret;
        readonly string userAgent;
        readonly string subredditName = "memes";
        int postCount = 10;

        public UpdateImagesFromReddit()
        {
            clientId = LoginParameters.GetReddit().clientID;
            clientSecret = LoginParameters.GetReddit().clientSecret;
            userAgent = LoginParameters.GetReddit().userAgent;
        }

        public bool UpdateImages()
        {
            bool result = true;
            try
            {
                string accessToken = GetAccessToken(clientId, clientSecret, userAgent).Result;

                List<RedditPost> posts = GetRedditPosts(subredditName, postCount, userAgent, accessToken).Result;

                DownloadAndSaveImages(posts);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }


        async Task<string> GetAccessToken(string clientId, string clientSecret, string userAgent)
        {
            string authUrl = "https://www.reddit.com/api/v1/access_token";
            var httpClient = new HttpClient();

            var authData = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("grant_type", "client_credentials") });

            string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);

            var authResponse = httpClient.PostAsync(authUrl, authData);
            string authResponseContent = authResponse.Result.Content.ReadAsStringAsync().Result;

            var tokenInfo = JsonConvert.DeserializeObject<RedditTokenResponse>(authResponseContent);
            return tokenInfo.AccessToken;
        }
        async Task<List<RedditPost>> GetRedditPosts(string subredditName, int postCount, string userAgent, string accessToken)
        {
            string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/new.json?limit={postCount}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");

            var response = await httpClient.GetStringAsync(apiUrl);
            var responseData = JsonConvert.DeserializeObject<RedditResponse>(response);

            return responseData.Data.Children.ConvertAll(child => child.Data);
        }

        async Task DownloadAndSaveImages(List<RedditPost> posts)
        {
            foreach (var post in posts)
            {
                if (post.Url.EndsWith(".jpg") || post.Url.EndsWith(".jpeg"))
                {
                    await DownloadAndSaveImage(post.Url, post.Id);
                }
            }
        }
        async Task DownloadAndSaveImage(string imageUrl, string postId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);

                string fileName = $"images/{postId}.jpg";
                File.WriteAllBytes(fileName, imageBytes);

                Console.WriteLine($"Image saved: {fileName}");
            }
        }
    }

    class RedditTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    class RedditResponse
    {
        public RedditData Data { get; set; }
    }

    class RedditData
    {
        public List<RedditPostData> Children { get; set; }
    }

    class RedditPostData
    {
        public RedditPost Data { get; set; }
    }

    class RedditPost
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Id { get; set; }
    }
}