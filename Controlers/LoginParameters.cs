using Bot_start.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bot_start.Controlers
{
    public static class LoginParameters
    {
        private static TelegramModel _telegram = null;
        private static RedditModel _reddit = null;
        private static LogOptions _logOptions = null;
        private static string _connectionString = null;

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

        public static string GetConnectioString()
        {
            if( _connectionString == null)
            {
                GetJsonParameters();
            }
            return _connectionString;
        }

        public static LogOptions GetLogOptions()
        {
            if (_logOptions == null)
            {
                GetJsonParameters();
            }
            return _logOptions;
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
                        _telegram = loginDataModel.Telegram;
                        _reddit = loginDataModel.Reddit;
                        _connectionString = loginDataModel.connectionStrings.WebApiDatabase;
                        _logOptions = loginDataModel.LogOptions;
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
