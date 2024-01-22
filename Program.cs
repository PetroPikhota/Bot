using Bot_start;

class Program
{
    

    static async Task Main()
    {       
        BotMain botMain = new BotMain();
        await botMain.StartReceiver();
    }

    
}
