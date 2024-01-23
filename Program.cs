using Bot_start;

class Program
{
    static async Task Main()
    {       
        BotMain botMain = new BotMain();
        await botMain.StartReceiver();
    }
    
    static void PrapereConfig()
    {
        if (!Directory.Exists("./images"))
        {
            DirectoryInfo di = Directory.CreateDirectory("./images");
        }
    }
}
