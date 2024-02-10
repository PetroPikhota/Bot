using Bot_start.Interface;
using System;

namespace Bot_start.Controlers
{
    public static class MyLogger 
    {
        public static IPrivateLogger GetLogger()
        {
            if (!Directory.Exists("./Log"))
            {
                DirectoryInfo di = Directory.CreateDirectory("./Log");
            }
            return new FileLogger();
        }
    }
}
