namespace Questore.Logger
{
    public interface ILog
    {
        void LogException(string path, string message, string stackTrace);
    }
}
