using Bot_start.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot_start.Controlers
{
    public static class LoginParameters
    {
        private static TelegramModel _telegram = null;
        private static RedditModel _reddit = null; 
        public static TelegramModel GetTelegram()
        {
            if( _telegram == null)
            {
                GetJsonParameters();
            }
            return _telegram;
        }

        public static RedditModel GetReddit()
        {
            if (_reddit == null)
            {
                GetJsonParameters();
            }
            return _reddit;
        }
        private static void GetJsonParameters()
        {
            string jsonData = getLoginJson();
            if (!String.IsNullOrEmpty(jsonData))
            {
                try
                {
                    LoginDataModel loginDataModel = JsonConvert.DeserializeObject<LoginDataModel>(jsonData);
                    if (loginDataModel != null)
                    {
                        _telegram = loginDataModel.Teelgram;
                        _reddit = loginDataModel.Reddit;
                    }
                    else
                    {
                        File.AppendAllText("Log.txt", "loginDataModel variable is null");
                    }
                }
                catch(Exception ex)
                {
                    File.AppendAllText("Log.txt", ex.Message);
                }                
            }
            else
            {
                File.AppendAllText("Log.txt", "jsonData variable is null or Empty");
            }
        }
        private static string getLoginJson()
        {
            string jsonString = null;
            try
            {
                jsonString = File.ReadAllText("LoginData.json");
            }
            catch (Exception ex)
            {
                File.AppendAllText("Log.txt", ex.Message);
            }
            return jsonString;
        }
    }
}
