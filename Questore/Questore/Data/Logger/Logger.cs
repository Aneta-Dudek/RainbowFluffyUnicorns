using System;
using System.IO;
using System.Text;

namespace Questore.Data.Logger
{
    public class Logger : ILog
    {
        private static readonly Lazy<Logger> instance = new Lazy<Logger>();
        public static Logger Instance => instance.Value;

        public void LogException(string path, string message, string stackTrace)
        {
            var fileName = $"Exception_{DateTime.Now.ToShortDateString()}.log";
            var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{fileName}";
            var sb = new StringBuilder()
                .AppendLine("-----------------------------------")
                .AppendLine(DateTime.Now.ToString())
                .AppendLine($"Path: {path}")
                .AppendLine($"Message: {message}")
                .AppendLine($"Stack Trace: {stackTrace}");

            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            using StreamWriter writer = new StreamWriter(filePath, true);

            writer.Write(sb.ToString());
            writer.Flush();
        }
    }
}
