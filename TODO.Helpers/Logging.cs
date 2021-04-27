using Microsoft.Extensions.Options;
using System;
using System.IO;
using TODO.Interfaces;
using static TODO.DTO.ConfigurationModel;

namespace TODO.Helpers
{
    public class Logging : ILogging
    {
        private readonly IOptions<ConfigurationLogging> _appSettingPath;
        public Logging(IOptions<ConfigurationLogging> appSettingPath)
        {
            _appSettingPath = appSettingPath;
        }
        public void WriteErr(Exception objError)
        {
            StreamWriter oSw;
            string sErrMsg = "";
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            try
            {
                if (!Directory.Exists(_appSettingPath.Value.LogFile.Path))
                {
                    Directory.CreateDirectory(_appSettingPath.Value.LogFile.Path);
                }

                string _logFilePath = _appSettingPath.Value.LogFile.Path + System.DateTime.Today.ToString("yyyy-MM-dd") + "." + "txt";
                logFileInfo = new FileInfo(_logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(_logFilePath, FileMode.Append);
                }
                oSw = new StreamWriter(fileStream);
                oSw.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " [E] " + objError.Message);
                oSw.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " [E] " + objError.InnerException);
                oSw.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " [E] " + objError.StackTrace);
                oSw.WriteLine();
                oSw.WriteLine("==========================================================================================================");
                oSw.WriteLine();
                oSw.Close();

            }
            catch (Exception ex)
            {
                sErrMsg = ex.Message;
            }
        }
    }
}
