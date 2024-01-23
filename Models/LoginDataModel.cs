namespace Bot_start.Models
{
    public class LoginDataModel
    {
        public RedditModel Reddit { get; set; }
        public TelegramModel Teelgram { get; set; }
    }
    public class RedditModel
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }
        public string userAgent { get; set; }
    }

    public class TelegramModel
    {
        public string botToken { get; set; }
    }
}
