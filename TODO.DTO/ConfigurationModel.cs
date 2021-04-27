using System;
using System.Collections.Generic;
using System.Text;

namespace TODO.DTO
{
    public class ConfigurationModel
    {
        public class ConfigurationLogging
        {
            public LogFile LogFile { get; set; }
        }
        public class LogFile
        {
            public string Path { get; set; }
        }

        public class ConfigurationConnectionStrings
        {
            public string TODO { get; set; }
        }
    }
}
