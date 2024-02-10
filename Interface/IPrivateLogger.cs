namespace Bot_start.Interface
{
    public interface IPrivateLogger
    {
        void LOG(string message);
        void LOG(string functionName, string message);
        void LOG_Message(string message);
    }
}
