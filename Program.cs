using Bot_start;
using Bot_start.Controlers;

class Program
{
    static async Task Main()
    {       
        PrapereConfig();
        BotMain botMain = new BotMain();
        await botMain.StartReceiver();
    }
    
    static void PrapereConfig()
    {
        if (!Directory.Exists("./images"))
        {
            DirectoryInfo di = Directory.CreateDirectory("./images");
        }

        DbController _db = new DbController();
    }
}
