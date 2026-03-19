using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Helpdesk.Utils
{
    public class LogWriter
    {
        private string LogPath;
        public LogWriter()
        {
            string logFolder = Environment.CurrentDirectory + @"\Logs\";
            string date = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            LogPath = Path.Combine(logFolder, string.Format("Log_{0}.txt", date));

            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }
            if (!File.Exists(LogPath))
            {
                File.Create(LogPath).Close();
            }
        }
        public void WriteLog(string logMessage)
        {
            File.AppendAllText(LogPath, string.Format("[{0}]{1} {2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), logMessage, Environment.NewLine));
        }
    }
}
