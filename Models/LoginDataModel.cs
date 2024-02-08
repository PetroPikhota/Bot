namespace Bot_start.Models
{
    public class LoginDataModel
    {
        public RedditModel Reddit { get; set; }
        public TelegramModel Telegram { get; set; }
        public ConnectionStrings connectionStrings { get; set; }
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

    public class ConnectionStrings
    {
        public string WebApiDatabase { get; set; }
    }
}
