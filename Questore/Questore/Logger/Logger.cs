using System;
using System.IO;
using System.Text;

namespace Questore.Logger
{
    public class Logger : ILog
    {
        private static readonly Lazy<Logger> instance = new Lazy<Logger>();
        public static Logger GetInstance => instance.Value;

        public void LogException(string path, string message, string stackTrace)
        {
            string fileName = $"Exception_{DateTime.Now.ToShortDateString()}.log";
            string filePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{fileName}";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-----------------------------------");
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine($"Path: {path}");
            sb.AppendLine($"Message: {message}");
            sb.AppendLine($"Stack Trace: {stackTrace}");
            //using StreamWriter writer = new StreamWriter(filePath, true);
            //writer.Write(sb.ToString());
            //writer.Flush();
        }
    }
}
