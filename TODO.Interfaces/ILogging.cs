using System;
using System.Collections.Generic;
using System.Text;

namespace TODO.Interfaces
{
    public interface ILogging
    {
        void WriteErr(Exception exception);
    }
}
