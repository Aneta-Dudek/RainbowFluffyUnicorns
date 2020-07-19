namespace Questore.Data.Logger
{
    public interface ILog
    {
        void LogException(string path, string message, string stackTrace);
    }
}
