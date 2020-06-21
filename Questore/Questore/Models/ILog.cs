namespace Questore.Models
{
    public interface ILog
    {
        void LogException(string path, string message, string stackTrace);
    }
}
